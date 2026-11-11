
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using ContactsSystemInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Repositories;

public sealed class ContactRepository (ContactsDbContext context) : IContactRepository
{
    

    public async Task<IReadOnlyList<Contact>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Contacts
            .AsNoTracking()
            .OrderBy(contact => contact.ContactId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Contact>> GetByOwnerIdAsync(
        int ownerUserId,
        CancellationToken cancellationToken = default)
    {
        return await context.Contacts
            .AsNoTracking()
            .Where(contact => contact.OwnerUserId == ownerUserId)
            .OrderBy(contact => contact.ContactId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Contact?> GetByIdAsync(
        int contactId,
        CancellationToken cancellationToken = default)
    {
        return await context.Contacts
            .FirstOrDefaultAsync(
                contact => contact.ContactId == contactId,
                cancellationToken);
    }

    public async Task AddAsync(
        Contact contact,
        CancellationToken cancellationToken = default)
    {
        await context.Contacts.AddAsync(
            contact,
            cancellationToken);
    }

    public void Delete(Contact contact)
    {
        context.Contacts.Remove(contact);
    }

    public Task<bool> ExitEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return context.Contacts.AnyAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
