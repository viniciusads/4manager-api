using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using _4Tech._4Manager.Application.Features.UserProfiles.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Tests.Fixtures;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;

namespace _4Tech._4Manager.Application.Tests.Features.UserProfiles.Handlers
{
    public class UpdateUserProfilePictureCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly UserProfileTestFixture _fixture;
        private readonly Mock<ILogger<UpdateUserProfilePictureCommandHandler>> _loggerMock = new();

        public UpdateUserProfilePictureCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfile, UserResponseDto>();
            });
            _mapper = config.CreateMapper();

            _fixture = new UserProfileTestFixture();
        }

        [Fact]
        public async Task ShouldInsertAndUpdateTheUserProfilePicture()
        {
            var userProfilePicture = "imagemexemplo.png";

            var fakeUserProfile = _fixture.GenerateUserProfile(1)[0];
            fakeUserProfile.UserProfilePicture = userProfilePicture;

            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.UpdateUserProfilePictureAsync(fakeUserProfile.UserId, userProfilePicture, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeUserProfile);

            var command = new UpdateUserProfilePictureCommand(fakeUserProfile.UserId, userProfilePicture);
            var handler = new UpdateUserProfilePictureCommandHandler(mockRepo.Object, _mapper, _loggerMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(fakeUserProfile.UserId, result.UserId);
            Assert.Equal(userProfilePicture, result.UserProfilePicture);

            mockRepo.Verify(r => r.UpdateUserProfilePictureAsync(fakeUserProfile.UserId, userProfilePicture, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UserShouldntPatchUserProfilePictureWhenNotFound_ThrowNotFoundException()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var fakeUserProfile = _fixture.GenerateUserProfile(1)[0];

            var userProfilePicture = "imagemexemplo.png";

            mockRepo
                .Setup(repo => repo.UpdateUserProfilePictureAsync(fakeUserProfile.UserId, userProfilePicture, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserException($"Usuário com id {fakeUserProfile.UserId} não encontrado."));

            var command = new UpdateUserProfilePictureCommand(fakeUserProfile.UserId, userProfilePicture);
            var handler = new UpdateUserProfilePictureCommandHandler(mockRepo.Object, _mapper, _loggerMock.Object);

            var exceptionMessage = $"Usuário com id {fakeUserProfile.UserId} não encontrado.";
            var exception = await Assert.ThrowsAsync<UserException>(
                () => handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal(exceptionMessage, exception.Message);
            mockRepo.Verify(a => a.GetByIdAsync(fakeUserProfile.UserId, It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
