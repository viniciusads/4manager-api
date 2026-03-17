using _4Tech._4Manager.Application.Features.TimesheetReports.Dtos;

namespace _4Tech._4Manager.Application.Features.TimesheetReports.Services
{
    public interface ITimesheetReportCalculator
    {
        TimesheetReportDataGroupResponseDto Calculate(List<TimesheetReportResponseDto> timesheets);
    }
}
