using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using ContactsSystemInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Repositories;

public sealed class ContactRepository(ContactsDbContext context) : IContactRepository
{
    public async Task<Contact?> GetByIdAsync(int id, string? ownerUserId) =>
        await context.Contacts.AsNoTracking().Include(c => c.Country)
            .SingleOrDefaultAsync(c => c.ContactId == id &&
                (ownerUserId == null || c.UserId == ownerUserId));

    public async Task<Contact?> GetByIdForUpdateAsync(int id, string? ownerUserId) =>
        await context.Contacts.Include(c => c.Country)
            .SingleOrDefaultAsync(c => c.ContactId == id &&
                (ownerUserId == null || c.UserId == ownerUserId));

    public async Task<List<Contact>> GetAllAsync(string? ownerUserId) =>
        await context.Contacts.AsNoTracking().Include(c => c.Country)
            .Where(c => ownerUserId == null || c.UserId == ownerUserId)
            .OrderBy(c => c.ContactId).ToListAsync();


    public async Task<(List<Contact> Items, int TotalCount)> GetPagedAsync(
        string? ownerUserId, string? search, int? countryId, int pageNumber, int pageSize)
    {
        IQueryable<Contact> query = context.Contacts.AsNoTracking().Include(contact => contact.Country);

        if (ownerUserId is not null)
            query = query.Where(contact => contact.UserId == ownerUserId);

        if (countryId.HasValue)
            query = query.Where(contact => contact.CountryId == countryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(contact =>
                contact.FirstName.Contains(term) ||
                contact.LastName.Contains(term) ||
                contact.Email.Contains(term) ||
                contact.Phone.Contains(term));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(contact => contact.ContactId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<bool> AddAsync(Contact contact)
    {
        await context.Contacts.AddAsync(contact);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> SaveChangesAsync()
    {
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string? ownerUserId) =>
        await context.Contacts
            .Where(c => c.ContactId == id && (ownerUserId == null || c.UserId == ownerUserId))
            .ExecuteDeleteAsync() > 0;

    public async Task<bool> ExistsAsync(int id, string? ownerUserId) =>
        await context.Contacts.AnyAsync(c => c.ContactId == id &&
            (ownerUserId == null || c.UserId == ownerUserId));

    public async Task<bool> EmailExistsAsync(string email, int? exceptContactId = null)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return await context.Contacts.AnyAsync(contact =>
            contact.Email.ToLower() == normalizedEmail &&
            (!exceptContactId.HasValue || contact.ContactId != exceptContactId.Value));
    }
}
