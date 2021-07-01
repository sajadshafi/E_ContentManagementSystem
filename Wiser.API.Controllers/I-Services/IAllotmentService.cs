using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.Entities.BusinessModels;

namespace Wiser.API.BL.I_Services
{
    public interface IAllotmentService
    {
        Task<Response<SubjectAllotmentVM>> AllotSubjectsAsync(SubjectAllotmentVM institute);
        Task<Response<List<SubjectAllotmentVM>>> GetAllotedSubjectsAsync();
    }
}
