using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public abstract class BaseModel : IBaseModel
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;  
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
