using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class CourseCategoryVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSemesters { get; set; }
    }
}
