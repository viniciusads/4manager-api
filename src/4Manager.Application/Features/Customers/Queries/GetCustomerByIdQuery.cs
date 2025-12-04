using _4Tech._4Manager.Application.Features.Customers.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Queries
{
    public record GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerResponseDto?>
    {
        public Guid CustomerId { get; set; } = CustomerId;
    }
}