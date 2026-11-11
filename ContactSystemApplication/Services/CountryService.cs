using ContactsSystemApplication.DTO;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemApplication.Services
{
    public class CountryService(ICountryRepository countryRepository) : ICountryService
    {
        public async Task<List<CountryDto>> GetAllCountriesAsync()
        {
            var countries = await countryRepository.GetAllAsync();
            return countries.Select(MapToDto).ToList();
        }
        public async Task<CountryDto?> GetCountryByIdAsync(int id)
        {
            var country = await countryRepository.GetByIdAsync(id);
            return country is null ? null : MapToDto(country);
        }
        public async Task<CountryDto?> GetCountryByNameAsync(string name)
        {
            var country = await countryRepository.GetByNameAsync(name);
            return country is null ? null : MapToDto(country);
        }
        public async Task<bool> ExistsCountryByIdAsync(int id) => await countryRepository.ExistsByIdAsync(id);
        public async Task<bool> ExistsCountryByNameAsync(string name) => await countryRepository.ExistsByNameAsync(name);
        private static CountryDto MapToDto(Country country)
        {
            return new CountryDto
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName!,
                Code = country.Code!,
                PhoneCode = country.PhoneCode!
            };
        }
    
    }
}
