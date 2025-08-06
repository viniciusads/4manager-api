using MediatR;
using _4Manager.Application.Features.Users.Dtos;

namespace _4Manager.Application.Features.Users.Commands
{
    public class LoginRequestCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginRequestCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
