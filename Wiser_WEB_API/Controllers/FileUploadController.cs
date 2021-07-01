using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities.BusinessModels;

namespace Wiser_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        [HttpPost, Route("/api/v1/upload-file")]
        public async Task<IActionResult> UploadFiles([FromForm] FileModelVM file)
        {
            var response = await this.fileUploadService.UploadFiles(file);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet, Route("/api/v1/download-file")]
        public async Task<IActionResult> FileDownload(string FileName)
        {
            Stream stream = await fileUploadService.FileDownload(FileName);
            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.
            return File(stream, "application/octet-stream",FileName); // returns a FileStreamResult
        }
    }
}
