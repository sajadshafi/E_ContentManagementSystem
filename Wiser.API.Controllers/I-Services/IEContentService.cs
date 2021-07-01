using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.Entities.BusinessModels;

namespace Wiser.API.BL.I_Services
{
    public interface IEContentService
    {
        public Task<Response<EContentVM>> SaveContent(EContentVM eContent);
        public Task<Response<List<EContentVM>>> GetContent();
        public Task<Response<bool>> DeleteContent(Guid Id);
    }
}
