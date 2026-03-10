using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public DeleteUserCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Delete_User_And_Return_Dto()
        {
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, Email = "test@example.com" };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(a => a.SoftDeleteUserAsync(userId, It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            var command = new DeleteUserCommand ( userId );
            var handler = new DeleteUserCommandHandler(mockRepo.Object, mockAuthService.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(user.Email, result.Email);

            mockRepo.Verify(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            mockAuthService.Verify(a => a.SoftDeleteUserAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
        {
            var userId = Guid.NewGuid();
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User)null!);

            var mockAuthService = new Mock<IAuthService>();
    
            var handler = new DeleteUserCommandHandler(mockRepo.Object, mockAuthService.Object, _mapper);

            var command = new DeleteUserCommand (userId );
            mockRepo.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
