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
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<IGotrueClient<User, Session>> _authMock = new();
        private readonly Mock<ILogger<ChangePasswordCommandHandler>> _loggerMock = new();
        private readonly Mock<Supabase.Client> _supabaseClientMock;
        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            _supabaseClientMock = new Mock<Supabase.Client>("http://localhost", "fake-key", null);
            _supabaseClientMock.Object.Auth = _authMock.Object;
            _handler = new ChangePasswordCommandHandler(_supabaseClientMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_UpdatesPassword_When_DataIsValid()
        {
            var command = new ChangePasswordCommand(
                accessToken: "fake-access-token",
                refreshToken: "fake-refresh-token",
                newPassword: "NewPassword@123"
            );

            _authMock
                .Setup(a => a.SetSession(
                    command.AccessToken,
                    command.RefreshToken,
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ReturnsAsync(new User());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);

            _authMock.Verify(a => a.SetSession(command.AccessToken, command.RefreshToken, It.IsAny<bool>()), Times.Once);
            _authMock.Verify(a => a.Update(It.Is<UserAttributes>(u => u.Password == command.NewPassword)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowAuthenticationException_When_SupabaseReturnsSamePasswordError()
        {
            var command = new ChangePasswordCommand("token", "refresh", "pass");

            _authMock
                .Setup(a => a.SetSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            var exception = new Supabase.Gotrue.Exceptions.GotrueException("same_password");
            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ThrowsAsync(exception);

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_When_UpdateReturnsNull()
        {
            var command = new ChangePasswordCommand(
                "token",
                "refresh",
                "NewPassword@123");

            _authMock
                .Setup(a => a.SetSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ReturnsAsync((User?)null!);

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_When_UpdateThrowsGenericException()
        {
            var command = new ChangePasswordCommand(
                "token",
                "refresh",
                "NewPassword@123");

            _authMock
                .Setup(a => a.SetSession(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(new Session());

            _authMock
                .Setup(a => a.Update(It.IsAny<UserAttributes>()))
                .ThrowsAsync(new AuthenticationException("generic error"));

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}