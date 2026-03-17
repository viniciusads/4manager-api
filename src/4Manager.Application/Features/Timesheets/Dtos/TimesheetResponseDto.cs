namespace _4Tech._4Manager.Application.Features.Timesheets.Dtos
{
    public class TimesheetResponseDto
    {
        public Guid TimesheetId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerName { get; set; } = string.Empty;
        public Guid? ProjectId { get; set; }
        public string? ProjectName { get; set; } = string.Empty;
        public Guid ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; } = string.Empty;
        public Guid? TagId { get; set; }
        public string ActivityColor {  get; set; } = string.Empty;
        public string BlockColor { get; set; } = string.Empty;
        public TimeSpan Duration {  get; set; } 
    }
}
