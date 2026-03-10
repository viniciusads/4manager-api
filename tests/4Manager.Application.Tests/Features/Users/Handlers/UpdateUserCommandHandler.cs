using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Enums;
using AutoMapper;
using Moq;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using DomainUser = _4Tech._4Manager.Domain.Entities.User;
using SupabaseUser = Supabase.Gotrue.User;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;

        private readonly Mock<IGotrueClient<SupabaseUser, Session>> _authMock;
        private readonly Mock<Supabase.Client> _supabaseClientMock;

        public UpdateUserCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DomainUser, UserResponseDto>();
            });
            _mapper = config.CreateMapper();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authMock = new Mock<IGotrueClient<SupabaseUser, Session>>();
            _supabaseClientMock = new Mock<Supabase.Client>("http://localhost", "fake-key", null);
            _supabaseClientMock.Object.Auth = _authMock.Object;
        }

        [Fact]
        public async Task Handle_UpdateUserLocally_When_IsValid()
        {
            var userId = Guid.NewGuid();
            var accessToken = "valid-access-token";
            var refreshToken = "valid-refresh-token";

            var existingUser = new DomainUser
            {
                UserId = userId,
                Name = "Nome antigo",
                Email = "emailteste@gmail.com",
                Role = RoleEnum.Gestor,
                Position = PositionEnum.AnalistaFuncional
            };

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            _userRepositoryMock
                .Setup(r => r.UpdateUserAsync(It.IsAny<DomainUser>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateUserCommand(
                userId,
                "Novo nome",
                "emailnovoteste@gmail.com",
                PositionEnum.GerenteProjetos,
                accessToken,
                refreshToken
            );

            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper, _supabaseClientMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Novo nome", result.Name);
            Assert.Equal(PositionEnum.GerenteProjetos, existingUser.Position);

            _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.Is<DomainUser>(u => u.Name == "Novo nome"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowException_When_UserNotFound()
        {
            var userId = Guid.NewGuid();

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DomainUser)null!);

            var command = new UpdateUserCommand(userId, "Nome", "emailteste@gmail.com", PositionEnum.GerenteProjetos, "token", "refresh");
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper, _supabaseClientMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("Usuário não encontrado.", exception.Message);

            _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<DomainUser>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Call_SupabaseAuth_When_EmailChanges()
        {
            var userId = Guid.NewGuid();
            var oldEmail = "antigo@email.com";
            var newEmail = "novo@email.com";
            var accessToken = "access-token-xyz";
            var refreshToken = "refresh-token-xyz";

            var existingUser = new DomainUser { UserId = userId, Email = oldEmail, Name = "Test" };

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            _authMock
                .Setup(a => a.SetSession(accessToken, refreshToken, It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ReturnsAsync(new SupabaseUser());

            var command = new UpdateUserCommand(userId, "Test", newEmail, PositionEnum.GerenteProjetos, accessToken, refreshToken);
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper, _supabaseClientMock.Object);

            await handler.Handle(command, CancellationToken.None);

            Assert.Equal(newEmail, existingUser.Email);

            _userRepositoryMock.Verify(r => r.UpdateUserAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);


            _authMock.Verify(a => a.SetSession(accessToken, refreshToken, It.IsAny<bool>()), Times.Once);
            _authMock.Verify(a => a.Update(It.Is<UserAttributes>(u => u.Email == newEmail)), Times.Once);
        }
    }
}