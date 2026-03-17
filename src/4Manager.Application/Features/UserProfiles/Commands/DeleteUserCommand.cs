using _4Tech._4Manager.Application.Features.UserProfiles.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Commands
{
    public class DeleteUserCommand : IRequest<UserResponseDto>
    {
        public Guid UserId { get; set; }
        public Guid AuthenticatedUserId { get; set; }
        public DeleteUserCommand(Guid userId, Guid authenticatedUserId)
        {
            UserId = userId;
            AuthenticatedUserId = authenticatedUserId;
        }
    }

}
