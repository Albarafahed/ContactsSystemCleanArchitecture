using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Repositories;

public sealed class ContactRepository(ContactsDbContext context)
    : IContactRepository
{
    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await context.Contacts
            .Include(c => c.Country)
            .SingleOrDefaultAsync(c => c.ContactId == id);
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await context.Contacts
            .AsNoTracking()
            .Include(c => c.Country)
            .OrderBy(c => c.ContactId)
            .ToListAsync();
    }

    public async Task AddAsync(Contact contact)
    {
        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contact contact)
    {
        context.Contacts.Update(contact);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Contact contact)
    {
        context.Contacts.Remove(contact);
        await context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(int id)
    {
        return context.Contacts.AnyAsync(c => c.ContactId == id);
    }
}