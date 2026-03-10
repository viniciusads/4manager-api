using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleEnum Role { get; set; }
        public bool IsActive { get; set; }
        public PositionEnum Position { get; set; }
        public AcessLevelEnum AcessLevel { get; set; }
        public string? UserProfilePicture { get; set; }
        public List<TeamCollaborator>? TeamCollaborators { get; set; }
    }
}
