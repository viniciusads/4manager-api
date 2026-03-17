using _4Tech._4Manager.Application.Features.Customers.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<CustomerResponseDto>
    {
        public Guid CustomerId { get; set; }

        public DeleteCustomerCommand(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}