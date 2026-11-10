
namespace ContactsSystemApplication.Requests;

public sealed class SaveContactRequest
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }

    public int CountryId { get; init; }

    public string? ImagePath { get; init; }
}
