using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class Subject : BaseModel
    {
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public int SemesterNo { get; set; }
        public int NumberOfSeats { get; set; }
        public Guid SubjectCategoryId { get; set; }
        public Guid DepartmentId { get; set; }
        public SubjectCategory SubjectCategory { get; set; }
        public Department Department { get; set; }
        public List<EContent> EContents { get; set; }
    }
}
