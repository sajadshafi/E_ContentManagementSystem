using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class SubjectAllotment : BaseModel
    {
        public Guid CourseId { get; set; }
        public int SemesterNo { get; set; }
        public string AcademicSession { get; set; }
        public string CoreSubjects { get; set; }
        public string GESubjects { get; set; }
        public string SkillSubjects { get; set; }
        public bool Active { get; set; }
        public Course Course { get; set; }
    }
}
