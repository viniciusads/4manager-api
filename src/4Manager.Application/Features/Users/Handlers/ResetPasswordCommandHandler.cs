using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using _4Manager.Application.Features.Users.Commands;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Domain.Interfaces;

namespace _4Manager.Application.Features.Users.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, UserDto>
    {
        private readonly IUserRepository _repository;

        public ResetPasswordCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            if (request.NewPassword != request.ConfirmPassword)
                throw new Exception("As senhas não coincidem.");

            user.PasswordHash = request.NewPassword; 

            await _repository.UpdateAsync(user);

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}

