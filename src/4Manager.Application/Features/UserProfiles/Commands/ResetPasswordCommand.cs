using MediatR;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Commands
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public required string Email { get; set; } 

    }
}
