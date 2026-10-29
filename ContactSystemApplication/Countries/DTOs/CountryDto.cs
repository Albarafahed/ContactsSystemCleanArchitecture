using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemApplication.Countries.DTOs
{
    public sealed class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
    }
}
