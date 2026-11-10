
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);

    string GenerateRefreshToken();

    string HashRefreshToken(string refreshToken);

    DateTime GetAccessTokenExpiration();
}
