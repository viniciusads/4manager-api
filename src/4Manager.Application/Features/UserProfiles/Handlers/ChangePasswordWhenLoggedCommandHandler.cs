using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Common.Strings;
using _4Tech._4Manager.Application.Features.UserProfiles.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Supabase.Gotrue;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Handlers
{
    public class ChangePasswordWhenLoggedCommandHandler : IRequestHandler<ChangePasswordWhenLoggedCommand, Unit>
    {
        private readonly Supabase.Client _client;
        private readonly ILogger<ChangePasswordWhenLoggedCommandHandler> _logger;

        public ChangePasswordWhenLoggedCommandHandler(Supabase.Client client, ILogger<ChangePasswordWhenLoggedCommandHandler> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Unit> Handle(ChangePasswordWhenLoggedCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _client.Auth.SetSession(request.AccessToken, request.RefreshToken);
            }
            catch (Exception ex)
            {
                throw TokenException.MapAuthException(ex);
            }

            await getCurrentUserPassword(request);

            return await ChangePassword(request);
        }

        private async Task getCurrentUserPassword(ChangePasswordWhenLoggedCommand request)
        {
            var currentUser = _client.Auth.CurrentUser;
            try
            {
                var newSession = await _client.Auth.SignIn(currentUser.Email, request.CurrentPassword);
            }
            catch (Exception ex)
            {
                throw TokenException.MapAuthException(ex);
            }
        }

        private async Task<Unit> ChangePassword(ChangePasswordWhenLoggedCommand request)
        {
            var currentUserId = Guid.Parse(_client.Auth.CurrentUser.Id);

            if (currentUserId != request.AuthenticatedUserId)
                throw new UnauthorizedAccessException(Messages.Auth.UnauthorizedAction);

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
