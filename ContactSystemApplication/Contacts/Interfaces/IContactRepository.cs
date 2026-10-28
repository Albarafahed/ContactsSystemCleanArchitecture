using ContactsSystemApplication.Contacts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemApplication.Contacts.Interfaces
{
    public interface IContactRepository
    {
        Task<ContactDto?> GetByIdAsync(int id);
        Task<List<ContactDto>> GetAllAsync();
        Task<ContactDto> AddAsync(ContactDto contact);
        Task<bool> UpdateAsync(ContactDto contact);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
