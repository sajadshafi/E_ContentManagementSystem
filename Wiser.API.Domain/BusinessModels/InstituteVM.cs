using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class InstituteVM
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneNo { get; set; }
        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                this._Email = value?.Trim()?.ToLower();
            }
        }
        public string Fax { get; set; }
        public string LogoPath { get; set; }
        public string Address { get; set; }
        public string PrincipalSign { get; set; }
    }
}
