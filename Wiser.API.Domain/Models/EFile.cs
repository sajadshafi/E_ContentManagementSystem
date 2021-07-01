using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public class EFile : BaseModel
    {
        public string Description { get; set; }
        public string FilePath { get; set; }
        public Guid EContentId { get; set; }
        public EContent EContent { get; set; }
    }
}
