using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using _4Tech._4Manager.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(IAuthService authService, IConfiguration configuration, ILogger<ResetPasswordCommandHandler> logger)
        {
            _authService = authService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new AuthenticationException("O e-mail não pode ser nulo ou vazio.");

            var correctRoute = _configuration.GetSection("FrontendSettings")["ResetPasswordUrl"];

            await _authService.ResetPasswordForEmail(request.Email, correctRoute);

            _logger.LogInformation(Messages.User.EmailResetPassword);

            return Unit.Value;
        }
       
    }
}

