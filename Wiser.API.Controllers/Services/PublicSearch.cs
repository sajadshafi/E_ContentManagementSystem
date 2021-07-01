using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.Filters;
using Wiser.API.BL.Helpers;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities;
using Wiser.API.Entities.BusinessModels;
using Wiser.API.Entities.Models;
using Wiser_WEB_API.Filters;

namespace Wiser.API.BL.Services
{
    public class PublicSearch : IPublicSearch
    {
        private readonly WiserContext wiserContext;
        private readonly IMapper mapper;
        public PublicSearch(WiserContext wiserContext, IMapper mapper)
        {
            this.wiserContext = wiserContext;
            this.mapper = mapper;
        }
        public async Task<PagedResponse<List<EContentVM>>> LoadContent(int unit = 1, PaginationFilter filter=null, SearchContentFilter searchContentFilter = null)
        {
            List<EContent> data = null;
            bool emptySearchFilter = true;
            if (searchContentFilter != null)
            {
                if (searchContentFilter.CourseId.HasValue)
                {
                    data = await this.wiserContext.EContents.Include(x => x.EFiles)
                                                            .Include(x => x.Subject)
                                                            .Include(x => x.SystemUser)
                                                            .Include(x => x.Course)
                                                            .Where(x => x.Unit == unit && x.CourseId == searchContentFilter.CourseId)
                                                            .ToListAsync();
                    emptySearchFilter = false;
                }
                if (searchContentFilter.SubjectId.HasValue)
                {
                    if (data != null)
                    {
                        data = data.Where(x => x.SubjectId == searchContentFilter.SubjectId).ToList();
                    }
                    else
                    {
                        data = await this.wiserContext.EContents.Include(x => x.EFiles)
                                                            .Include(x => x.Subject)
                                                            .Include(x => x.SystemUser)
                                                            .Include(x => x.Course)
                                                            .Where(x => x.Unit == unit && x.SubjectId == searchContentFilter.SubjectId)
                                                            .ToListAsync();
                    }
                    emptySearchFilter = false;
                }
            }
            else
            {
                data = await this.wiserContext.EContents.Include(x => x.EFiles)
                                                                .Include(x => x.Subject)
                                                                .Include(x => x.SystemUser)
                                                                .Include(x => x.Course)
                                                                .Where(x => x.Unit == unit)
                                                                .ToListAsync();

            }
            if (data != null && data.Any())
            {
                if (filter != null)
                {
                    data = data.OrderByDescending(x => x.CreatedDate)
                                .Skip((filter.PageNumber - 1) * filter.PageSize)
                                .Take(filter.PageSize)
                                .ToList();
                }
                var dataVm = mapper.Map<List<EContentVM>>(data);
                var response=PaginationHelper.CreatePagedReponse<List<EContentVM>>(dataVm, filter, dataVm.Count);            
                response.Message = "Data found";
                response.Count = dataVm.Count;
                return response;

            }
            else
            {
                var response = new PagedResponse<List<EContentVM>>(null,filter.PageNumber, filter.PageSize);
                response.Message = emptySearchFilter?"Please use filter to search the content":"No record is available";
                response.Count = 0;
                return response;

            }
        }
    }
}
