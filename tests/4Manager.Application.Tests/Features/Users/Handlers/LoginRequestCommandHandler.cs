using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using _4Manager.Application.Features.Users.Commands;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Handlers;
using _4Manager.Domain.Entities;
using _4Manager.Application.Interfaces;

namespace _4Manager.Application.Tests.Features.Users.Handlers
{
    public class LoginRequestCommandHandlerTests
    {
        [Fact]
        public async Task ValidCredentials_ReturnsLoginResponseDto()
        {
        
            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var email = "pedro@gmail.com";
            var password = "123456";
            var accessToken = "access-token";
            var refreshToken = "refresh-token";

            authServiceMock
                .Setup(x => x.LoginAsync(email, password))
                .ReturnsAsync((accessToken, refreshToken));

            userRepositoryMock
                .Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync(new User { Email = email });

            var handler = new LoginRequestCommandHandler(authServiceMock.Object, userRepositoryMock.Object);
            var command = new LoginRequestCommand(email, password);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            Assert.Equal(accessToken, result.AccessToken);
            Assert.Equal(refreshToken, result.RefreshToken);
        }

        [Fact]
        public async Task UserNotFound_ThrowsException()
        {

            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var email = "pedro@gmail.com";
            var password = "123456";

            authServiceMock
                .Setup(x => x.LoginAsync(email, password))
                .ReturnsAsync(("token", "refreshtoken"));

            userRepositoryMock
                .Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync((User)null!);

            var handler = new LoginRequestCommandHandler(authServiceMock.Object, userRepositoryMock.Object);
            var command = new LoginRequestCommand (email, password );

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
