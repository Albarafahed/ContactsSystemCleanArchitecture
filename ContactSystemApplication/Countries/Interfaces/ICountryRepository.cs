using ContactsSystemApplication.Countries.DTOs;

namespace ContactsSystemApplication.Countries.Interfaces
{
    public interface ICountryRepository
    {
        Task<CountryDto?> GetByIdAsync(int id);
        Task<CountryDto?> GetByNameAsync(string name);
        Task<List<CountryDto>> GetAllAsync();
        Task<CountryDto> AddAsync(CountryDto country);
        Task<bool> UpdateAsync(CountryDto country);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
