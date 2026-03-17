using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Authentication;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class LoginRequestCommandHandlerTests
    {
        private readonly Mock<ILogger<LoginRequestCommandHandler>> _loggerMock = new();

        [Fact]
        public async Task Login_InvalidUser_ThrowsException()
        {
            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var cancellationToken = CancellationToken.None;

            authServiceMock.Setup(a => a.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ThrowsAsync(new AuthenticationException("Erro no login."));

            var handler = new LoginRequestCommandHandler(authServiceMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                handler.Handle(new LoginRequestCommand("email@fake.com", "senha"), CancellationToken.None));
        }

        [Fact]
        public async Task ValidCredentials_ReturnsLoginResponseDto()
        {

            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var cancellationToken = CancellationToken.None;

            var accessToken = "access-token";
            var refreshToken = "refresh-token";

            authServiceMock
                .Setup(x => x.LoginAsync("pedro@gmail.com", "123456"))
                .ReturnsAsync(new AuthResult(Guid.NewGuid(), accessToken, refreshToken));

            var handler = new LoginRequestCommandHandler(authServiceMock.Object, _loggerMock.Object);
            var command = new LoginRequestCommand("pedro@gmail.com", "123456");

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
            var handler = new LoginRequestCommandHandler(authServiceMock.Object, _loggerMock.Object);

            var command = new LoginRequestCommand("pedro@gmail.com", "123456");
            authServiceMock.Setup(a => a.LoginAsync(command.Email, command.Password))
                .ThrowsAsync(new UnauthorizedAccessException("E-mail ou senha incorretos."));

            var exceptionMessage = "E-mail ou senha incorretos.";
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
