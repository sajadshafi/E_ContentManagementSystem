using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class SystemUser : IdentityUser
    {
        public string Name { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Department Department { get; set; }
        public List<EContent> EContents { get; set; }
    }

    public class SystemRole : IdentityRole
    {
        public bool IsDeleted { get; set; } = false;
    }

    public class SystemUserRole : IdentityUserRole<string>
    {
        public bool IsDeleted { get; set; } = false;
    }
}
