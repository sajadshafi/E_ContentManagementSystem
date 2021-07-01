using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.Entities.BusinessModels;

namespace Wiser.API.BL.I_Services
{
    public interface IFileUploadService
    {
        Task<Response<EFileVM>> UploadFiles(FileModelVM file);
        Task<Stream> FileDownload(string FileName);
    }
}
