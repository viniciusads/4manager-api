namespace _4Tech._4Manager.Domain.Entities
{
    public class Timesheet
    {
        public Guid TimesheetId { get; set; }
        public DateTime Date {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }
        public Guid ActivityTypeId { get; set; }
        public ActivityType? ActivityType {  get; set; }
        public Guid? TagId { get; set; }
    }
}
