using _4Tech._4Manager.Domain.Enums;

namespace _4Tech._4Manager.Domain.Entities
{
    public class ScheduleTask
    {
        public Guid ScheduleTaskId { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid? ParentTaskId { get; set; }
        public ScheduleTaskTypeEnum TaskType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public Guid? ResponsibleId { get; set; }
        public string? Stakeholder { get; set; }
        public decimal CompletionPercentage { get; set; }
        public int OrderIndex { get; set; }
        public Schedule? Schedule { get; set; }
        public UserProfile? Responsible { get; set; }
        public ScheduleTask? ParentTask { get; set; }
        public List<ScheduleTask> SubTasks { get; set; } = new List<ScheduleTask>();
    }
}