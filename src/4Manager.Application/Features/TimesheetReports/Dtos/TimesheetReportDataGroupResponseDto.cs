namespace _4Tech._4Manager.Application.Features.TimesheetReports.Dtos
{
    public class TimesheetReportDataGroupResponseDto
    {
        public string TotalHours { get; set; } = string.Empty;
        public List<TimesheetReportResponseDto> Timesheets { get; set; } = new List<TimesheetReportResponseDto>();
        public List<ActivityTypePercentageDto> ActivityPercentages { get; set; } = new List<ActivityTypePercentageDto>();
        public List<ProjectPercentageDto> ProjectPercentages { get; set; } = new List<ProjectPercentageDto> { };
    }
}
