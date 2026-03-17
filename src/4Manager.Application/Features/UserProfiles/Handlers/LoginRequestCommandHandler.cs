using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using _4Tech._4Manager.Application.Common.Strings;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class LoginRequestCommandHandler : IRequestHandler<LoginRequestCommand, AuthResult>
    {
       
        private readonly IAuthService _authService;
        private readonly ILogger<LoginRequestCommandHandler> _logger;

        public LoginRequestCommandHandler(IAuthService authService, ILogger<LoginRequestCommandHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<AuthResult> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
        {
          var loginResult = await _authService.LoginAsync(request.Email, request.Password);

            _logger.LogInformation(Messages.User.LoginSuccess);

          return new AuthResult(
              loginResult.UserId,
              loginResult.AccessToken,
              loginResult.RefreshToken
              ); 
        }
    }
}

