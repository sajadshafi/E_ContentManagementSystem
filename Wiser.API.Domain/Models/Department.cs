using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class Department : BaseModel
    {
        public string DepartmentName { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<SystemUser> SystemUsers { get; set; }
    }
}
