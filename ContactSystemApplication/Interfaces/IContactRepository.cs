
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IContactRepository
{
    Task<IReadOnlyList<Contact>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Contact>> GetByOwnerIdAsync(
        int ownerUserId,
        CancellationToken cancellationToken = default);

    Task<Contact?> GetByIdAsync(
        int contactId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Contact contact,
        CancellationToken cancellationToken = default);

    void Delete(Contact contact);
    Task<bool> ExitEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> SaveChangesAsync(
        CancellationToken cancellationToken = default);

 
}
