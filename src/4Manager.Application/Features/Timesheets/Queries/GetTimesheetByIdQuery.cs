using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Queries
{
    public class GetTimesheetByIdQuery : IRequest<TimesheetResponseDto>
    {
        public Guid TimesheetId { get; set; }
        public Guid AuthenticatedUserId { get; set; }

        public GetTimesheetByIdQuery(Guid timesheetId, Guid authenticatedUserId)
        {
            TimesheetId = timesheetId;
            AuthenticatedUserId = authenticatedUserId;
        }
    }
}
