using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class EContentVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid SubjectId { get; set; }
        public Guid? CourseId { get; set; }
        public string NameOfUser { get; set; }
        public string CourseName { get; set; }
        public int SemesterNo { get; set; }
        public int Unit { get; set; }
        public string UnitName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SubjectNameCode { get; set; }
        public List<EFileVM> eFileVMs { get; set; }
    }
}
