namespace _4Tech._4Manager.Application.Interfaces
{
    public record AuthResult(Guid UserId, string AccessToken, string? RefreshToken);
    public interface IAuthService
    {
        Task<AuthResult> SignUpAsync(string email, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task ResetPasswordForEmail(string email, string url);
        Task SoftDeleteUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<Guid> GetCurrentUserAsync();

    }
}
