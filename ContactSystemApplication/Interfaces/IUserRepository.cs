
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> GetAllAsync(
    CancellationToken cancellationToken = default);

    Task<int> CountActiveAdminsAsync(
        CancellationToken cancellationToken = default);
}
