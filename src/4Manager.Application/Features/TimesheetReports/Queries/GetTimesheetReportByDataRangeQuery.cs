using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Queries
{
    public class GetTimesheetReportByDataRangeQuery : IRequest<TimesheetReportDataGroupResponseDto>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid AuthenticatedUserId { get; set; }
        public GetTimesheetReportByDataRangeQuery(DateTime startDate, DateTime endDate, Guid authenticatedUserId)
        {
            StartDate = startDate;
            EndDate = endDate;
            AuthenticatedUserId = authenticatedUserId;
        }
    }
}

