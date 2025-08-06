using _4Manager.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Supabase.Gotrue;
using Supabase.Interfaces;


namespace _4Manager.Infrastructure.Services
{
    public class AuthService : IAuthService

    {

        private readonly Supabase.Client _client;

        public AuthService(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<(string AccessToken, string? RefreshToken)> LoginAsync(string email, string password)
        {
            try
            {
                var session = await _client.Auth.SignInWithPassword(email, password);

                if (session?.AccessToken == null)
                    throw new Exception("Token de acesso não gerado.");

                return (session.AccessToken, session.RefreshToken);
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("Invalid login credentials"))
                    throw new Exception("E-mail ou senha incorretos.");

                if (ex.Message.Contains("Email not confirmed"))
                    throw new Exception("Confirme seu e-mail antes de fazer login.");

                throw new Exception($"Falha na autenticação: {ex.Message}");
            }
        }

        public async Task ResetPasswordForEmail(string email)
        {
            var response = await _client.Auth.ResetPasswordForEmail(email);
            if (!response)
            {
                throw new Exception("Erro ao solicitar redefinição de senha.");
            }
        }

        public async Task<Session?> SignUpAsync(string email, string password)
        {
            var response = await _client.Auth.SignUp(email, password);
            if (response == null || response.User == null)
                throw new Exception("Erro ao criar usuário no Supabase.");
            return response;
        }

        public async Task UpdatePasswordAsync(string newPassword)
        {
            var userAttributes = new UserAttributes { Password = newPassword };

            var response = await _client.Auth.Update(userAttributes);

            if (response?.Id == null)
                throw new Exception("Falha ao atualizar a senha.");
        }

    }

}

