using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Commands
{
    public class StartTimerTimesheetCommand : IRequest<TimesheetResponseDto>
    {
        public DateTime StartDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthenticatedUserId { get; set; }

        public StartTimerTimesheetCommand () { }

        public StartTimerTimesheetCommand (DateTime startDate, string description, Guid authenticatedUserId)
        {
            StartDate = startDate;
            Description = description;
            AuthenticatedUserId = authenticatedUserId;
        }
    }
}
