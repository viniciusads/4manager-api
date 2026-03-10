using Xunit;
using Moq;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Features.Users.Queries;
using AutoMapper;
using _4Tech._4Manager.Application.Features.Users.Dtos;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
{
    public class GetUserByIdQueryHandlerTests
    {
        [Fact]
        public async Task UserExists_ReturnsUserResponseDto()
        {
            var mockRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var user = new User
            {
                UserId = userId,
                Name = "User",
                Email = "user@gmail.com"
            };

            var userResponseDto = new UserResponseDto
            {
                UserId = userId,
                Name = "User",
                Email = "user@gmail.com"
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync(user);

            mockMapper.Setup(m => m.Map<UserResponseDto>(user))
                .Returns(userResponseDto);

            var handler = new GetUserByIdQueryHandler(mockRepository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("User", result.Name);
        }


        [Fact]
        public async Task UserDoesNotExist_ThrowsUserNotFoundException()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            var cancellationToken = CancellationToken.None;
            var userId = Guid.NewGuid();

            userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync((User?)null);

            var handler = new GetUserByIdQueryHandler(userRepositoryMock.Object, mockMapper.Object);
            var query = new GetUserByIdQuery(userId);

            await Assert.ThrowsAsync<_4Tech._4Manager.Application.Common.Exceptions.UserNotFoundException>(
                () => handler.Handle(query, CancellationToken.None)
            );
        }
    }
}
