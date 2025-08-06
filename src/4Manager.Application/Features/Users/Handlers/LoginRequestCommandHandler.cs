using _4Manager.Application.Features.Users.Commands;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Interfaces;
using MediatR;

namespace _4Manager.Application.Features.Users.Handlers
{
    public class LoginRequestCommandHandler : IRequestHandler<LoginRequestCommand, LoginResponseDto>
    {
       
        private readonly IAuthService _authService;
        private readonly IUserRepository _repository;

        public LoginRequestCommandHandler(IAuthService authService, IUserRepository repository )
        {
         
            _authService = authService;
            _repository = repository;
        }

        public async Task<LoginResponseDto> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
        {
            var (accessToken, refreshToken) = await _authService.LoginAsync(request.Email, request.Password);

            var user = await _repository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Email = request.Email
            };
        }
    }
}

