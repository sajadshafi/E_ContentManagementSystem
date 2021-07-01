using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wiser.API.Entities.BusinessModels
{
    public class FileModelVM
    {
        public string Description { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
