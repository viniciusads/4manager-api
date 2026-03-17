namespace _4Tech._4Manager.Application.Features.TimesheetReports.Dtos
{
    public class ActivityTypePercentageDto
    {
        public Guid ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; } = string.Empty;
        public string Percentage { get; set; } = string.Empty;
        public string ActivityColor { get; set; } = string.Empty;
    }
}
