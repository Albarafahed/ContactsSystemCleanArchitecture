
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Repositories;

public sealed class RefreshTokenRepository (ContactsDbContext context) : IRefreshTokenRepository
{
  

    public async Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(
                token => token.TokenHash == tokenHash,
                cancellationToken);
    }

    public async Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await context.RefreshTokens
            .Where(token =>
                token.UserId == userId &&
                token.RevokedAt == null &&
                token.ExpiresAt > now)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default)
    {
        await context.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
