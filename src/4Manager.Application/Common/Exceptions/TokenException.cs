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

            return new AuthenticationException($"Falha na autenticação: {ex.Message}", ex);
        }
    }
}
