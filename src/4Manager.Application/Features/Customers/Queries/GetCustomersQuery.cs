using _4Tech._4Manager.Application.Common.Pagination;
using _4Tech._4Manager.Application.Features.Customers.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Customers.Queries
{
    public record GetCustomersQuery : IRequest<IEnumerable<CustomerResponseDto>>, IPagedQuery
    {
        public int PageNumber { get; set; } = PaginationDefaults.DefaultPageNumber;
        public int PageSize { get; set; } = PaginationDefaults.DefaultPageSize;
    }
}