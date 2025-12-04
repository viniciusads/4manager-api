using _4Tech._4Manager.Application.Common.Pagination;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Queries
{
    public record GetUsersQuery : IRequest<IEnumerable<UserResponseDto>>, IPagedQuery
    {
        public int PageNumber { get; set; } = PaginationDefaults.DefaultPageNumber;
        public int PageSize { get; set; } = PaginationDefaults.DefaultPageSize;
    }
}
