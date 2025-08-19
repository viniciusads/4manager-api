using MediatR;
using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Application.Interfaces;

namespace _4Tech._4Manager.Application.Features.Users.Commands
{
    public class LoginRequestCommand : IRequest<AuthResult>
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
