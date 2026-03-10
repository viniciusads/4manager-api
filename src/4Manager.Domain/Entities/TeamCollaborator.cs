namespace _4Tech._4Manager.Domain.Entities
{
    public class TeamCollaborator
    {
        public Guid TeamCollaboratorId { get; set; }
        public Guid TeamId { get; set; }
        public Guid CollaboratorId { get; set; }
        public Team Team { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
