using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.Helpers;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities.BusinessModels;

namespace Wiser.API.BL.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IOptions<ApplicationSetting> applicationSetting;

        public FileUploadService(IOptions<ApplicationSetting> applicationSetting)
        {
            this.applicationSetting = applicationSetting;
        }

        public async Task<Stream> FileDownload(string FileName)
        {
            Stream stream = null;
            string folderPath = applicationSetting.Value.RootPath + Constants.E_FILE_PATH;
            
            if (File.Exists(folderPath + FileName))
            {
                await Task.Run(() =>
                {
                    stream = new FileStream(folderPath + FileName, FileMode.Open, FileAccess.Read);

                });


            }

            return stream;
        }

        public async Task<Response<EFileVM>> UploadFiles(FileModelVM file)
        {
            var response = new Response<EFileVM>() { Success = true };
            if (file == null)
            {
                response.Success = false;
                response.Message = "File upload failed";
                response.Errors.Add("No file found to upload");
            }
            else
            {
                EFileVM FileList = null;

                var File_Name = file.FormFile.FileName.Substring(0, file.FormFile.FileName.LastIndexOf(".")) + "_" + DateTime.Now.Ticks + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")).Trim();
                string folderPath = applicationSetting.Value.RootPath + Constants.E_FILE_PATH;
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string fullFilePath = folderPath + File_Name;
                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await file.FormFile.CopyToAsync(stream);
                }
                FileList = new EFileVM() { FilePath = File_Name, Description = file.Description, Active = true };

                if (FileList != null)
                {
                    response.Message = "File uploaded successfully";
                    response.Data = FileList;
                    response.Count = 1;
                }
                else
                {
                    response.Message = "File uploading failed";
                    response.Success = false;
                    response.Errors.Add("Some unknown error has occurred");
                }
            }
            return response;
        }
    }
}
