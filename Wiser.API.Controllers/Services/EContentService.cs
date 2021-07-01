using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.Helpers;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities;
using Wiser.API.Entities.BusinessModels;
using Wiser.API.Entities.Models;

namespace Wiser.API.BL.Services
{
    public class EContentService : IEContentService
    {
        private readonly WiserContext wiserContext;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Guid CurrentUserId;
        private readonly List<string> CurrentUserRoles;
        public EContentService(WiserContext wiserContext, IMapper mapper, IHttpContextAccessor _httpContextAccessor)
        {
            this.wiserContext = wiserContext;
            this.mapper = mapper;
            httpContextAccessor = _httpContextAccessor;
            var global = new Global(httpContextAccessor);
            this.CurrentUserId = global.GetCurrentUserId();
            this.CurrentUserRoles = global.GetCurrentUserRoles();
        }

        public async Task<Response<bool>> DeleteContent(Guid Id)
        {
            var content =await wiserContext.EContents.FirstOrDefaultAsync(x => x.Id == Id);
            if (content != null && content.UserId==CurrentUserId.ToString())
            {
               var files = await wiserContext.EFiles.Where(x => x.EContentId == content.Id).ToListAsync();
                foreach (var file in files)
                    wiserContext.EFiles.Remove(file);
                wiserContext.EContents.Remove(content);

                await wiserContext.SaveChangesAsync();
                return new Response<bool>()
                {
                    Message = "Content Deleted Successfully",
                    Success = true
                };
            }
            else
            {
                return new Response<bool>()
                {
                    Message = "You are trying to delete the invalid content",
                    Success = false
                };
            }
        }

        public async Task<Response<List<EContentVM>>> GetContent()
        {
            List<EContent> data = null;
            if (this.CurrentUserRoles != null && this.CurrentUserRoles.Any())
            {
                if (this.CurrentUserRoles.Contains(SystemRoles.Admin))
                    data = await this.wiserContext.EContents.Include(x => x.EFiles)
                                                            .Include(x => x.Subject)
                                                            .Include(x => x.SystemUser)
                                                            .Include(x => x.Course)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .ToListAsync();
                else if (this.CurrentUserRoles.Contains(SystemRoles.Teacher))
                {
                    data = await this.wiserContext.EContents.Include(x => x.EFiles)
                                                            .Include(x => x.Subject)
                                                            .Include(x => x.SystemUser)
                                                            .Include(x => x.Course)
                                                            .Where(x => x.UserId == CurrentUserId.ToString())
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .ToListAsync();
                }
            }
            else
            {
                data = await this.wiserContext.EContents.Include(x => x.EFiles)
                                                            .Include(x => x.Subject)
                                                            .Include(x => x.SystemUser)
                                                            .Include(x => x.Course)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .ToListAsync();
            }

            if (data != null && data.Any())
            {
                var dataVm = mapper.Map<List<EContentVM>>(data);
                return new Response<List<EContentVM>>()
                {
                    Count = dataVm.Count,
                    Data = dataVm,
                    Message = "E-content found"
                };
            }
            else
            {
                return new Response<List<EContentVM>>()
                {
                    Count = 0,
                    Data = null,
                    Message = "E-content not found"
                };
            }
        }
        public async Task<Response<EContentVM>> SaveContent(EContentVM eContent)
        {
            var response = new Response<EContentVM>() { Success = true };
            using (var dbTran = await wiserContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var files = eContent.eFileVMs;
                    if (eContent.Id.Equals(Constants.DEFAULT_GUID))
                    {
                        var newEContent = mapper.Map<EContent>(eContent);
                        newEContent.Subject = null;
                        newEContent.Course = null;
                        newEContent.EFiles = null;
                        newEContent.SystemUser = null;
                        newEContent.UserId = this.CurrentUserId.ToString();
                        newEContent.CreatedBy = this.CurrentUserId;
                        newEContent.CreatedDate = DateTime.Now;
                        await wiserContext.EContents.AddAsync(newEContent);
                        await wiserContext.SaveChangesAsync();
                        await StoreFiles(files, newEContent.Id);
                        response.Message = "Content added successfully";
                    }
                    else
                    {
                        var storedContent = await this.wiserContext.EContents.FirstOrDefaultAsync(x => x.Id == eContent.Id && x.UserId == CurrentUserId.ToString());
                        if (storedContent != null)
                        {
                            var createData = storedContent.CreatedDate;
                            mapper.Map(eContent, storedContent);
                            storedContent.EFiles = null;
                            storedContent.Course = null;
                            storedContent.Subject = null;
                            storedContent.SystemUser = null;
                            storedContent.CreatedDate = createData;
                            storedContent.ModifiedBy = this.CurrentUserId;
                            storedContent.ModifiedDate = DateTime.Now;
                            await wiserContext.SaveChangesAsync();
                            await StoreFiles(files, storedContent.Id);
                            response.Message = "Content updated successfully";
                        }
                        else
                        {
                            response.Message = "Content updated failed";
                            response.Errors.Add("You are trying to update the invalid content");
                            response.Success = false;
                        }
                    }
                }
                catch
                {
                    dbTran.Rollback();
                }
                await dbTran.CommitAsync();
            }
            return response;
        }

        private async Task StoreFiles(List<EFileVM> fileVMs, Guid EContentId)
        {

            foreach (var file in fileVMs)
            {
                if (file.Id.Equals(Constants.DEFAULT_GUID))
                {
                    if (file.Active)
                    {
                        var efile = new EFile()
                        {
                            Description = file.Description,
                            FilePath = file.FilePath,
                            CreatedBy = CurrentUserId,
                            EContentId = EContentId,
                        };
                        await wiserContext.EFiles.AddAsync(efile);
                    }
                }
                else
                {
                    var storedFile = await wiserContext.EFiles.FirstOrDefaultAsync(x => x.Id == file.Id);
                    if (storedFile != null)
                    {
                        storedFile.FilePath = file.FilePath;
                        storedFile.Description = file.Description;
                        storedFile.ModifiedBy = CurrentUserId;
                        storedFile.ModifiedDate = DateTime.Now;
                        storedFile.IsDeleted = !file.Active;
                    }

                }                
            }
            await wiserContext.SaveChangesAsync();
        }
    }
}
