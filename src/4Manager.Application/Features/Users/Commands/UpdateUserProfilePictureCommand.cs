using _4Tech._4Manager.Application.Features.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Tech._4Manager.Application.Features.Users.Commands
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
