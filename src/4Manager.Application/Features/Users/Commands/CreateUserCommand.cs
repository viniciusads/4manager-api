using MediatR;
using _4Manager.Application.Features.Users.Dtos;

namespace _4Manager.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }

        public CreateUserCommand(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
