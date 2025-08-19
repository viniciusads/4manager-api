using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using Moq;
using System.Security.Authentication;

namespace _4Manager.Application.Tests.Features.Users.Handlers
{
    public class LoginRequestCommandHandlerTests
    {
        [Fact]
        public async Task Login_InvalidUser_ThrowsException()
        {
            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var cancellationToken = CancellationToken.None;

            authServiceMock.Setup(a => a.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ThrowsAsync(new AuthenticationException("Erro no login."));

            var handler = new LoginRequestCommandHandler(authServiceMock.Object, userRepositoryMock.Object);

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                handler.Handle(new LoginRequestCommand("email@fake.com", "senha"), CancellationToken.None));
        }

        [Fact]
        public async Task ValidCredentials_ReturnsLoginResponseDto()
        {

            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var cancellationToken = CancellationToken.None;

            var email = "pedro@gmail.com";
            var password = "123456";
            var accessToken = "access-token";
            var refreshToken = "refresh-token";

            authServiceMock
                .Setup(x => x.LoginAsync(email, password))
                .ReturnsAsync(new AuthResult(Guid.NewGuid(), accessToken, refreshToken));

            userRepositoryMock
                .Setup(x => x.GetByEmailAsync(email, cancellationToken))
                .ReturnsAsync(new User { Email = email });

            var handler = new LoginRequestCommandHandler(authServiceMock.Object, userRepositoryMock.Object);
            var command = new LoginRequestCommand(email, password);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(accessToken, result.AccessToken);
            Assert.Equal(refreshToken, result.RefreshToken);
        }

        [Fact]
        public async Task Handle_ThrowsAuthenticationException_WhenAuthServiceFails()
        {

            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var cancellationToken = CancellationToken.None;
            var handler = new LoginRequestCommandHandler(authServiceMock.Object, userRepositoryMock.Object);

            var email = "pedro@gmail.com";
            var password = "123456";

            var command = new LoginRequestCommand(email, password);
            authServiceMock.Setup(a => a.LoginAsync(command.Email, command.Password))
                .ThrowsAsync(new AuthenticationException("E-mail ou senha incorretos."));

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
