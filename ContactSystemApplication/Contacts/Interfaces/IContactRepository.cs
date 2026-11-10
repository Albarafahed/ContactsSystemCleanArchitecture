using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(int id, string? ownerUserId);
    Task<Contact?> GetByIdForUpdateAsync(int id, string? ownerUserId);
    Task<List<Contact>> GetAllAsync(string? ownerUserId);
    Task<bool> AddAsync(Contact contact);
    Task<bool> SaveChangesAsync();
    Task<bool> DeleteAsync(int id, string? ownerUserId);
    Task<bool> ExistsAsync(int id, string? ownerUserId);
    Task<bool> EmailExistsAsync(string email, int? exceptContactId = null);
}
