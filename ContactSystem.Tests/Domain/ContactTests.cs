using ContactsSystemDomain.Entities;
using Xunit;

namespace ContactSystem.Tests.Domain;

public sealed class ContactTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesContact()
    {
        var contact = new Contact(
            "Albara", "Harissy", "albara@example.com", "+967 777 123 456",
            "Ibb", new DateTime(2000, 1, 1), 1);

        Assert.Equal("Albara", contact.FirstName);
        Assert.Equal("Harissy", contact.LastName);
        Assert.Equal("albara@example.com", contact.Email);
        Assert.Equal(1, contact.CountryId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void Constructor_WithInvalidEmail_Throws(string email)
    {
        Assert.Throws<ArgumentException>(() => new Contact(
            "A", "B", email, "777123456", "Address",
            new DateTime(2000, 1, 1), 1));
    }

    [Fact]
    public void Constructor_WithFutureDateOfBirth_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Contact(
            "A", "B", "a@example.com", "777123456", "Address",
            DateTime.Today.AddDays(1), 1));
    }

    [Fact]
    public void AssignToUser_WithBlankId_Throws()
    {
        var contact = CreateValidContact();
        Assert.Throws<ArgumentException>(() => contact.AssignToUser(" "));
    }

    [Fact]
    public void AssignToUser_WithValidId_AssignsOwner()
    {
        var contact = CreateValidContact();
        contact.AssignToUser("user-123");
        Assert.Equal("user-123", contact.UserId);
    }

    [Fact]
    public void SetPhone_WithInvalidCharacters_Throws()
    {
        var contact = CreateValidContact();
        Assert.Throws<ArgumentException>(() => contact.SetPhone("123ABC456"));
    }

    private static Contact CreateValidContact() => new(
        "A", "B", "a@example.com", "777123456", "Address",
        new DateTime(2000, 1, 1), 1);
}
