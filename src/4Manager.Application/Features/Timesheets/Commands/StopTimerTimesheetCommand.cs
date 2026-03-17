using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Commands
{
    public class StopTimerTimesheetCommand : IRequest<TimesheetResponseDto>
    {
        public Guid TimesheetId { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public Guid AuthenticatedUserId { get; set; }
        public Guid? ProjectId { get; set; }

        public StopTimerTimesheetCommand() { }

        public StopTimerTimesheetCommand(DateTime endDate, Guid timesheetId, string? description, Guid authenticatedUserId, Guid? projectId)
        {
            EndDate = endDate;
            TimesheetId = timesheetId;
            Description = description;
            AuthenticatedUserId = authenticatedUserId;
            ProjectId = projectId;
        }
    }
}
