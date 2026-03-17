namespace _4Tech._4Manager.Domain.Entities
{
    public class Schedule
    {
        public Guid ScheduleId { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public List<ScheduleTask> Tasks { get; set; } = new List<ScheduleTask>();
    }
}