using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country?> GetByIdAsync(int id);
        Task<Country?> GetByIdForUpdateAsync(int id);
        Task<Country?> GetByNameAsync(string name);
        Task<List<Country>> GetAllAsync();
        Task<bool> AddAsync(Country country);
        public Task<bool> SaveChangesAsync();
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
