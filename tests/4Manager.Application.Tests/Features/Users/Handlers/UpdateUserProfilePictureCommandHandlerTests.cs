using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Features.Timesheets.Commands;
using _4Tech._4Manager.Application.Features.Timesheets.Handlers;
using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Features.Users.Handlers;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Tests.Features.Users.Handlers
{
    public class UpdateUserProfilePictureCommandHandlerTests
    {
        private readonly IMapper _mapper;

        public UpdateUserProfilePictureCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponseDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task ShouldInsertAndUpdateTheUserProfilePicture()
        {
            var userId = Guid.NewGuid();
            var userProfilePicture = "imagemexemplo.png";

            var fakeUser = new User
            {
                UserId = userId,
                UserProfilePicture = userProfilePicture
            };

            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.UpdateUserProfilePictureAsync(userId, userProfilePicture, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeUser);

            var command = new UpdateUserProfilePictureCommand(userId, userProfilePicture);
            var handler = new UpdateUserProfilePictureCommandHandler(mockRepo.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(userProfilePicture, result.UserProfilePicture);

            mockRepo.Verify(r => r.UpdateUserProfilePictureAsync(userId, userProfilePicture, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UserShouldntPatchUserProfilePictureWhenNotFound_ThrowNotFoundException()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();

            var userId = Guid.NewGuid();
            var userProfilePicture = "imagemexemplo.png";

            var fakeUser = new User
            {
                UserId = userId,
                UserProfilePicture = userProfilePicture
            };

            mockRepo
                .Setup(repo => repo.UpdateUserProfilePictureAsync(userId, userProfilePicture, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException($"UserId com id {userId} não encontrado."));

            var command = new UpdateUserProfilePictureCommand(userId, userProfilePicture);
            var handler = new UpdateUserProfilePictureCommandHandler(mockRepo.Object, _mapper);

            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
