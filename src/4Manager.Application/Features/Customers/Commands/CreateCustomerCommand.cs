using _4Tech._4Manager.Application.Features.Customers.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerResponseDto>
    {
        public string Name { get; set; } = string.Empty;

        public CreateCustomerCommand() {}
        
        public CreateCustomerCommand(string name)
        {
            Name = name;
        }
    }
}