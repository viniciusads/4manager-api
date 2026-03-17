namespace _4Tech._4Manager.Domain.Entities
{
    public class ActivityType
    {
        public Guid ActivityTypeId { get; set; }
        public string ActivityTypeName { get; set; } = string.Empty;
        public string ActivityTypeColor { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
