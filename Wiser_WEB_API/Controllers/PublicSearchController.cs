using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiser.API.BL.Filters;
using Wiser.API.BL.I_Services;
using Wiser_WEB_API.Filters;

namespace Wiser_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicSearchController : ControllerBase
    {
        private readonly IPublicSearch publicSearch;

        public PublicSearchController(IPublicSearch publicSearch)
        {
            this.publicSearch = publicSearch;
        }
        [HttpGet, Route("/api/v1/search-content")]
        public async Task<IActionResult> LoadContent(int unit = 1, [FromQuery] PaginationFilter filter = null, [FromQuery] SearchContentFilter searchContentFilter=null)
        {
            var response = await this.publicSearch.LoadContent(unit, filter,searchContentFilter);
            return Ok(response);
        }
    }
}
