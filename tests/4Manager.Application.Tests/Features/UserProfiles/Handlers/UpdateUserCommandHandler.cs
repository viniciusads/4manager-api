using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using SupabaseUser = Supabase.Gotrue.User;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UserProfileTestFixture _fixture;
        private readonly Mock<ILogger<UpdateUserCommandHandler>> _loggerMock = new();

        private readonly Mock<IGotrueClient<SupabaseUser, Session>> _authMock;
        private readonly Mock<Supabase.Client> _supabaseClientMock;

        public UpdateUserCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfile, UserResponseDto>();
            });
            _mapper = config.CreateMapper();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authMock = new Mock<IGotrueClient<SupabaseUser, Session>>();
            _supabaseClientMock = new Mock<Supabase.Client>("http://localhost", "fake-key", null);
            _supabaseClientMock.Object.Auth = _authMock.Object;
            _fixture = new UserProfileTestFixture();
        }

        [Fact]
        public async Task Handle_UpdateUserLocally_When_IsValid()
        {
            var userId = Guid.NewGuid();
            var accessToken = "valid-access-token";
            var refreshToken = "valid-refresh-token";
            var oldEmail = "emailteste@gmail.com";
            var newEmail = "emailnovoteste@gmail.com";
            var AuthenticatedUserId = userId;

            var fakeUserProfile = _fixture.GenerateUserProfile(1)[0];
            fakeUserProfile.Function = RoleEnum.Gestor;
            fakeUserProfile.UserId = userId;

            var mockSupabaseUser = new Supabase.Gotrue.User
            {
                Email = oldEmail
            };
            _authMock.SetupGet(a => a.CurrentUser).Returns(mockSupabaseUser);

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeUserProfile);

            _userRepositoryMock
                .Setup(r => r.UpdateUserAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateUserCommand(
                fakeUserProfile.UserId,
                fakeUserProfile.Name = "Nome Teste",
                newEmail,
                fakeUserProfile.Position,
                accessToken,
                refreshToken,
                AuthenticatedUserId
            );

            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper, _supabaseClientMock.Object, _loggerMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);

            _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.Is<UserProfile>(u => u.Name == "Nome Teste"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowException_When_UserNotFound()
        {
            var fakeUserProfile = _fixture.GenerateUserProfile(1)[0];
            var AuthenticatedUserId = fakeUserProfile.UserId;

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(fakeUserProfile.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserProfile)null!);

            var command = new UpdateUserCommand(fakeUserProfile.UserId, fakeUserProfile.Name, "emailteste@gmail.com", fakeUserProfile.Position, "token", "refresh", AuthenticatedUserId);
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper, _supabaseClientMock.Object, _loggerMock.Object);

            var exception = await Assert.ThrowsAsync<UserException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("Usuário não encontrado.", exception.Message);

            _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<UserProfile>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [Fact]
        public async Task Handle_Call_SupabaseAuth_When_EmailChanges()
        {
            var userId = Guid.NewGuid();
            var oldEmail = "antigo@email.com";
            var newEmail = "novo@email.com";
            var newName = "updatedTest";
            var accessToken = "access-token-xyz";
            var refreshToken = "refresh-token-xyz";
            var AuthenticatedUserId = userId;

            var fakeUserProfile = _fixture.GenerateUserProfile(1)[0];
            fakeUserProfile.UserId = userId;

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeUserProfile);

            var mockSupabaseUser = new Supabase.Gotrue.User { 
                Email = oldEmail 
            };

            _authMock.SetupGet(a => a.CurrentUser).Returns(mockSupabaseUser);

            _authMock
                .Setup(a => a.SetSession(accessToken, refreshToken, It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ReturnsAsync(new SupabaseUser());

            var command = new UpdateUserCommand(userId, newName, newEmail, PositionEnum.GerenteProjetos, accessToken, refreshToken, AuthenticatedUserId);
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper, _supabaseClientMock.Object, _loggerMock.Object);

            await handler.Handle(command, CancellationToken.None);

            Assert.Equal(newName, command.Name);

            _userRepositoryMock.Verify(r => r.UpdateUserAsync(fakeUserProfile, It.IsAny<CancellationToken>()), Times.Once);
            _authMock.Verify(a => a.SetSession(accessToken, refreshToken, It.IsAny<bool>()), Times.Once);
            _authMock.Verify(a => a.Update(It.Is<UserAttributes>(u => u.Email == newEmail)), Times.Once);
        }
        
    }
}