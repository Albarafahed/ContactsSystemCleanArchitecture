
namespace ContactsSystemApplication.Auth;

public sealed class UserDto
{
    public int UserId { get; init; }

    public int ContactId { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
