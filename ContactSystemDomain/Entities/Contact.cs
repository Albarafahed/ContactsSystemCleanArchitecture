using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ContactsSystemDomain.Entities;

public sealed class Contact
{
    public int ContactId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public int CountryId { get; private set; }
    public string? ImagePath { get; private set; }
    public Country Country { get; private set; } = null!;

    private Contact() { }

    public Contact(
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        DateTime dateOfBirth,
        int countryId,
        string? imagePath = null)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
        SetPhone(phone);
        SetAddress(address);
        SetDateOfBirth(dateOfBirth);
        SetCountryId(countryId);
        SetImagePath(imagePath);
    }

    public void SetFirstName(string firstName)
    {
        FirstName = ValidateRequiredText(firstName, 50, nameof(firstName), "First name");
    }

    public void SetLastName(string lastName)
    {
        LastName = ValidateRequiredText(lastName, 50, nameof(lastName), "Last name");
    }

    public void SetEmail(string email)
    {
        var value = ValidateRequiredText(email, 50, nameof(email), "Email");
        try
        {
            var parsed = new MailAddress(value);
            if (!string.Equals(parsed.Address, value, StringComparison.OrdinalIgnoreCase))
                throw new FormatException();
        }
        catch (FormatException)
        {
            throw new ArgumentException("A valid email address is required.", nameof(email));
        }

        Email = value;
    }

    public void SetPhone(string phone)
    {
        var value = ValidateRequiredText(phone, 20, nameof(phone), "Phone");
        if (value.Length < 7 ||
            !Regex.IsMatch(value, @"^[0-9+() .-]+$", RegexOptions.CultureInvariant))
        {
            throw new ArgumentException("Phone must contain 7 to 20 valid phone characters.", nameof(phone));
        }

        Phone = value;
    }

    public void SetAddress(string address)
    {
        Address = ValidateRequiredText(address, 200, nameof(address), "Address");
    }

    public void SetDateOfBirth(DateTime dateOfBirth)
    {
        if (dateOfBirth == default)
            throw new ArgumentException("Date of birth is required.", nameof(dateOfBirth));

        if (dateOfBirth.Date > DateTime.Today)
            throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

        DateOfBirth = dateOfBirth;
    }

    public void SetCountryId(int countryId)
    {
        if (countryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(countryId), "Country ID must be greater than zero.");

        CountryId = countryId;
    }

    public void SetImagePath(string? imagePath)
    {
        if (imagePath?.Trim().Length > 500)
            throw new ArgumentException("Image path cannot exceed 500 characters.", nameof(imagePath));

        ImagePath = string.IsNullOrWhiteSpace(imagePath) ? null : imagePath.Trim();
    }

    private static string ValidateRequiredText(string value, int maxLength, string parameterName, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} is required.", parameterName);

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
            throw new ArgumentException($"{fieldName} cannot exceed {maxLength} characters.", parameterName);

        return normalized;
    }
}
