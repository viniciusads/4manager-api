using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using _4Tech._4Manager.Application.Features.UserProfiles.Queries;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class GetUsersQueryHandlerTests
    {

        private readonly UserProfileTestFixture _fixture;
        private readonly Mock<ILogger<GetUsersQueryHandler>> _loggerMock = new();

        public GetUsersQueryHandlerTests()
        {
            _fixture = new UserProfileTestFixture();
        }

        [Fact]
        public async Task ReturnListOfUserResponseDto()
        {
            var mockRepository = new Mock<IUserRepository>();
            var fakeUserProfiles = _fixture.GenerateUserProfile(2).ToList();

            mockRepository.Setup(repo => repo.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(fakeUserProfiles);

            var handler = new GetUsersQueryHandler(mockRepository.Object, _loggerMock.Object);

            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]

        public async Task ReturnEmptyListWhenNoUsers()
        {
            var mockRepository = new Mock<IUserRepository>();
            var handler = new GetUsersQueryHandler(mockRepository.Object, _loggerMock.Object);

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(new List<UserProfile>());

            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnThrowUserNotFoundException_WhenRepositoryFails()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserException());

            var handler = new GetUsersQueryHandler(mockRepository.Object, _loggerMock.Object);

            var exception = await Assert.ThrowsAsync<UserException>(() =>
                handler.Handle(new GetUsersQuery(), CancellationToken.None));

            Assert.Equal("Usuário não encontrado.", exception.Message);
        }
    }
}

