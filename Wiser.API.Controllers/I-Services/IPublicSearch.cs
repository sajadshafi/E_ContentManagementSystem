using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.Filters;
using Wiser.API.Entities.BusinessModels;
using Wiser_WEB_API.Filters;

namespace Wiser.API.BL.I_Services
{
    public interface IPublicSearch
    {
        public Task<PagedResponse<List<EContentVM>>> LoadContent(int unit=1, PaginationFilter filter = null, SearchContentFilter searchContentFilter = null);
    }
}
