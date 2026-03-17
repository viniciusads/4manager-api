using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Domain.Entities
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public Guid? CustomerId { get; set; }
        public Guid? CustomerManagerId { get; set; }
        public ProjectStatusEnum StatusProject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string TitleColor { get; set; } = string.Empty;
        public TimeSpan StatusTime { get; set; }
        public bool Favorite { get; set; }
        public bool Archived { get; set; }
        public Team Team { get; set; } = null!;
        public Customer? Customer { get; set; }
        public UserProfile? CustomerManager { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}