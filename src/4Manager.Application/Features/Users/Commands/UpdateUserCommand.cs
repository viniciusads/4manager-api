using _4Tech._4Manager.Application.Features.Users.Dtos;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserResponseDto>
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UpdateUserCommand(string name, string email, string role, string password)
        {
            Name = name;
            Email = email;
            Role = role;
            Password = password;
        }
    }
}
