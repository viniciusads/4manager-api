using _4Tech._4Manager.Application.Features.Customers.Dtos;
using _4Tech._4Manager.Application.Features.Customers.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Handlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponseDto?>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerResponseDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken);

            return _mapper.Map<CustomerResponseDto>(customer);
        }
    }
}