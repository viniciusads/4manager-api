using _4Manager.Application.Features.Users.Commands;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Interfaces;
using _4Manager.Domain.Entities;
using _4Manager.Domain.Enums;
using MediatR;
using Supabase;

namespace _4Manager.Application.Features.Users.Handlers
{
    public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _repository;
        private readonly IAuthService _authService;

        public SignUpUserCommandHandler(IAuthService authService, IUserRepository repository)
        {
            _authService = authService;
            _repository = repository;
        }

        public async Task<UserResponseDto> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                throw new Exception("As senhas não coincidem.");

            var session = await _authService.SignUpAsync(request.Email, request.Password);

            if (session?.User?.Id == null)
                throw new Exception("Falha ao criar usuário no Supabase.");

            var supabaseUserId = session.User.Id;

            var user = new User
            {
                UserId = Guid.Parse(supabaseUserId),
                Name = request.Name,
                Email = request.Email,
                isActive = true,
                Role = RoleEnum.Analista
            };

            await _repository.AddUserAsync(user);

            return new UserResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                isActive = true,
                Role = user.Role.ToString()
            };
        }
    }
}
