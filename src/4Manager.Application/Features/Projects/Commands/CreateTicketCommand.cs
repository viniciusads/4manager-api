using _4Tech._4Manager.Domain.Enums;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Projects.Commands
{
    public class CreateTicketCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
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
    }
}
