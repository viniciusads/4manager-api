using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Handlers;
using _4Manager.Application.Features.Users.Queries;
using _4Manager.Domain.Entities;
using _4Manager.Domain.Enums;
using _4Manager.Application.Interfaces;

namespace _4Manager.Application.Tests.Features.Users.Handlers
{
    public class GetUsersQueryHandlerTests
    {

        [Fact]
        public async Task ReturnListOfUserResponseDto()
        {
            var mockRepository = new Mock<IUserRepository>();
            var fakeUsers = new List<User>
    {
        new User
        {
            UserId = Guid.NewGuid(),
            Name = "User One",
            Email = "user1@gmail.com"
        },
        new User
        {
            UserId = Guid.NewGuid(),
            Name = "User Two",
            Email = "user2@gmail.com"
        }
    };

            mockRepository.Setup(repo => repo.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(fakeUsers);

            var handler = new GetUsersQueryHandler(mockRepository.Object);

            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Name == "User One");
        }

    }
}
