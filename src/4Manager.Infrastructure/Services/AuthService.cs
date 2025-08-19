using _4Tech._4Manager.Application.Common.Exceptions;
using _4Tech._4Manager.Application.Interfaces;
using _4Tech._4Manager.Domain.Entities;
using MediatR;
using Supabase.Gotrue;
using System.Linq.Expressions;
using System.Security.Authentication;


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

                return new AuthResult(userId, session.AccessToken, session.RefreshToken);
            }
            catch (Exception ex)
            {
                throw TokenException.MapAuthException(ex);
            }
        }

        public async Task ResetPasswordForEmail(string email)
        {
            try
            {
                var response = await _client.Auth.ResetPasswordForEmail(email);
                if (!response)

                    throw new AuthenticationException("Erro ao solicitar redefinição de senha.");
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

        public async Task UpdatePasswordAsync(Guid UserId, string newPassword)
        {
            try
            {
                var userAttributes = new UserAttributes { Password = newPassword };

                var updateUser = await _client.Auth.Update(userAttributes);

                if (updateUser?.Id == null || !Guid.TryParse(updateUser.Id, out var updateId))
                    throw new AuthenticationException("Falha ao atualizar a senha.");
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
