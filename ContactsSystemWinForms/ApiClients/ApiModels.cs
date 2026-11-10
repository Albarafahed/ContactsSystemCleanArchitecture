using System.Diagnostics;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace ContactsSystemWinForms;

public sealed class ContactDto
{
    public int ContactId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int CountryId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}
public sealed class CountryDto { public int CountryId { get; set; } public string CountryName { get; set; } = string.Empty; public string Code { get; set; } = string.Empty; public string PhoneCode { get; set; } = string.Empty; }

public sealed class ContactsApiClient(HttpClient http)
{
    
public async Task<List<ContactDto>> GetAllAsync() => await http.GetFromJsonAsync<List<ContactDto>>("api/contacts") ?? [];
public Task<ContactDto?> GetAsync(int id) => http.GetFromJsonAsync<ContactDto>($"api/contacts/{id}");
    public async Task<ContactDto?> SaveAsync(ContactDto value) 
    {
        using var response = value.ContactId == 0 ? await http.PostAsJsonAsync("api/contacts", value) : await http.PutAsJsonAsync($"api/contacts/{value.ContactId}", value); 
        response.EnsureSuccessStatusCode(); return await response.Content.ReadFromJsonAsync<ContactDto>(); }
    public async Task<bool> DeleteAsync(int id) { using var response = await http.DeleteAsync($"api/contacts/{id}"); if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return false; response.EnsureSuccessStatusCode(); return true; }
}

public sealed class CountriesApiClient(HttpClient http)
{
    public async Task<List<CountryDto>> GetAllAsync() => await http.GetFromJsonAsync<List<CountryDto>>("api/countries") ?? [];
}
