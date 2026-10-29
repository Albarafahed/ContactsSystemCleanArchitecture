using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemApplication.Contacts.Services
{
    public class ContactService(IContactRepository contactRepository) : IContactService
    {
      public  Task<ContactDto?> GetContactByIdAsync(int id)=> contactRepository.GetByIdAsync(id);
             public  Task<List<ContactDto>> GetAllContactsAsync() => contactRepository.GetAllAsync();
        public async Task<ContactDto?> SaveAsync(int id, SaveContactRequest request)
        {
         var dto = new ContactDto
         {
             ContactId = id,
             FirstName = request.FirstName,
             LastName = request.LastName,
             Email = request.Email,
             Phone = request.Phone,
             Address = request.Address,
             DateOfBirth = request.DateOfBirth,
             CountryId = request.CountryId,
             ImagePath = request.ImagePath!
         };
            if( id == 0 ) return await contactRepository.AddAsync(dto);
            else return (await contactRepository.UpdateAsync(dto)) ? dto : null;
        }
       public async Task<bool> DeleteContactAsync(int id) => await contactRepository.DeleteAsync(id);
    }
}
