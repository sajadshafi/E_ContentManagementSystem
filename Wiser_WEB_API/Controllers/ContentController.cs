using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities.BusinessModels;

namespace Wiser_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContentController : ControllerBase
    {
        private readonly IEContentService eContentService;

        public ContentController(IEContentService eContentService)
        {
            this.eContentService = eContentService;
        }

        [HttpPost, Route("/api/v1/save-content")]
        public async Task<IActionResult> SaveContent(EContentVM eContent)
        {
            var response = await this.eContentService.SaveContent(eContent);
            return Ok(response);
        }

        [HttpPost, Route("/api/v1/delete-content")]
        public async Task<IActionResult> DeleteContent(Guid Id)
        {
            var response = await this.eContentService.DeleteContent(Id);
            return Ok(response);
        }

        [HttpGet, Route("/api/v1/get-content")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContent()
        {
            var response = await this.eContentService.GetContent();
            return Ok(response);
        }
    }
}
