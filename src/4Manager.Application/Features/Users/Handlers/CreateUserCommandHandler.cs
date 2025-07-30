using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using _4Manager.Application.Features.Users.Commands;
using _4Manager.Domain.Entities;
using _4Manager.Domain.Interfaces;
using _4Manager.Application.Features.Users.Dtos;

namespace _4Manager.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _repository;

        public CreateUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.Password, 
                Active = true
            };

            await _repository.AddAsync(user);

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
