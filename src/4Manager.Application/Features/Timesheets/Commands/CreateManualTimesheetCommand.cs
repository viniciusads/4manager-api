using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Commands
{
    public class CreateManualTimesheetCommand : IRequest<TimesheetResponseDto>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthenticatedUserId { get; set; }

        public CreateManualTimesheetCommand() { }

        public CreateManualTimesheetCommand(DateTime startDate, DateTime endDate, string description, Guid authenticatedUserId)
        {
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            AuthenticatedUserId = authenticatedUserId;
        }
    }
}
