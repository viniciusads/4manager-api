namespace _4Tech._4Manager.Application.Features.TimesheetReports.Dtos
{
    public class ProjectPercentageDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Percentage { get; set; } = string.Empty;
        public string BlockColor {  get; set; } = string.Empty;
    }
}
