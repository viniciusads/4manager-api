using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Queries
{
    public record ExportTimesheetReportQuery(
        DateTime StartDate,
        DateTime EndDate,
        Guid AuthenticatedUserId,
        string Format // "txt", "csv", "pdf"
    ) : IRequest<ExportFileResultDto>;
}