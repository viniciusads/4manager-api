namespace _4Tech._4Manager.Domain.Entities
{
    public class Team
    {
        public Guid TeamId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ManagerId { get; set; }
        public Project? Project { get; set; }
        public UserProfile Manager { get; set; } = null!;
        public List<TeamCollaborator> Collaborators { get; set; } = new List<TeamCollaborator>();
    }
}