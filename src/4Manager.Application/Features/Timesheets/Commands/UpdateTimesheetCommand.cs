using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Commands
{
    public class UpdateTimesheetCommand : IRequest<TimesheetResponseDto>
    {
        public Guid TimesheetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthenticatedUserId { get; set; }

        public UpdateTimesheetCommand() { }

        public UpdateTimesheetCommand(DateTime startDate, DateTime endDate, string description, Guid timesheetId, Guid authenticatedUserId)
        {
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            TimesheetId = timesheetId;
            AuthenticatedUserId = authenticatedUserId;
        }
    }
}
