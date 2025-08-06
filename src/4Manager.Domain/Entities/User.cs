using _4Manager.Domain.Enums;

namespace _4Manager.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public RoleEnum Role { get; set; }
        public bool isActive { get; set; }
    }
}
