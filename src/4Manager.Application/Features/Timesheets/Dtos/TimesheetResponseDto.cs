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
        public Guid? ClientId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? ActivityId { get; set; }
        public Guid? TagId { get; set; }
        public string BlockColor { get; set; } = string.Empty;
    }
}
