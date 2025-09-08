using _4Tech._4Manager.Application.Features.Users.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Queries
{
    public record GetUsersQuery : IRequest<IEnumerable<UserResponseDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get;  set; } = 10;
    }
}
