
namespace ContactsSystemDomain.Entities;

public sealed class RefreshToken
{
    public int Id { get; private set; }

    public int UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTime ExpiresAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    // Navigation property
    public User User { get; private set; } = null!;

    private RefreshToken() { }

    public RefreshToken(
        int userId,
        string tokenHash,
        DateTime expiresAt)
    {
        SetUserId(userId);
        SetTokenHash(tokenHash);
        SetExpiresAt(expiresAt);

        CreatedAt = DateTime.UtcNow;
    }

    public void SetUserId(int userId)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(userId),
                "User ID must be greater than zero.");

        UserId = userId;
    }

    public void SetTokenHash(string tokenHash)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException(
                "Token hash is required.", nameof(tokenHash));

        if (tokenHash.Length != 64)
            throw new ArgumentException(
                "Token hash must contain exactly 64 characters.",
                nameof(tokenHash));

        TokenHash = tokenHash;
    }

    public void SetExpiresAt(DateTime expiresAt)
    {
        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException(
                "Expiration date must be in the future.",
                nameof(expiresAt));

        ExpiresAt = expiresAt;
    }

    public bool IsExpired() => ExpiresAt <= DateTime.UtcNow;

    public bool IsRevoked() => RevokedAt.HasValue;

    public bool IsActive() => !IsExpired() && !IsRevoked();

    public void Revoke()
    {
        if (RevokedAt.HasValue)
            return;

        RevokedAt = DateTime.UtcNow;
    }
}
