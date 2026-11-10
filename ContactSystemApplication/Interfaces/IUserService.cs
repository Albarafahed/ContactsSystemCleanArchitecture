using ContactsSystemApplication.Requests;
using ContactsSystemApplication.Users;

namespace ContactsSystemApplication.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<UserManagementDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<UserManagementDto?> GetByIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task ChangeRoleAsync(int userId,
        UpdateUserRoleRequest request,
        CancellationToken cancellationToken = default);

    Task ActivateAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task DeactivateAsync(
        int userId,
        CancellationToken cancellationToken = default);
}