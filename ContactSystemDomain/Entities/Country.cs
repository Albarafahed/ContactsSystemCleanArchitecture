using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSystemDomain.Entities
{
    public sealed class Country
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Code { get; set; }
        public string? PhoneCode { get; set; }
    }
}
