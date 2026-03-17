using _4Tech._4Manager.Application.Features.Customers.Dtos;
using _4Tech._4Manager.Application.Features.Customers.Queries;
using _4Tech._4Manager.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Handlers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerResponseDto>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public GetCustomersQueryHandler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerResponseDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _repository.GetAllAsync(cancellationToken);

            var pagedCustomers = customers
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return pagedCustomers.Select(c => _mapper.Map<CustomerResponseDto>(c));
        }
    }
}