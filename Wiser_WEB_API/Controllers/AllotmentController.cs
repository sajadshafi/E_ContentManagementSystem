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
    public class AllotmentController : ControllerBase
    {
        private readonly IAllotmentService service;

        public AllotmentController(IAllotmentService service)
        {
            this.service = service;
        }
        [HttpPost, Route("/api/v1/allot-subjects")]
        public async Task<IActionResult> AllotSubjectsAsync(SubjectAllotmentVM allotmentVM)
        {
            var response = await this.service.AllotSubjectsAsync(allotmentVM);
            return Ok(response);
        }

        [HttpGet, Route("/api/v1/get-alloted-subjects")]
        public async Task<IActionResult> GetAllotedSubjectsAsync()
        {
            var response = await this.service.GetAllotedSubjectsAsync();
            return Ok(response);
        }
    }
}
