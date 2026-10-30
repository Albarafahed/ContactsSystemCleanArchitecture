using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Requests;
namespace ContactsSystemApplication.Contacts.Interfaces
{
    public interface IContactService
    {
        Task<ContactDto?> GetContactByIdAsync(int id);
        Task<List<ContactDto>> GetAllContactsAsync();
        Task<ContactDto?> SaveAsync(int id, SaveContactRequest request);
        Task<bool> DeleteContactAsync(int id);
    }
}
