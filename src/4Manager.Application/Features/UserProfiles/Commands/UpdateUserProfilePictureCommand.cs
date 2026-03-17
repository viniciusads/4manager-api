using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Commands
{
    public class UpdateUserProfilePictureCommand : IRequest<UserResponseDto>
    {
        public Guid UserId { get; set; }
        public string UserProfilePicture { get; set; } = string.Empty;

        public UpdateUserProfilePictureCommand() { }

        public UpdateUserProfilePictureCommand(Guid userId, string profilePicture)
        {
            UserId = userId;
            UserProfilePicture = profilePicture;
        }
    }
}
