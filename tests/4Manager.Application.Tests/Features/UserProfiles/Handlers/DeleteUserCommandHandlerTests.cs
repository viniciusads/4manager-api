using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<DeleteUserCommandHandler>> _loggerMock = new();

        public DeleteUserCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfile, UserResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Delete_User_And_Return_Dto()
        {
            var userId = Guid.NewGuid();
            var authenticatedUser = userId;
            var user = new UserProfile { 
                UserId = userId,
                Function = RoleEnum.Gestor
            };
 
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);
 
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(a => a.SoftDeleteUserAsync(userId, It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);
 
            var command = new DeleteUserCommand (userId, authenticatedUser);
            var handler = new DeleteUserCommandHandler(mockRepo.Object, mockAuthService.Object, _mapper, _loggerMock.Object);
 
            var result = await handler.Handle(command, CancellationToken.None);
 
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
 
            mockRepo.Verify(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            mockAuthService.Verify(a => a.SoftDeleteUserAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
 
        [Fact]
        public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
        {
            var userId = Guid.NewGuid();
            var authenticatedUserId = userId;

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((UserProfile)null!);
 
            var mockAuthService = new Mock<IAuthService>();
            var handler = new DeleteUserCommandHandler(mockRepo.Object, mockAuthService.Object, _mapper, _loggerMock.Object);
 
            var command = new DeleteUserCommand (userId, authenticatedUserId);
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserException());
 
            var exception = await Assert.ThrowsAsync<UserException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal("Usuário não encontrado.", exception.Message);

            mockAuthService.Verify(a => a.SoftDeleteUserAsync(userId, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowUnauthorized_WhenUserIsNotGestor()
        {
            var userId = Guid.NewGuid();

            var user = new UserProfile
            {
                UserId = userId,
                Function = RoleEnum.Funcionario
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

            var mockAuth = new Mock<IAuthService>();

            var handler = new DeleteUserCommandHandler(
                mockRepo.Object,
                mockAuth.Object,
                _mapper,
                _loggerMock.Object);

            var command = new DeleteUserCommand(userId, userId);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(command, CancellationToken.None));

            mockAuth.Verify(a =>
                a.SoftDeleteUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task ShouldThrow_WhenUserIsNull()
        {
            var userId = Guid.NewGuid();

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((UserProfile?)null);

            var mockAuth = new Mock<IAuthService>();

            var handler = new DeleteUserCommandHandler(
                mockRepo.Object,
                mockAuth.Object,
                _mapper,
                _loggerMock.Object);

            var command = new DeleteUserCommand(userId, userId);

            await Assert.ThrowsAsync<NullReferenceException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}