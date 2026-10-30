using ContactsSystemApplication.Countries.DTOs;

namespace ContactsSystemApplication.Countries.Interfaces
{
    public interface ICountryService
    {
        Task<List<CountryDto>> GetAllCountriesAsync();
        Task<CountryDto?> GetCountryByIdAsync(int id);
        Task<CountryDto?> GetCountryByNameAsync(string name);
        Task<bool> ExistsCountryByIdAsync(int id);
        Task<bool> ExistsCountryByNameAsync(string name);
    }
}
