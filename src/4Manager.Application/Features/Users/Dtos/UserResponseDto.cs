

using _4Manager.Domain.Enums;

namespace _4Manager.Application.Features.Users.Dtos
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool isActive { get; set; } 
    }
}
