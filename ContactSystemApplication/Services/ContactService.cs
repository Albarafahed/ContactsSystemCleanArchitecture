
using ContactsSystemApplication.DTO;
using ContactsSystemApplication.DTOs;
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Services;

public sealed class ContactService(IContactRepository contactRepository, IAuditLogService auditLogService, 
                        IUnitOfWork unitOfWork, IContactAccessService contactAccessService) : IContactService
{
    public async Task<IReadOnlyList<ContactDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var contacts = await contactAccessService
            .GetAccessibleContactsAsync(cancellationToken);

        return contacts.Select(MapToDto).ToList();
    }

    public async Task<ContactDto> GetByIdAsync(
        int contactId,
        CancellationToken cancellationToken = default)
    {
        var contact = await contactAccessService
            .GetAccessibleContactAsync(contactId, cancellationToken);

        return MapToDto(contact);
    }

    public async Task<ContactDto> CreateAsync(
        SaveContactRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userId = contactAccessService.GetCurrentUserId();

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
        if(await contactRepository.ExitEmailAsync(request.Email, cancellationToken))
        {
            throw new InvalidOperationException("A contact with the specified email already exists.");
        }

        await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                await contactRepository.AddAsync(contact, ct);

                await contactRepository.SaveChangesAsync(ct);

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

        var contact = await contactAccessService
            .GetAccessibleContactAsync(contactId, cancellationToken);

        var userId = contactAccessService.GetCurrentUserId();

        contact.SetFirstName(request.FirstName);
        contact.SetLastName(request.LastName);
        contact.SetEmail(request.Email);
        contact.SetPhone(request.Phone);
        contact.SetAddress(request.Address);
        contact.SetDateOfBirth(request.DateOfBirth);
        contact.SetCountryId(request.CountryId);
        contact.SetImagePath(request.ImagePath);

        // OwnerUserId is never changed during an update.
        await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                await contactRepository.SaveChangesAsync(ct);

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
        var contact = await contactAccessService
            .GetAccessibleContactAsync(contactId, cancellationToken);

        var userId = contactAccessService.GetCurrentUserId();

        return await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                contactRepository.Delete(contact);

                var deleted = await contactRepository.SaveChangesAsync(ct);

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
        contactAccessService.GetCurrentUserId();

        if (contactId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(contactId),
                "Contact ID must be greater than zero.");
        }

        var contact = await contactRepository.GetByIdAsync(
            contactId,
            cancellationToken);

        if (contact is null)
            return false;

        contactAccessService.EnsureCanAccess(contact);

        return true;
    }

    private Task LogAuditAsync(
        int? userId,
        string action,
        string entityName,
        string? entityId,
        string outcome,
        CancellationToken cancellationToken)
    {
        return auditLogService.LogAsync(new CreateAuditLogRequestDto{
            UserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Outcome = outcome,
            SaveImmediately= true,
        },
            cancellationToken: cancellationToken);
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
