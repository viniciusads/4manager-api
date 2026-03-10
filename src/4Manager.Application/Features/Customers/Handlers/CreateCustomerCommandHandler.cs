using _4Tech._4Manager.Domain.Entities;
using _4Tech._4Manager.Application.Features.Customers.Commands;
using _4Tech._4Manager.Application.Features.Customers.Dtos;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerResponseDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerResponseDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddCustomerAsync(customer, cancellationToken);

            return _mapper.Map<CustomerResponseDto>(customer);
        }
    }
}