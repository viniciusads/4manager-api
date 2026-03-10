using _4Tech._4Manager.Application.Features.Users.Dtos;
using _4Tech._4Manager.Domain.Enums;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserResponseDto>
    {
        public Guid UserId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public PositionEnum? Position { get; set; }
        public string AccessToken { get; set;}
        public string RefreshToken { get; set; }

        public UpdateUserCommand() { }

        public UpdateUserCommand(Guid userId, string name, string email, PositionEnum? position, string accessToken, string refreshToken)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Position = position;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

    }
}