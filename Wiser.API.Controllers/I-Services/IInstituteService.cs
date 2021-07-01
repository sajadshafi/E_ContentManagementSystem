using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.Entities.BusinessModels;

namespace Wiser.API.BL.I_Services
{
    public interface IInstituteService
    {
        Task<Response<InstituteVM>> GetInstitute();
        Task<Response<InstituteVM>> SaveInstitute(InstituteVM institute);
        
        Task<Response<List<DepartmentVM>>> GetDepartments();
        Task<Response<DepartmentVM>> GetDepartmentById(Guid Id);
        Task<Response<DepartmentVM>> SaveDepartment(DepartmentVM department);
        Task<Response<bool>> DeleteDepartment(Guid Id);

        Task<Response<CourseCategoryVM>> SaveCourseCategory(CourseCategoryVM courseCategory);
        Task<Response<List<CourseCategoryVM>>> GetCourseCategories();
        Task<Response<CourseCategoryVM>> GetCourseCategoryById(Guid Id);
        Task<Response<List<CourseVM>>> GetCourses(Guid? courseCategoryId);
        Task<Response<CourseVM>> GetCourseId(Guid Id);
        Task<Response<CourseVM>> SaveCourse(CourseVM courseCategory);
        Task<Response<bool>> DeleteCourse(Guid Id);

        Task<Response<SubjectCategoryVM>> SaveSubjectCategory(SubjectCategoryVM courseCategory);
        Task<Response<List<SubjectCategoryVM>>> GetSubjectCategories();
        
        Task<Response<List<SubjectVM>>> GetSubjectsByCategory(Guid? CategoryId, Guid? DepartmentId, int semesterNo);
        Task<Response<SubjectVM>> SaveSubject(SubjectVM subject);
        Task<Response<bool>> DeleteSubject(Guid Id);

    }
}
