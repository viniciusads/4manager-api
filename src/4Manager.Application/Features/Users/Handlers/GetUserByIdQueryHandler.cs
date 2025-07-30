using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Queries;
using _4Manager.Domain.Interfaces;

namespace _4Manager.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                return null;

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
