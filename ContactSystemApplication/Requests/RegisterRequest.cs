
using ContactsSystemApplication.Requests;

namespace ContactsSystemApplication.Requests;

public sealed class RegisterRequest
{
    public SaveContactRequest Contact { get; init; } = null!;

    public string Password { get; init; } = string.Empty;

    public string ConfirmPassword { get; init; } = string.Empty;
}
