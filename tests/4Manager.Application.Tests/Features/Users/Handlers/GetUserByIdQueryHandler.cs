using Xunit;
using Moq;
using System;
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
    public class GetUserByIdQueryHandlerTests
    {
        [Fact]
        public async Task UserExists_ReturnsUserResponseDto()
        {
            var mockRepository = new Mock<IUserRepository>();
            var userId = Guid.NewGuid();

            var user = new User
            {
                UserId = userId,
                Name = "User",
                Email = "user@gmail.com"
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            var handler = new GetUserByIdQueryHandler(mockRepository.Object);

            var result = await handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("User", result.Name);
        }


        [Fact]
        public async Task UserDoesNotExist_ReturnsNull()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((User?)null);

            var handler = new GetUserByIdQueryHandler(userRepositoryMock.Object);
            var query = new GetUserByIdQuery(Guid.NewGuid());

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
