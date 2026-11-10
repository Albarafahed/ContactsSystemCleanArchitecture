namespace ContactsSystemApplication.Requests;

public sealed class UpdateUserRoleRequest
{
    public string Role { get; init; } = string.Empty;
}