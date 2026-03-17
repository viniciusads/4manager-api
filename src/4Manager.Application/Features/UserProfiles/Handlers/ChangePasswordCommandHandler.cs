using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Supabase.Gotrue;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly Supabase.Client _client;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(Supabase.Client client, ILogger<ChangePasswordCommandHandler> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _client.Auth.SetSession(request.AccessToken, request.RefreshToken);

            return await ChangePassword(request);
        }

        private async Task<Unit> ChangePassword(ChangePasswordCommand request)
        {

            var userAttributes = new UserAttributes
            {
                Password = request.NewPassword
            };

            try
            {
                var response = await _client.Auth.Update(userAttributes);

                _logger.LogInformation(Messages.User.ResetPassword);

                if (response == null)
                    throw new Exception(Messages.Auth.ErrorResetPassword);
            }
            catch (Exception ex)
            {
                throw TokenException.MapAuthException(ex);
            }
            return Unit.Value;
        }
    }
}
