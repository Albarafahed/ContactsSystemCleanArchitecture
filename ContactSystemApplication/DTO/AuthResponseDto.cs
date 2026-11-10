
namespace ContactsSystemApplication.Auth;

public sealed class AuthResponseDto
{
    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public DateTime AccessTokenExpiresAt { get; init; }

    public UserDto User { get; init; } = null!;
}
