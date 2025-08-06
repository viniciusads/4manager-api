

namespace _4Manager.Application.Features.Users.Dtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}