using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.Models
{
    public interface IBaseModel
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        Guid CreatedBy { get; set; }
        Guid? ModifiedBy { get; set; }
    }

}
