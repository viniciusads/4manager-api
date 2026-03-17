namespace _4Tech._4Manager.Domain.Entities
{
    public class Customer {
        public Guid? CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Project>? Projects { get; set; }
    }
}