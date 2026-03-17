using MediatR;

namespace _4Tech._4Manager.Application.Features.UserProfiles.Commands
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public ChangePasswordCommand(string accessToken, string newPassword, string refreshToken)
        {
            AccessToken = accessToken;
            NewPassword = newPassword;
            RefreshToken = refreshToken;
        }
    }
}
