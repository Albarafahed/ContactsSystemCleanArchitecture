using ContactsSystemDomain.Entities;

namespace ContactsSystemDomain.Entities;

public class Country
{
    public int CountryId { get; private set; }

    public string? CountryName { get; private set; }

    public string? Code { get; private set; }

    public string? PhoneCode { get; private set; }

    public  ICollection<Contact> Contacts { get; private set; }
        = new List<Contact>();

    private Country() { }
}