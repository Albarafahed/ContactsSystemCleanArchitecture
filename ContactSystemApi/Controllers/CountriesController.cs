using ContactsSystemApplication.DTO;
using ContactsSystemApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsSystemApi.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [Route("api/Countries")]
    [ApiController]
    public class CountriesController(ICountryService countryService) : ControllerBase
    {

        [HttpGet("All", Name = "GetAllCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountriesAsync()
        {
            var countries = await countryService.GetAllCountriesAsync();
            if (countries.Count == 0) return NotFound("No countries found.");
            return Ok(countries);
        }

        [HttpGet("{id:int}", Name = "GetCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CountryDto>> GetCountryByIdAsync(int id)
        {
            if (id <= 0) return BadRequest($"Invalid country with id {id}.");
            var country = await countryService.GetCountryByIdAsync(id);
            if (country is null) return NotFound($"Country with id {id} not found.");
            return Ok(country);
        }
        [HttpGet("{name}", Name = "GetCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CountryDto>> GetCountryByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest($"Invalid country with name {name}.");
            var country = await countryService.GetCountryByNameAsync(name);
            if (country is null) return NotFound($"Country with name {name} not found.");
            return Ok(country);
        }

        [HttpGet("ExistsById/{id:int}", Name = "ExistsCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ExistsCountryByIdAsync(int id)
        {
            if (id <= 0) return BadRequest($"Invalid country with id {id}.");
            var exists = await countryService.ExistsCountryByIdAsync(id);
            if(!exists) return NotFound($"Country with id {id} not found.");
            return Ok(exists);
        }

        [HttpGet("ExistsByName/{name}", Name = "ExistsCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ExistsCountryByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest($"Invalid country with name {name}.");
            var exists = await countryService.ExistsCountryByNameAsync(name);
            if (!exists) return NotFound($"Country with name {name} not found.");
            return Ok(exists);
        }


    }
}
