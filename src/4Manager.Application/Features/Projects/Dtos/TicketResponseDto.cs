using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Application.Features.Projects.Dtos
{
    public class TicketResponseDto
    {
        public Guid ProjectId { get; set; }
        public Guid TicketId { get; set; }
        public int TicketNumber { get; set; }
        public Guid InternalCall { get; set; }
        public string Applicant { get; set; } = null!;
        public string Sector { get; set; } = null!;
        public string TicketResponsible { get; set; } = null!;
        public TicketStatusEnum TicketStatus { get; set; }
        public string Description { get; set; } = null!;
        public TicketPriorityEnum Priority { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string AffectedSystem { get; set; } = null!;
        public string ResponsibleArea { get; set; } = null!;
        public List<TicketAttachmentResposeDto> Attachments { get; set; } = new List<TicketAttachmentResposeDto>();
    }
}
