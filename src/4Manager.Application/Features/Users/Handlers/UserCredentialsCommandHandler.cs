using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using _4Manager.Application.Features.Users.Commands;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Domain.Interfaces;

namespace _4Manager.Application.Features.Users.Handlers
{
    public class UserCredentialsCommandHandler : IRequestHandler<UserCredentialsCommand, UserDto>
    {
        private readonly IUserRepository _repository;

        public UserCredentialsCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(UserCredentialsCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmailAsync(request.Email);

            if (user == null)
                throw new Exception("Usuário não encontrado.");

            if (user.PasswordHash != request.Password) 
                throw new Exception("Senha inválida.");

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}

