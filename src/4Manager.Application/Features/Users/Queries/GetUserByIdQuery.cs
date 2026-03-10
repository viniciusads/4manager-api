using _4Tech._4Manager.Application.Features.Users.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserResponseDto?>
    {
        public Guid UserId { get; set; } = UserId;
    }
}
