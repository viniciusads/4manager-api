using _4tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;
using System.Security.Authentication;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
{
    public class SignUpUserCommandHandlerTests
    {
        private readonly Mock<IAuthService> _authServiceMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly SignUpUserCommandHandler _handler;

        public SignUpUserCommandHandlerTests()
        {
            _handler = new SignUpUserCommandHandler(_authServiceMock.Object, _userRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_CreatesUser_WhenSignUpIsValid()
        {

            var email = "test@test.com";
            var password = "123456";
            var name = "Teste";
            var confirmPassword = "123456";

            var command = new SignUpUserCommand (email, password, name, confirmPassword);
            var authResult = new AuthResult(Guid.NewGuid(), "access", "refresh");
            var user = new User { UserId = authResult.UserId, Email = command.Email, Name = command.Name };
            var userDto = new UserResponseDto { UserId = user.UserId, Email = user.Email, Name = user.Name };

            _authServiceMock.Setup(a => a.SignUpAsync(command.Email, command.Password))
                .ReturnsAsync(authResult);
            _userRepoMock.Setup(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UserResponseDto>(It.IsAny<User>())).Returns(userDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(userDto.UserId, result.UserId);
            Assert.Equal(userDto.Email, result.Email);
        }

        [Fact]
        public async Task Handle_ThrowsAuthenticationException_WhenSignUpFails()
        {
            var email = "test@test.com";
            var password = "123456";
            var name = "Teste";
            var confirmPassword = "12336";

            var command = new SignUpUserCommand (email, password, name, confirmPassword);
            _authServiceMock.Setup(a => a.SignUpAsync(command.Email, command.Password))
                .ThrowsAsync(new AuthenticationException("Erro ao criar usuário."));

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
