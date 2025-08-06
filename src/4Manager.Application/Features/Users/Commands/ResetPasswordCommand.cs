using MediatR;
using _4Manager.Application.Features.Users.Dtos;

namespace _4Manager.Application.Features.Users.Commands
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; } = null!;

    }
}
