using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Features.Users.Commands;
using MediatR;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class LoginRequestCommandHandler : IRequestHandler<LoginRequestCommand, AuthResult>
    {
       
        private readonly IAuthService _authService;
        private readonly IUserRepository _repository;

        public LoginRequestCommandHandler(IAuthService authService, IUserRepository repository )
        {
         
            _authService = authService;
            _repository = repository;
        }

        public async Task<AuthResult> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _authService.LoginAsync(request.Email, request.Password);

            var user = await _repository.GetByEmailAsync(request.Email, cancellationToken);

          return new AuthResult(
              user.UserId,
              loginResult.AccessToken,
              loginResult.RefreshToken
              ); 
        }
    }
}

