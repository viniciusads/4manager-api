using _4Tech._4Manager.Application.Features.Customers.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<CustomerResponseDto>
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public UpdateCustomerCommand(Guid customerId, string name, bool isActive)
        {
            CustomerId = customerId;
            Name = name;
            IsActive = isActive;
        }
    }
}