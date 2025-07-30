using MediatR;
using _4Manager.Application.Features.Users.Dtos;

namespace _4Manager.Application.Features.Users.Commands
{
    public class UserCredentialsCommand : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserCredentialsCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
