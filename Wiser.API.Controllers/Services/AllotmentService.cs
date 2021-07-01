using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities;
using Wiser.API.Entities.BusinessModels;
using Wiser.API.Entities.Models;

namespace Wiser.API.BL.Services
{
    public class AllotmentService : IAllotmentService
    {
        private readonly WiserContext wiserContext;
        private readonly IMapper mapper;

        public AllotmentService(WiserContext wiserContext, IMapper mapper)
        {
            this.wiserContext = wiserContext;
            this.mapper = mapper;
        }
        public async Task<Response<SubjectAllotmentVM>> AllotSubjectsAsync(SubjectAllotmentVM allotment)
        {
            var response = new Response<SubjectAllotmentVM>() { Success = false };
            var isAnyAllotment=await wiserContext.SubjectAllotments.AnyAsync(x => x.CourseId == allotment.CourseId &&
                                                                          x.SemesterNo == allotment.SemesterNo && 
                                                                          x.AcademicSession.Trim().ToLower() == allotment.AcademicSession.Trim().ToLower() && 
                                                                          x.Active);
            if (!isAnyAllotment)
            {
                //allot
                var subjectAllotment = mapper.Map<SubjectAllotment>(allotment);
                subjectAllotment.Course = null;
                wiserContext.SubjectAllotments.Add(subjectAllotment);
                await wiserContext.SaveChangesAsync();
                response.Message = "Subjects alloted successfully";
                response.Success = true;
            }
            else
            {
                response.Errors = new List<string>() { "Allotment is already existing for this Semester of this Course" };
                response.Message = "Subject Allotment Failed";
            }
            return response;
        }

        public async Task<Response<List<SubjectAllotmentVM>>> GetAllotedSubjectsAsync()
        {
            var response = new Response<List<SubjectAllotmentVM>>() { Success = true };
            var data=await this.wiserContext.SubjectAllotments.ToListAsync();
            if (data.Any())
            {
                var dataToSent=mapper.Map<List<SubjectAllotmentVM>>(data);
                response.Data = dataToSent;
                response.Message = "Subject allotments found";
            }
            else
            {
                response.Message = "No Subject allotment found";
                response.Count = 0;
            }
            return response;
        }
    }
}
