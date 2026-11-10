using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using ContactsSystemInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Repositories;

public sealed class ContactRepository(ContactsDbContext context) : IContactRepository
{
    public async Task<Contact?> GetByIdAsync(int id) =>
        await context.Contacts.AsNoTracking().Include(c => c.Country)
            .SingleOrDefaultAsync(c => c.ContactId == id);

    public async Task<Contact?> GetByIdForUpdateAsync(int id) =>
        await context.Contacts.Include(c => c.Country)
            .SingleOrDefaultAsync(c => c.ContactId == id);

    public async Task<List<Contact>> GetAllAsync() =>
        await context.Contacts.AsNoTracking().Include(c => c.Country)
            .OrderBy(c => c.ContactId).ToListAsync();

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

    public async Task<bool> DeleteAsync(int id) =>
        await context.Contacts.Where(c => c.ContactId == id).ExecuteDeleteAsync() > 0;

    public async Task<bool> ExistsAsync(int id) =>
        await context.Contacts.AnyAsync(c => c.ContactId == id);

    public async Task<bool> EmailExistsAsync(string email, int? exceptContactId = null)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return await context.Contacts.AnyAsync(contact =>
            contact.Email.ToLower() == normalizedEmail &&
            (!exceptContactId.HasValue || contact.ContactId != exceptContactId.Value));
    }
}
