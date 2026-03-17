using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using configurationFuck = Microsoft.Extensions.Configuration.IConfiguration;
using configurationSection = Microsoft.Extensions.Configuration.IConfigurationSection;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class ResetPasswordCommandHandlerTests
    {
        [Fact]
        public async Task Should_Call_ResetPasswordForEmail_And_Return_Unit()
        {
            var email = "maria@gmail.com";
            var expectedUrl = "http://localhost:4200/auth/adicionar-nova-senha";

            var mockAuthService = new Mock<IAuthService>();
            var mockConfiguration = new Mock<configurationFuck>();
            var mockSection = new Mock<configurationSection>();
            var mockLogger = new Mock<ILogger<ResetPasswordCommandHandler>>();


            mockSection.Setup(s => s["ResetPasswordUrl"]).Returns(expectedUrl);

            mockConfiguration
                .Setup(c => c.GetSection("FrontendSettings"))
                .Returns(mockSection.Object);

            mockAuthService
                .Setup(x => x.ResetPasswordForEmail(email, expectedUrl))
                .Returns(Task.CompletedTask);

            var command = new ResetPasswordCommand { 
                Email = email
            };

            var handler = new ResetPasswordCommandHandler(mockAuthService.Object, mockConfiguration.Object, mockLogger.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
            mockAuthService.Verify(x => x.ResetPasswordForEmail(email, expectedUrl), Times.Once);
        }
    }
}
