
using ContactsSystemApplication.Exceptions;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Services;

public sealed class ContactAccessService : IContactAccessService
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUser;

    public ContactAccessService(
        IContactRepository contactRepository,
        ICurrentUserService currentUser)
    {
        _contactRepository = contactRepository;
        _currentUser = currentUser;
    }

    public int GetCurrentUserId()
    {
        if (!_currentUser.IsAuthenticated ||
            !_currentUser.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "Authentication is required.");
        }

        return _currentUser.UserId.Value;
    }

    public async Task<IReadOnlyList<Contact>> GetAccessibleContactsAsync(
        CancellationToken cancellationToken = default)
    {
        GetCurrentUserId();

        if (_currentUser.IsAdmin)
        {
            return await _contactRepository.GetAllAsync(
                cancellationToken);
        }

        return await _contactRepository.GetByOwnerIdAsync(
            GetCurrentUserId(),
            cancellationToken);
    }

    public async Task<Contact> GetAccessibleContactAsync(
        int contactId,
        CancellationToken cancellationToken = default)
    {
        GetCurrentUserId();

        if (contactId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(contactId),
                "Contact ID must be greater than zero.");
        }

        var contact = await _contactRepository.GetByIdAsync(
            contactId,
            cancellationToken);

        if (contact is null)
        {
            throw new KeyNotFoundException(
                "Contact was not found.");
        }

        EnsureCanAccess(contact);

        return contact;
    }

    public void EnsureCanAccess(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        GetCurrentUserId();

        if (!_currentUser.IsAdmin &&
            contact.OwnerUserId != _currentUser.UserId)
        {
            throw new ForbiddenAccessException(
                "You are not allowed to access this contact.");
        }
    }
}
