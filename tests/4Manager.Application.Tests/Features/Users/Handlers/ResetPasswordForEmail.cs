using Xunit;
using Moq;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using MediatR;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
{
    public class ResetPasswordCommandHandlerTests
    {
        [Fact]
        public async Task Should_Call_ResetPasswordForEmail_And_Return_Unit()
        {
            var email = "maria@gmail.com";
            var url = "http://localhost:4200/auth/adicionar-nova-senha";

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService
                .Setup(x => x.ResetPasswordForEmail(email, url))
                .Returns(Task.CompletedTask);

            var command = new ResetPasswordCommand { Email = email };
            var handler = new ResetPasswordCommandHandler(mockAuthService.Object);

 
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);
            mockAuthService.Verify(x => x.ResetPasswordForEmail(email, url), Times.Once);
        }
    }
}
