
namespace ContactsSystemDomain.Entities;

public sealed class User
{
    public int UserId { get; private set; }

    public int ContactId { get; private set; }

    public string PasswordHash { get; private set; } = string.Empty;

    public string Role { get; private set; } = "User";

    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public Contact ProfileContact { get; private set; } = null!;

    public ICollection<Contact> Contacts { get; private set; }
        = new List<Contact>();

    public ICollection<RefreshToken> RefreshTokens { get; private set; }
        = new List<RefreshToken>();

    private User() { }

    public User(int contactId, string passwordHash)
    {
        SetContactId(contactId);
        SetPasswordHash(passwordHash);
    }

    public void SetContactId(int contactId)
    {
        if (contactId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(contactId),
                "Contact ID must be greater than zero.");

        ContactId = contactId;
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException(
                "Password hash is required.",
                nameof(passwordHash));

        if (passwordHash.Length > 255)
            throw new ArgumentException(
                "Password hash cannot exceed 255 characters.",
                nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void ChangeRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException(
                "Role is required.", nameof(role));

        var normalized = role.Trim();

        if (normalized is not ("Admin" or "User"))
            throw new ArgumentException(
                "Invalid user role.", nameof(role));

        Role = normalized;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
