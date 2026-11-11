
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IContactAccessService
{
    int GetCurrentUserId();

    Task<IReadOnlyList<Contact>> GetAccessibleContactsAsync(
        CancellationToken cancellationToken = default);

    Task<Contact> GetAccessibleContactAsync(
        int contactId,
        CancellationToken cancellationToken = default);

    void EnsureCanAccess(Contact contact);
}
