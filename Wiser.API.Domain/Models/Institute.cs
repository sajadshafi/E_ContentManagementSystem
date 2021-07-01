using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class Institute : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string LogoPath { get; set; }
        public string Address { get; set; }
        public string PrincipalSign { get; set; }
    }
}
