using MediatR;
using _4Manager.Application.Features.Users.Dtos;

namespace _4Manager.Application.Features.Users.Commands
{
    public class ResetPasswordCommand : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ResetPasswordCommand(string email, string newPassword, string confirmPassword)
        {
            Email = email;
            NewPassword = newPassword;
            ConfirmPassword = confirmPassword;
        }
    }
}
