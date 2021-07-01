using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Guid? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class UpdateUserVM : UserProfile
    {
        public string Id { get; set; }
    }
}
