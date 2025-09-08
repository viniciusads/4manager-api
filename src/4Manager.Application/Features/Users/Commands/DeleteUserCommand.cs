using _4Tech._4Manager.Application.Features.Users.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<UserResponseDto>
    {
        public Guid UserId { get; set; }
        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }

}
