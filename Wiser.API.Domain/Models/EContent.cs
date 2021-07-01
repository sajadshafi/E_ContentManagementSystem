using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class EContent : BaseModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid SubjectId { get; set; }
        public string UserId { get; set; }
        public Guid? CourseId { get; set; }
        public int Unit { get; set; }
        public Course Course { get; set; }
        public Subject Subject { get; set; }
        public SystemUser SystemUser { get; set; }
        public List<EFile> EFiles { get; set; }
    }
}
