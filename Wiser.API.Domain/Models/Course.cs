using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class Course : BaseModel
    {
        public string CourseName { get; set; }
        public Guid CourseCategoryId { get; set; }
        public CourseCategory CourseCategory { get; set; }
        public List<SubjectAllotment> SubjectAllotments { get; set; }
        public List<Course> Courses { get; set; }
    }
}
