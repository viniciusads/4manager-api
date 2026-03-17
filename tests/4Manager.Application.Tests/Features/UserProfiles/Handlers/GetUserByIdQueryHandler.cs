using Moq;
using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using _4Tech._4Manager.Application.Features.UserProfiles.Queries;
using AutoMapper;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class GetUserByIdQueryHandlerTests
    {

        private readonly Mock<ILogger<GetUserByIdQueryHandler>> _logger = new();

        [Fact]

        public async Task UserExists_ReturnsUserResponseDto()
        {
            var mockRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
 
            var user = new UserProfile
            {
                UserId = userId,
                Name = "UserProfile"
            };
 
            var userResponseDto = new UserResponseDto
            {
                UserId = userId,
                Name = "UserProfile"
            };
 
            mockRepository.Setup(repo => repo.GetByIdAsync(userId, cancellationToken))
                .ReturnsAsync(user);
 
            mockMapper.Setup(m => m.Map<UserResponseDto>(user))
                .Returns(userResponseDto);

            var handler = new GetUserByIdQueryHandler(mockRepository.Object, mockMapper.Object, _logger.Object);
 
            var result = await handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);
 
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("UserProfile", result.Name);
        }
 
 
        [Fact]
        public async Task UserDoesNotExist_ThrowsUserNotFoundException()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var userId = Guid.NewGuid();
 
            userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserProfile?)null);
 
            var handler = new GetUserByIdQueryHandler(userRepositoryMock.Object, mockMapper.Object, _logger.Object);
            var query = new GetUserByIdQuery(userId);

            var exceptionMessage = $"Usuário não encontrado.";
            var exception = await Assert.ThrowsAsync<UserException>(
                () => handler.Handle(query, CancellationToken.None)
            );

            Assert.Equal(exceptionMessage, exception.Message);
            userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}