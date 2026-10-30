using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(int id);

    Task<List<Contact>> GetAllAsync();

    Task AddAsync(Contact contact);

    Task UpdateAsync(Contact contact);

    Task DeleteAsync(Contact contact);

    Task<bool> ExistsAsync(int id);
}