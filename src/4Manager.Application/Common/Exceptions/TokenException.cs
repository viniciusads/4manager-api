using System.Security.Authentication;

namespace _4Tech._4Manager.Application.Common.Exceptions
{
    public static class TokenException 
    {
        public static Exception MapAuthException(Exception ex)
        {
            if (ex.Message.Contains("Invalid login credentials", StringComparison.OrdinalIgnoreCase))
                return new AuthenticationException("E-mail ou senha incorretos.", ex);

            if (ex.Message.Contains("Email not confirmed", StringComparison.OrdinalIgnoreCase))
                return new AuthenticationException("Confirme seu e-mail antes de fazer login.", ex);

            if (ex.Message.Contains("user_already_exists", StringComparison.OrdinalIgnoreCase))
                return new AuthenticationException("Este endereço de e-mail já está sendo utilizado.", ex);
            
            if (ex.Message.Contains("Invalid token", StringComparison.OrdinalIgnoreCase))
                return new AuthenticationException("Token inválido ou expirado.", ex);
           
            if (ex.Message.Contains("User not authenticated", StringComparison.OrdinalIgnoreCase))
                return new AuthenticationException("Usuário não autenticado.", ex);

            if (ex.Message.Contains("session_not_found"))
                return new AuthenticationException("Sessão não encontrada", ex);

            if (ex.Message.Contains("same_password", StringComparison.OrdinalIgnoreCase))
                return new AuthenticationException("A nova senha deve ser diferente da senha atual.", ex);

            if (ex.Message.Contains("user_already_exists"))
                return new AuthenticationException("Este endereço de e-mail já está sendo utilizado.", ex);

            return new AuthenticationException($"Falha na autenticação: {ex.Message}", ex);
        }
    }
}
