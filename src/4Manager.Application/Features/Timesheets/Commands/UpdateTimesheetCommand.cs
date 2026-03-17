using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Commands
{
    public class UpdateTimesheetCommand : IRequest<TimesheetResponseDto>
    {
        public Guid TimesheetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? CustomerId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ActivityTypeId { get; set; }
        public Guid AuthenticatedUserId { get; set; }

        public UpdateTimesheetCommand() { }

        public UpdateTimesheetCommand(Guid timesheetId, DateTime startDate, DateTime endDate, string description, Guid authenticatedUserId, Guid activityTypeId, Guid? projectId, Guid? customerId)
        {
            TimesheetId = timesheetId;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            AuthenticatedUserId = authenticatedUserId;
            ActivityTypeId = activityTypeId;
            ProjectId = projectId;
            CustomerId = customerId;
        }
    }
}
