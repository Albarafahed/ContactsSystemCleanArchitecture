using ContactsSystemApplication.Countries.DTOs;
using ContactsSystemApplication.Countries.Interfaces;
using ContactsSystemDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemApplication.Countries.Services
{
    internal class CountryService(ICountryRepository countryRepository) : ICountryService
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
