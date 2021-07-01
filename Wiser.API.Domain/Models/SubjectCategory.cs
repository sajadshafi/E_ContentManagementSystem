using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class SubjectCategory : BaseModel
    {
        public string Name { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}
