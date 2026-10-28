using System;
using System.Collections.Generic;

namespace ContactsSystemInfrastructure.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public string? Code { get; set; }

    public string? PhoneCode { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
