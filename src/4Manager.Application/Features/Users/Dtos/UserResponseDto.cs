using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Application.Features.Users.Dtos
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public PositionEnum Position { get; set; }
        public bool IsActive { get; set; }
        public string UserProfilePicture { get; set; } = string.Empty;
    }
}
