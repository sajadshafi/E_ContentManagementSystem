using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class CourseCategory : BaseModel
    {
        public string Name { get; set; }
        public int NumberOfSemesters { get; set; }
        public List<Course> Courses { get; set; }

    }
}
