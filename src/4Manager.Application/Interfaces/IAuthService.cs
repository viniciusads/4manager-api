using Microsoft.Win32.SafeHandles;
using Supabase.Gotrue;
using System.Globalization;

namespace _4Manager.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Session?> SignUpAsync( string email, string password);
        Task<(string AccessToken, string? RefreshToken)> LoginAsync(string email, string password);
        Task ResetPasswordForEmail(string email);
        Task UpdatePasswordAsync(string newPassword);
    }
}
