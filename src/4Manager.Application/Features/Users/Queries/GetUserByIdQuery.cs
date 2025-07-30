using _4Manager.Application.Features.Users.Dtos;
using MediatR;
using System;

namespace _4Manager.Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public Guid UserId { get; set; }

        public GetUserByIdQuery(Guid UserId)
        {
            UserId = UserId;
        }
    }
}
