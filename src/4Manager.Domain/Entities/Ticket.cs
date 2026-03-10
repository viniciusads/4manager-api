using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Domain.Entities
{
    public class Ticket
    {
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public Guid TicketId { get; set; }
        public int TicketNumber { get; set; }
        public Guid InternalCall {  get; set; }
        public string Applicant { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public string TicketResponsible { get; set; } = string.Empty;
        public TicketStatusEnum TicketStatus { get; set; }
        public string Description { get; set; } = string.Empty;
        public TicketPriorityEnum Priority { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string AffectedSystem { get; set; } = string.Empty;
        public string ResponsibleArea { get; set; } = string.Empty;
        public List<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
        public TicketDetails? TicketDetails { get; set; }
    }
}
