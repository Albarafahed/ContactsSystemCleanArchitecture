using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Requests;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using System.Reflection.Metadata.Ecma335;

namespace ContactsSystemApplication.Contacts.Services;

public class ContactService(IContactRepository contactRepository)
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

    public async Task<ContactDto?> SaveAsync(
        int id,
        SaveContactRequest request)
    {
        bool Access = false;
        if (id == 0)
        {
            var newContact = new Contact(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Phone,
                request.Address,
                request.DateOfBirth,
                request.CountryId,
                request.ImagePath
            );

         Access=  await contactRepository.AddAsync(newContact);
            

            return Access ? MapToDto(newContact) : null;
        }

        var existingContact = await contactRepository.GetByIdForUpdateAsync(id);

        if (existingContact is null)
            return null;

        existingContact.SetFirstName(request.FirstName);
        existingContact.SetLastName(request.LastName);
        existingContact.SetEmail(request.Email);
        existingContact.SetPhone(request.Phone);
        existingContact.SetAddress(request.Address);
        existingContact.SetDateOfBirth(request.DateOfBirth);
        existingContact.SetCountryId(request.CountryId);
        existingContact.SetImagePath(request.ImagePath);

        Access = await contactRepository.SaveChangesAsync();

        return Access? MapToDto(existingContact) : null;
    }

    public async Task<bool> DeleteContactAsync(int id) => 
        await contactRepository.DeleteAsync(id);
    
    public async Task<bool> ExistsAsync(int id)=>
        await contactRepository.ExistsAsync(id);
   
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
            CountryName = contact.Country?.CountryName ?? string.Empty,
            ImagePath = contact.ImagePath ?? string.Empty
        };
    }
}