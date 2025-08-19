using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Features.Users.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
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

        [Fact]

        public async Task ReturnEmptyListWhenNoUsers()
        {
            var mockRepository = new Mock<IUserRepository>();
            var handler = new GetUsersQueryHandler(mockRepository.Object);

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(new List<User>());

            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnThrowUserNotFoundException_WhenRepositoryFails()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            var handler = new GetUsersQueryHandler(mockRepository.Object);

            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                 handler.Handle(new GetUsersQuery(), CancellationToken.None));
        }
    }
}

