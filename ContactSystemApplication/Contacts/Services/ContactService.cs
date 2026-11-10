using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Requests;
using ContactsSystemApplication.Countries.Interfaces;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Contacts.Services;

public sealed class ContactService(
    IContactRepository contactRepository,
    ICountryRepository countryRepository) : IContactService
{
    public async Task<ContactDto?> GetContactByIdAsync(int id, string? ownerUserId)
    {
        var contact = await contactRepository.GetByIdAsync(id, ownerUserId);
        return contact is null ? null : MapToDto(contact);
    }

    public async Task<List<ContactDto>> GetAllContactsAsync(string? ownerUserId)
    {
        var contacts = await contactRepository.GetAllAsync(ownerUserId);
        return contacts.Select(MapToDto).ToList();
    }

    public async Task<ContactDto?> SaveAsync(int id, SaveContactRequest request, string? ownerUserId)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Contact ID cannot be negative.");
        if (id == 0 && string.IsNullOrWhiteSpace(ownerUserId))
            throw new ArgumentException("Authenticated owner is required to create a contact.", nameof(ownerUserId));
        if (!await countryRepository.ExistsByIdAsync(request.CountryId))
            throw new ArgumentException($"Country with ID {request.CountryId} does not exist.", nameof(request.CountryId));

        if (id == 0)
        {
            if (await contactRepository.EmailExistsAsync(request.Email))
                throw new ArgumentException("A contact with this email address already exists.", nameof(request.Email));

            var newContact = new Contact(request.FirstName, request.LastName, request.Email,
                request.Phone, request.Address, request.DateOfBirth, request.CountryId, request.ImagePath);
            newContact.AssignToUser(ownerUserId!);

            if (!await contactRepository.AddAsync(newContact))
                return null;
            var created = await contactRepository.GetByIdAsync(newContact.ContactId, ownerUserId);
            return created is null ? null : MapToDto(created);
        }

        var existing = await contactRepository.GetByIdForUpdateAsync(id, ownerUserId);
        if (existing is null)
            return null;
        if (await contactRepository.EmailExistsAsync(request.Email, id))
            throw new ArgumentException("A contact with this email address already exists.", nameof(request.Email));

        existing.SetFirstName(request.FirstName);
        existing.SetLastName(request.LastName);
        existing.SetEmail(request.Email);
        existing.SetPhone(request.Phone);
        existing.SetAddress(request.Address);
        existing.SetDateOfBirth(request.DateOfBirth);
        existing.SetCountryId(request.CountryId);
        existing.SetImagePath(request.ImagePath);

        if (!await contactRepository.SaveChangesAsync())
            return null;
        var updated = await contactRepository.GetByIdAsync(id, ownerUserId);
        return updated is null ? null : MapToDto(updated);
    }

    public Task<bool> DeleteContactAsync(int id, string? ownerUserId) =>
        contactRepository.DeleteAsync(id, ownerUserId);

    public Task<bool> ExistsAsync(int id, string? ownerUserId) =>
        contactRepository.ExistsAsync(id, ownerUserId);

    private static ContactDto MapToDto(Contact contact) => new()
    {
        ContactId = contact.ContactId,
        FirstName = contact.FirstName,
        LastName = contact.LastName,
        Email = contact.Email,
        Phone = contact.Phone,
        Address = contact.Address,
        DateOfBirth = contact.DateOfBirth,
        CountryId = contact.CountryId,
        CountryName = contact.Country?.CountryName ?? string.Empty,
        ImagePath = contact.ImagePath ?? string.Empty
    };
}
