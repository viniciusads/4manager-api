using _4Tech._4Manager.Application.Features.Timesheets.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Timesheets.Queries
{
    public class GetTimesheetsByPeriodQuery : IRequest<IEnumerable<TimesheetResponseDto>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid AuthenticatedUserId { get; set; }
        public GetTimesheetsByPeriodQuery(DateTime startDate, DateTime endDate, Guid authenticatedUserId)
        {
            StartDate = startDate;
            EndDate = endDate;
            AuthenticatedUserId = authenticatedUserId;
        }
    }
}
