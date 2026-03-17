using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System.Security.Authentication;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class ChangePasswordWhenLoggedCommandHandlerTests
    {
        private readonly Mock<IGotrueClient<User, Session>> _authMock = new();
        private readonly Mock<Supabase.Client> _supabaseClientMock;
        private readonly Mock<ILogger<ChangePasswordWhenLoggedCommandHandler>> _loggerMock = new();
        private readonly ChangePasswordWhenLoggedCommandHandler _handler;

        public ChangePasswordWhenLoggedCommandHandlerTests()
        {
            _supabaseClientMock = new Mock<Supabase.Client>("http://localhost", "fake-key", null);
            _supabaseClientMock.Object.Auth = _authMock.Object;
            _handler = new ChangePasswordWhenLoggedCommandHandler(_supabaseClientMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ChangesPasswordWhenUserIsLoggedIn()
        {

            var email = "emailteste@gmail.com";
            var userId = Guid.NewGuid();

            var command = new ChangePasswordWhenLoggedCommand(
                accessToken: "fake-access-token",
                refreshToken: "fake-refresh-token",
                newPassword: "NewPassword@123",
                currentPassword: "CurrentTestPassword",
                authenticatedUserId: userId
            );

            _authMock
                .Setup(a => a.SetSession(
                    command.AccessToken,
                    command.RefreshToken,
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(new Session());

            var fakeUser = new User
            {
                Email = email,
                Id = userId.ToString()
            };

            Guid.Parse(fakeUser.Id);

            _authMock.Setup(a => a.CurrentUser).Returns(fakeUser);

            _authMock.Setup(a => a.SignIn(email, command.CurrentPassword))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ReturnsAsync(new User());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
            _authMock.Verify(a => a.SignIn(email, command.CurrentPassword), Times.Once);
            _authMock.Verify(a => a.SetSession(command.AccessToken, command.RefreshToken, It.IsAny<bool>()), Times.Once);
            _authMock.Verify(a => a.Update(It.Is<UserAttributes>(u => u.Password == command.NewPassword)), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowUnauthorized_WhenUserIdDoesNotMatch()
        {
            var userId = Guid.NewGuid();

            var command = new ChangePasswordWhenLoggedCommand(
                "token",
                "refresh",
                "NewPassword@123",
                "CurrentPassword",
                Guid.NewGuid()); 

            var fakeUser = new User
            {
                Email = "email@test.com",
                Id = userId.ToString()
            };

            _authMock.Setup(a => a.CurrentUser).Returns(fakeUser);

            _authMock
                .Setup(a => a.SetSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.SignIn(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Session());

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrow_WhenUpdateReturnsNull()
        {
            var userId = Guid.NewGuid();

            var command = new ChangePasswordWhenLoggedCommand(
                "token",
                "refresh",
                "NewPassword@123",
                "CurrentPassword",
                userId);

            var fakeUser = new User
            {
                Email = "email@test.com",
                Id = userId.ToString()
            };

            _authMock.Setup(a => a.CurrentUser).Returns(fakeUser);

            _authMock
                .Setup(a => a.SetSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.SignIn(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ReturnsAsync((User?)null!);

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrow_WhenSignInFails()
        {
            var userId = Guid.NewGuid();

            var command = new ChangePasswordWhenLoggedCommand(
                "token",
                "refresh",
                "NewPassword@123",
                "CurrentPassword",
                userId);

            var fakeUser = new User
            {
                Email = "email@test.com",
                Id = userId.ToString()
            };

            _authMock.Setup(a => a.CurrentUser).Returns(fakeUser);

            _authMock
                .Setup(a => a.SetSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.SignIn(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new AuthenticationException("error"));

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldThrow_WhenSetSessionFails()
        {
            var command = new ChangePasswordWhenLoggedCommand(
                "token",
                "refresh",
                "NewPassword@123",
                "CurrentPassword",
                Guid.NewGuid());

            _authMock
                .Setup(a => a.SetSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new AuthenticationException("invalid token"));

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
