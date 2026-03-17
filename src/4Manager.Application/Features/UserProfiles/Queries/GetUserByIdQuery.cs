using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserResponseDto?>
    {
        public Guid UserId { get; set; } = UserId;
    }
}
