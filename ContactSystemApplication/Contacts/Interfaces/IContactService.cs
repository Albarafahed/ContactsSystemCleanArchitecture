using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Requests;

namespace ContactsSystemApplication.Contacts.Interfaces;

public interface IContactService
{
    Task<ContactDto?> GetContactByIdAsync(int id, string? ownerUserId);
    Task<List<ContactDto>> GetAllContactsAsync(string? ownerUserId);
    Task<ContactDto?> SaveAsync(int id, SaveContactRequest request, string ownerUserId);
    Task<bool> DeleteContactAsync(int id, string? ownerUserId);
    Task<bool> ExistsAsync(int id, string? ownerUserId);
}
