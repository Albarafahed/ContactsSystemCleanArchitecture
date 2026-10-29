using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace ContactsSystemInfrastructure.Repositories
{
    public sealed class ContactRepository(ContactsDbContext context) : IContactRepository
    {
        public async Task<ContactDto?> GetByIdAsync(int id)
        {
            var e = await context.Contacts.AsNoTracking().SingleOrDefaultAsync(x => x.ContactId == id);
            return e is null ? null : Map(e);
        }
        public async Task<List<ContactDto>> GetAllAsync()
        {
            var sw = Stopwatch.StartNew();

            var result = await context.Contacts
                .AsNoTracking()
                .Select(e => new ContactDto
                {
                    ContactId = e.ContactId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    DateOfBirth = e.DateOfBirth,
                    CountryId = e.CountryId,
                    ImagePath = e.ImagePath ?? string.Empty
                })
                .ToListAsync();

            sw.Stop();

            Console.WriteLine(
                $"EF Core query execution: {sw.ElapsedMilliseconds} ms");

            return result;
        }
        public async Task<ContactDto> AddAsync(ContactDto dto)
        {
            var e = new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                DateOfBirth = dto.DateOfBirth,
                CountryId = dto.CountryId,
                ImagePath = NullIfEmpty(dto.ImagePath)
            };
            await context.Contacts.AddAsync(e);
            await context.SaveChangesAsync();
            dto.ContactId = e.ContactId;
            return dto;
        }
        public async Task<bool> UpdateAsync(ContactDto dto)
        {
            var e = await context.Contacts.FindAsync(dto.ContactId);
            if (e is null) return false;
            e.FirstName = dto.FirstName; e.LastName = dto.LastName; e.Email = dto.Email; e.Phone = dto.Phone;
            e.Address = dto.Address; e.DateOfBirth = dto.DateOfBirth; e.CountryId = dto.CountryId; e.ImagePath = NullIfEmpty(dto.ImagePath);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var e = await context.Contacts.FindAsync(id);
            if (e is null) return false;
            context.Contacts.Remove(e);
            return await context.SaveChangesAsync() > 0;
        }
        public Task<bool> ExistsAsync(int id) => context.Contacts.AnyAsync(e => e.ContactId == id);
        private static string? NullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;
        private static ContactDto Map(Contact e) => new()
        {
            ContactId = e.ContactId,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            Address = e.Address,
            DateOfBirth = e.DateOfBirth,
            CountryId = e.CountryId,
            ImagePath = e.ImagePath ?? string.Empty
        };
    }
}
