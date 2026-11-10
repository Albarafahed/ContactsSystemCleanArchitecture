using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Requests;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Contacts.Services;

public sealed class ContactService(IContactRepository contactRepository)
    : IContactService
{
    public async Task<ContactDto?> GetContactByIdAsync(int id)
    {
        var contact = await contactRepository.GetByIdAsync(id);
        return contact is null ? null : MapToDto(contact);
    }

    public async Task<List<ContactDto>> GetAllContactsAsync()
    {
        var contacts = await contactRepository.GetAllAsync();
        return contacts.Select(MapToDto).ToList();
    }

    public async Task<ContactDto?> SaveAsync(int id, SaveContactRequest request)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Contact ID cannot be negative.");

        if (id == 0)
        {
            if (await contactRepository.EmailExistsAsync(request.Email))
                throw new ArgumentException("A contact with this email address already exists.", nameof(request.Email));

            var newContact = new Contact(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                request.Address,
                request.DateOfBirth,
                request.CountryId,
                request.ImagePath);

            if (!await contactRepository.AddAsync(newContact))
                return null;

            var createdContact = await contactRepository.GetByIdAsync(newContact.ContactId);
            return createdContact is null ? null : MapToDto(createdContact);
        }

        var existingContact = await contactRepository.GetByIdForUpdateAsync(id);
        if (existingContact is null)
            return null;

        if (await contactRepository.EmailExistsAsync(request.Email, id))
            throw new ArgumentException("A contact with this email address already exists.", nameof(request.Email));

        existingContact.SetFirstName(request.FirstName);
        existingContact.SetLastName(request.LastName);
        existingContact.SetEmail(request.Email);
        existingContact.SetPhone(request.Phone);
        existingContact.SetAddress(request.Address);
        existingContact.SetDateOfBirth(request.DateOfBirth);
        existingContact.SetCountryId(request.CountryId);
        existingContact.SetImagePath(request.ImagePath);

        if (!await contactRepository.SaveChangesAsync())
            return null;

        var updatedContact = await contactRepository.GetByIdAsync(id);
        return updatedContact is null ? null : MapToDto(updatedContact);
    }

    public Task<bool> DeleteContactAsync(int id) =>
        contactRepository.DeleteAsync(id);

    public Task<bool> ExistsAsync(int id) =>
        contactRepository.ExistsAsync(id);

    private static ContactDto MapToDto(Contact contact) =>
        new()
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
