using Microsoft.EntityFrameworkCore;
using ContactsSystemDomain.Entities;
public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();

    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ContactsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}