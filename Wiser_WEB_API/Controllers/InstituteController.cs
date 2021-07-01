using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities.BusinessModels;

namespace Wiser_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituteController : ControllerBase
    {
        private readonly IInstituteService service;

        public InstituteController(IInstituteService instituteService)
        {
            this.service = instituteService;
        }

        [Authorize]
        [HttpGet, Route("/api/v1/get-institute")]
        public async Task<IActionResult> GetInstitute()
        {
            var response = await this.service.GetInstitute();
            return Ok(response);
        }

        //[Authorize]
        [HttpGet, Route("/api/v1/get-departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var response = await this.service.GetDepartments();
            return Ok(response);
        }

        //[Authorize]
        [HttpGet, Route("/api/v1/get-course-categories")]
        public async Task<IActionResult> GetCourseCategories()
        {
            var response = await this.service.GetCourseCategories();
            return Ok(response);
        }

        //[Authorize]
        [HttpGet, Route("/api/v1/get-courses")]
        public async Task<IActionResult> GetCourses(Guid? courseCategoryId = null)
        {
            var response = await this.service.GetCourses(courseCategoryId);
            return Ok(response);
        }

        //[Authorize]
        [HttpGet, Route("/api/v1/get-subject-categories")]
        public async Task<IActionResult> GetSubjectCategories()
        {
            var response = await this.service.GetSubjectCategories();
            return Ok(response);
        }

        //[Authorize]
        [HttpGet, Route("/api/v1/get-subject-by-category")]
        public async Task<IActionResult> GetSubjectsByCategory(Guid? CategoryId = null, Guid? DepartmentId = null, int semesterNo=0)
        {
            var response = await this.service.GetSubjectsByCategory(CategoryId, DepartmentId, semesterNo);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/save-institute")]
        public async Task<IActionResult> SaveInstitute(InstituteVM institute)
        {
            var response = await this.service.SaveInstitute(institute);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/save-department")]
        public async Task<IActionResult> SaveDepartment(DepartmentVM department)
        {
            var response = await this.service.SaveDepartment(department);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/save-course")]
        public async Task<IActionResult> SaveCourse(CourseVM course)
        {
            var response = await this.service.SaveCourse(course);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/save-course-category")]
        public async Task<IActionResult> SaveCourseCategory(CourseCategoryVM course)
        {
            var response = await this.service.SaveCourseCategory(course);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/save-subject-category")]
        public async Task<IActionResult> SaveSubjectCategory(SubjectCategoryVM categoryVM)
        {
            var response = await this.service.SaveSubjectCategory(categoryVM);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin+","+SystemRoles.Teacher)]
        [HttpPost, Route("/api/v1/save-subject")]
        public async Task<IActionResult> SaveSubject(SubjectVM subject)
        {
            var response = await this.service.SaveSubject(subject);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/delete-department")]
        public async Task<IActionResult> DeleteDepartment(Guid Id)
        {
            var response = await this.service.DeleteDepartment(Id);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/delete-course")]
        public async Task<IActionResult> DeleteCourse(Guid Id)
        {
            var response = await this.service.DeleteCourse(Id);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/delete-subject")]
        public async Task<IActionResult> DeleteSubject(Guid Id)
        {
            var response = await this.service.DeleteSubject(Id);
            return Ok(response);
        }
    }
}
