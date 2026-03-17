using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Domain.Entities
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public RoleEnum Function { get; set; }
        public bool IsActive { get; set; }
        public PositionEnum Position { get; set; }
        public AcessLevelEnum AcessLevel { get; set; }
        public string? UserProfilePicture { get; set; }
        public List<TeamCollaborator>? TeamCollaborators { get; set; }
    }
}
