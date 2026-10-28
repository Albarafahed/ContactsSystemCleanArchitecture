using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
