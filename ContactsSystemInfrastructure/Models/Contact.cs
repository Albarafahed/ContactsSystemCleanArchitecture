using System;
using System.Collections.Generic;

namespace ContactsSystemInfrastructure.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public int CountryId { get; set; }

    public string? ImagePath { get; set; }

    public virtual Country Country { get; set; } = null!;
}
