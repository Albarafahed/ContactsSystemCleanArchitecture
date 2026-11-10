
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default);
    Task<RefreshToken?> TryRevokeActiveAsync(
    string tokenHash,
    DateTime now,
    CancellationToken cancellationToken = default);
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(
    int userId,
    CancellationToken cancellationToken = default);
}
