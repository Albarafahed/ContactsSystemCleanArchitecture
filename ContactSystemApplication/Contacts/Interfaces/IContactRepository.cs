using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(int id);
    Task<Contact?> GetByIdForUpdateAsync(int id);
    Task<List<Contact>> GetAllAsync();
    Task<bool> AddAsync(Contact contact);
    Task<bool> SaveChangesAsync();
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? exceptContactId = null);
}