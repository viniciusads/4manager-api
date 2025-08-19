using _4Tech._4Manager.Application.Features.Users.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Commands
{
    public class SignUpUserCommand : IRequest<UserResponseDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public SignUpUserCommand(string name, string email, string password, string confirmPassword)
        {
            Name = name;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
