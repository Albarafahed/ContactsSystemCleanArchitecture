namespace ContactsSystemInfrastructure.Identity;

public sealed class RefreshToken
{
    public int Id { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    public byte[] RowVersion { get; set; } = System.Security.Cryptography.RandomNumberGenerator.GetBytes(8);
    public bool IsRevoked => RevokedAtUtc.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
}
