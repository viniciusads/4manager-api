using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Interfaces;
using Supabase.Gotrue;
using System.Security.Authentication;
using System.Security.Claims;


namespace _4Tech._4Manager.Infrastructure.Services
{
    public class AuthService : IAuthService

    {
        private readonly Supabase.Client _client;
        private readonly IUserRepository _userRepository;

        public AuthService(Supabase.Client client, IUserRepository userRepository)
        {
            _client = client;
            _userRepository = userRepository;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                var session = await _client.Auth.SignInWithPassword(email, password);

                if (session?.User == null || !Guid.TryParse(session.User.Id, out var userId))
                    throw new AuthenticationException("Erro no login.");

                var user = await _userRepository.GetByIdAsync(userId, CancellationToken.None);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("accessLevel", user.AcessLevel.ToString())
                };

                return new AuthResult(userId, session.AccessToken, session.RefreshToken);
            }
            catch (Exception ex)
            {
                throw TokenException.MapAuthException(ex);
            }
        }

        public async Task ResetPasswordForEmail(string email, string redirectTo)
        {
            try
            {
                var options = new Supabase.Gotrue.ResetPasswordForEmailOptions(email) { RedirectTo = redirectTo };
                var response = await _client.Auth.ResetPasswordForEmail(options);
            }
            catch (Exception ex)
            {
                throw TokenException.MapAuthException(ex);
            }
        }

        public async Task<AuthResult> SignUpAsync(string email, string password)
        {
            try
            {
                var response = await _client.Auth.SignUp(email, password);
                if (response?.User == null || !Guid.TryParse(response.User.Id, out var userId))
                    throw new AuthenticationException("Erro ao criar usuário.");
                return new AuthResult(userId, response.AccessToken, response.RefreshToken);
            }
            catch (Exception ex)
            {
                throw TokenException.MapAuthException(ex);
            }
        }

        public async Task SoftDeleteUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new AuthenticationException("Usuário não encontrado.");

            user.IsActive = false;
            await _userRepository.UpdateUserAsync(user, cancellationToken);


        }
    }
}
