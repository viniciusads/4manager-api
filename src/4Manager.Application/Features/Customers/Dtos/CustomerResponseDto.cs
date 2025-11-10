namespace _4Tech._4Manager.Application.Features.Customers.Dtos
{
    public class CustomerResponseDto
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}