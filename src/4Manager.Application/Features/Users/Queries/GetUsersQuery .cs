using _4Manager.Application.Features.Users.Dtos;
using MediatR;

namespace _4Manager.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserResponseDto>>
    {
    }
}
