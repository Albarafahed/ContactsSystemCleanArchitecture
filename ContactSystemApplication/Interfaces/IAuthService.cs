
using ContactsSystemApplication.Auth;
using ContactsSystemApplication.Requests;

namespace ContactsSystemApplication.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default);

    Task LogoutAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default);
}
