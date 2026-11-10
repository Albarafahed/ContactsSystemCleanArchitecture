using ContactsSystemDomain.Entities;
using ContactsSystemInfrastructure.Identity;
using ContactsSystemInfrastructure.Persistence;
using ContactsSystemInfrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ContactSystem.Tests.Infrastructure;

public sealed class ContactOwnershipRepositoryTests : IAsyncLifetime
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private ContactsDbContext _context = null!;
    private ContactRepository _repository = null!;

    public async Task InitializeAsync()
    {
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<ContactsDbContext>()
            .UseSqlite(_connection)
            .Options;
        _context = new ContactsDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _context.Users.AddRange(
            new ApplicationUser { Id = "user-1", UserName = "user1", NormalizedUserName = "USER1" },
            new ApplicationUser { Id = "user-2", UserName = "user2", NormalizedUserName = "USER2" });
        await _context.SaveChangesAsync();

        await _context.Database.ExecuteSqlRawAsync(
            "INSERT INTO Countries (CountryID, CountryName, Code, PhoneCode) VALUES (1, 'Yemen', 'YEM', '967')");

        var first = CreateContact("one@example.com", "user-1");
        var second = CreateContact("two@example.com", "user-2");
        await _context.Contacts.AddRangeAsync(first, second);
        await _context.SaveChangesAsync();

        _repository = new ContactRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_WithUserId_ReturnsOnlyThatUsersContacts()
    {
        var contacts = await _repository.GetAllAsync("user-1");

        var contact = Assert.Single(contacts);
        Assert.Equal("one@example.com", contact.Email);
        Assert.Equal("user-1", contact.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_WhenContactBelongsToAnotherUser_ReturnsNull()
    {
        var otherUsersContactId = await _context.Contacts
            .Where(contact => contact.UserId == "user-2")
            .Select(contact => contact.ContactId)
            .SingleAsync();

        var result = await _repository.GetByIdAsync(otherUsersContactId, "user-1");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdForUpdateAsync_WhenContactBelongsToAnotherUser_ReturnsNull()
    {
        var otherUsersContactId = await _context.Contacts
            .Where(contact => contact.UserId == "user-2")
            .Select(contact => contact.ContactId)
            .SingleAsync();

        var result = await _repository.GetByIdForUpdateAsync(otherUsersContactId, "user-1");

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_WhenContactBelongsToAnotherUser_ReturnsFalse()
    {
        var otherUsersContactId = await _context.Contacts
            .Where(contact => contact.UserId == "user-2")
            .Select(contact => contact.ContactId)
            .SingleAsync();

        Assert.False(await _repository.ExistsAsync(otherUsersContactId, "user-1"));
    }

    [Fact]
    public async Task DeleteAsync_WhenContactBelongsToAnotherUser_DoesNotDeleteIt()
    {
        var otherUsersContactId = await _context.Contacts
            .Where(contact => contact.UserId == "user-2")
            .Select(contact => contact.ContactId)
            .SingleAsync();

        var deleted = await _repository.DeleteAsync(otherUsersContactId, "user-1");

        Assert.False(deleted);
        Assert.True(await _context.Contacts.AnyAsync(contact => contact.ContactId == otherUsersContactId));
    }

    [Fact]
    public async Task GetAllAsync_WithNullOwnerScope_ReturnsAllContactsForAdmin()
    {
        var contacts = await _repository.GetAllAsync(null);

        Assert.Equal(2, contacts.Count);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    private static Contact CreateContact(string email, string ownerId)
    {
        var contact = new Contact(
            "Test", "User", email, "777123456", "Ibb",
            new DateTime(1998, 1, 1), 1);
        contact.AssignToUser(ownerId);
        return contact;
    }
}
