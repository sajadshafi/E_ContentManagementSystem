using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class SubjectAllotmentVM
    {
        public Guid CourseId { get; set; }
        public int SemesterNo { get; set; }
        public bool Active { get; set; } = true;
        public string AcademicSession { get; set; }
        public List<string> CoreSubjects { get; set; }
        public List<string> GESubjects { get; set; }
        public List<string> SkillSubjects { get; set; }
    }
}
