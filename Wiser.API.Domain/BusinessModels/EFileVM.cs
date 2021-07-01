using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class EFileVM
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string FilePath { get; set; }
        public Guid EContentId { get; set; }
    }
}
