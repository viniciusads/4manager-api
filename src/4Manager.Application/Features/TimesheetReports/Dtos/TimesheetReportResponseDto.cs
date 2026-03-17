namespace _4Tech._4Manager.Application.Features.TimesheetReports.Dtos
{
    public class TimesheetReportResponseDto
    {
        public Guid TimesheetId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ActivityTypeId { get; set; }
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? CustomerName { get; set; }
        public string ActivityTypeName { get; set; } = string.Empty;
        public string BlockColor { get; set; } = string.Empty;
        public string ActivityColor {  get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public string? Hours { get; set; }
        public string? Time { get; set; }

    }
}
