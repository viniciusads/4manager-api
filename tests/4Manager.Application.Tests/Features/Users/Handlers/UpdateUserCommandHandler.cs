using Moq;
using AutoMapper;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public UpdateUserCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Update_User_And_Return_Dto()
        {
           
            var userId = Guid.NewGuid();
            var name = "Maria Silva";
            var email = "maria@example.com";
            var role = "Admin";
            var password = "novaSenha123";

            var user = new User { UserId = userId, Name = name, Email = email, Role = RoleEnum.Analista };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);
            mockRepo.Setup(r => r.UpdateUserAsync(user, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(a => a.UpdatePasswordAsync(user.UserId, password))
                           .Returns(Task.CompletedTask);

            var command = new UpdateUserCommand(name, email, role, password);
            var handler = new UpdateUserCommandHandler(mockRepo.Object, mockAuthService.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Role.ToString(), result.Role);

            mockRepo.Verify(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()), Times.Once);
            mockAuthService.Verify(a => a.UpdatePasswordAsync(user.UserId, password), Times.Once);
            mockRepo.Verify(r => r.UpdateUserAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
        {
            var name = "João";
            var email = "naoexiste@example.com";
            var role = "User";
            var password = "123456";

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User)null!); 

            var mockAuthService = new Mock<IAuthService>();
            var command = new UpdateUserCommand(name, email, role, password);
            var handler = new UpdateUserCommandHandler(mockRepo.Object, mockAuthService.Object, _mapper);

            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));

            mockRepo.Verify(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()), Times.Once);
            mockAuthService.Verify(a => a.UpdatePasswordAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
            mockRepo.Verify(r => r.UpdateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
