
using ContactsSystemApplication.AuditLogs.Interfaces;
using ContactsSystemApplication.DTO;
using ContactsSystemApplication.Exceptions;
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Services;

public sealed class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IAuditLogService _auditLogService;
    private readonly IUnitOfWork _unitOfWork;

    public ContactService(
        IContactRepository contactRepository,
        ICurrentUserService currentUser,
        IAuditLogService auditLogService,
        IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _currentUser = currentUser;
        _auditLogService = auditLogService;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ContactDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();

        var contacts = _currentUser.IsAdmin
            ? await _contactRepository.GetAllAsync(cancellationToken)
            : await _contactRepository.GetByOwnerIdAsync(
                GetCurrentUserId(), cancellationToken);

        return contacts.Select(MapToDto).ToList();
    }

    public async Task<ContactDto> GetByIdAsync(
        int contactId,
        CancellationToken cancellationToken = default)
    {
        var contact = await GetAccessibleContactAsync(
            contactId, cancellationToken);

        return MapToDto(contact);
    }

    public async Task<ContactDto> CreateAsync(
        SaveContactRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        ArgumentNullException.ThrowIfNull(request);

        var userId = GetCurrentUserId();

        var contact = new Contact(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.Address,
            request.DateOfBirth,
            request.CountryId,
            request.ImagePath);

        // Ownership comes from the authenticated user.
        contact.AssignToUser(userId);

        await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                await _contactRepository.AddAsync(contact, ct);

                await _contactRepository.SaveChangesAsync(ct);

                await LogAuditAsync(
                    userId,
                    "Create",
                    "Contact",
                    contact.ContactId.ToString(),
                    "Success",
                    ct);

                return true;
            },
            cancellationToken);

        return MapToDto(contact);
    }

    public async Task<ContactDto> UpdateAsync(
        int contactId,
        SaveContactRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var contact = await GetAccessibleContactAsync(
            contactId, cancellationToken);

        var userId = GetCurrentUserId();

        contact.SetFirstName(request.FirstName);
        contact.SetLastName(request.LastName);
        contact.SetEmail(request.Email);
        contact.SetPhone(request.Phone);
        contact.SetAddress(request.Address);
        contact.SetDateOfBirth(request.DateOfBirth);
        contact.SetCountryId(request.CountryId);
        contact.SetImagePath(request.ImagePath);

        // OwnerUserId is never changed during an update.
        await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                await _contactRepository.SaveChangesAsync(ct);

                await LogAuditAsync(
                    userId,
                    "Update",
                    "Contact",
                    contact.ContactId.ToString(),
                    "Success",
                    ct);

                return true;
            },
            cancellationToken);

        return MapToDto(contact);
    }

    public async Task<bool> DeleteAsync(
        int contactId,
        CancellationToken cancellationToken = default)
    {
        var contact = await GetAccessibleContactAsync(
            contactId, cancellationToken);

        var userId = GetCurrentUserId();

        return await _unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                _contactRepository.Delete(contact);

                var deleted = await _contactRepository.SaveChangesAsync(ct);

                if (deleted)
                {
                    await LogAuditAsync(
                        userId,
                        "Delete",
                        "Contact",
                        contact.ContactId.ToString(),
                        "Success",
                        ct);
                }

                return deleted;
            },
            cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int contactId,
        CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();

        if (contactId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(contactId),
                "Contact ID must be greater than zero.");

        var contact = await _contactRepository.GetByIdAsync(
            contactId,
            cancellationToken);

        if (contact is null)
            return false;

        if (!_currentUser.IsAdmin &&
            contact.OwnerUserId != GetCurrentUserId())
        {
            throw new ForbiddenAccessException(
                "You are not allowed to access this contact.");
        }

        return true;
    }

    private async Task<Contact> GetAccessibleContactAsync(
        int contactId,
        CancellationToken cancellationToken)
    {
        EnsureAuthenticated();

        if (contactId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(contactId),
                "Contact ID must be greater than zero.");

        var contact = await _contactRepository.GetByIdAsync(
            contactId, cancellationToken);

        if (contact is null)
            throw new KeyNotFoundException(
                "Contact was not found.");

        if (!_currentUser.IsAdmin &&
            contact.OwnerUserId != GetCurrentUserId())
        {
            throw new ForbiddenAccessException(
                "You are not allowed to access this contact.");
        }

        return contact;
    }

    private Task LogAuditAsync(
        int? userId,
        string action,
        string entityName,
        string? entityId,
        string outcome,
        CancellationToken cancellationToken)
    {
        return _auditLogService.LogAsync(
            userId,
            action,
            entityName,
            entityId,
            outcome,
            saveImmediately: true,
            cancellationToken: cancellationToken);
    }

    private void EnsureAuthenticated()
    {
        if (!_currentUser.IsAuthenticated ||
            !_currentUser.UserId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "Authentication is required.");
        }
    }

    private int GetCurrentUserId()
    {
        EnsureAuthenticated();
        return _currentUser.UserId!.Value;
    }

    private static ContactDto MapToDto(Contact contact)
    {
        return new ContactDto
        {
            ContactId = contact.ContactId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Phone = contact.Phone,
            Address = contact.Address,
            DateOfBirth = contact.DateOfBirth,
            CountryId = contact.CountryId,
            ImagePath = contact.ImagePath
        };
    }
}
