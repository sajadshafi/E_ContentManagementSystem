using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class CourseVM
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public Guid CourseCategoryId { get; set; }
        public string CourseCategoryName { get; set; }
    }
}
