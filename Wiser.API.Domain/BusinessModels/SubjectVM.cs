using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class SubjectVM
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public int NumberOfSeats { get; set; }
        public int SemesterNo { get; set; }
        public Guid SubjectCategoryId { get; set; }
        public string SubjectCategoryName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
