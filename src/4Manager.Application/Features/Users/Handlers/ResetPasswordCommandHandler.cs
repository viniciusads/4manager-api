using _4Tech._4Manager.Application.Features.Users.Commands;
using _4Tech._4Manager.Application.Interfaces;
using MediatR;
using Supabase;
using System.Security.Authentication;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IAuthService _authService;

        public ResetPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new AuthenticationException("O e-mail não pode ser nulo ou vazio.");

            await _authService.ResetPasswordForEmail(request.Email);
            return Unit.Value;

        }
       
    }
}

