using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Dtos
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        public PositionEnum Position { get; set; }
        public bool IsActive { get; set; }
        public string UserProfilePicture { get; set; } = string.Empty;
    }
}
