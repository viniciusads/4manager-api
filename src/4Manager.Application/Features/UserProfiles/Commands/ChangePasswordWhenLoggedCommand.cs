using MediatR;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Commands
{
    public class ChangePasswordWhenLoggedCommand : IRequest<Unit>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public Guid AuthenticatedUserId { get; set; }

        public ChangePasswordWhenLoggedCommand(string accessToken, string newPassword, string refreshToken, string currentPassword, Guid authenticatedUserId)
        {
            AccessToken = accessToken;
            NewPassword = newPassword;
            RefreshToken = refreshToken;
            CurrentPassword = currentPassword;
            AuthenticatedUserId = authenticatedUserId;
        }
    }
}
