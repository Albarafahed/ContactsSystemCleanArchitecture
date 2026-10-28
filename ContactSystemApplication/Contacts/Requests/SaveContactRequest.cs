using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemApplication.Contacts.Requests
{
    public sealed class SaveContactRequest
    {
        [Required, StringLength(50)] public string FirstName { get; init; } = string.Empty;
        [Required, StringLength(50)] public string LastName { get; init; } = string.Empty;
        [Required, StringLength(50)] public string Email { get; init; } = string.Empty;
        [Required, StringLength(20)] public string Phone { get; init; } = string.Empty;
        [Required, StringLength(200)] public string Address { get; init; } = string.Empty;
        public DateTime DateOfBirth { get; init; }
        [Range(1, int.MaxValue)] public int CountryId { get; init; }
        [StringLength(500)] public string? ImagePath { get; init; }
    }
}
