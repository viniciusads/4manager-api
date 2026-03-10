using _4Tech._4Manager.Application.Features.Users.Commands;
using MediatR;
using Supabase.Gotrue;

namespace _4Tech._4Manager.Application.Features.Users.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly Supabase.Client _client;

        public ChangePasswordCommandHandler(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _client.Auth.SetSession(request.AccessToken, request.RefreshToken);

            var userAttributes = new UserAttributes
            {
                Password = request.NewPassword
            };

            try
            {
                var response = await _client.Auth.Update(userAttributes);
                if (response == null)
                    throw new Exception("Erro ao atualizar a senha.");
            }
            catch (Supabase.Gotrue.Exceptions.GotrueException ex)
            {
                if (ex.Message.Contains("same_password"))
                    return Unit.Value;
            }

            return Unit.Value;
        }
    }
}
