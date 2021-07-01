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
    public class InstituteService : IInstituteService
    {
        private readonly WiserContext wiserContext;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Guid CurrentUserId;
        private readonly List<string> CurrentUserRoles;

        public InstituteService(WiserContext wiserContext, IMapper mapper, IHttpContextAccessor _httpContextAccessor)
        {
            this.wiserContext = wiserContext;
            this.mapper = mapper;
            this.httpContextAccessor = _httpContextAccessor;
            var global = new Global(httpContextAccessor);
            this.CurrentUserId = global.GetCurrentUserId();
            this.CurrentUserRoles = global.GetCurrentUserRoles();
        }
        public async Task<Response<bool>> DeleteCourse(Guid Id)
        {
            var course = await wiserContext.Courses.FirstOrDefaultAsync(x => x.Id == Id);
            if (course != null)
            {
                wiserContext.Courses.Remove(course);
                await wiserContext.SaveChangesAsync();
                return new Response<bool>()
                {
                    Message = "Department deleted successfully",
                    Success = true,
                    Data = true
                };
            }
            return new Response<bool>()
            {
                Data = false,
                Success = true,
                Message = "Department not found"
            };
        }
        public async Task<Response<bool>> DeleteDepartment(Guid Id)
        {
            var department = await wiserContext.Departments.FirstOrDefaultAsync(x => x.Id == Id);
            if (department != null)
            {
                wiserContext.Departments.Remove(department);
                await wiserContext.SaveChangesAsync();
                return new Response<bool>()
                {
                    Message = "Department deleted successfully",
                    Success = true,
                    Data = true
                };
            }
            return new Response<bool>()
            {
                Data = false,
                Success = true,
                Message = "Department not found"
            };
        }
        public async Task<Response<bool>> DeleteSubject(Guid Id)
        {
            var subject = await wiserContext.Subjects.FirstOrDefaultAsync(x => x.Id == Id);
            if (subject != null)
            {
                wiserContext.Subjects.Remove(subject); /// isDeleted ; true
                await wiserContext.SaveChangesAsync();
                return new Response<bool>()
                {
                    Message = "Subject deleted successfully",
                    Success = true,
                    Data = true
                };
            }
            return new Response<bool>()
            {
                Data = false,
                Success = true,
                Message = "Subject not found"
            };
        }
        public async Task<Response<List<CourseCategoryVM>>> GetCourseCategories()
        {
            var data = await wiserContext.CourseCategories.ToListAsync();
            if (data.Any())
            {
                var dataVm = mapper.Map<List<CourseCategoryVM>>(data);
                return new Response<List<CourseCategoryVM>>()
                {
                    Success = true,
                    Count = dataVm.Count,
                    Data = dataVm,
                    Message = "Course category list found"
                };
            }
            return new Response<List<CourseCategoryVM>>()
            {
                Success = true,
                Count = 0,
                Data = null,
                Message = "Course category list not found"
            };
        }
        public Task<Response<CourseCategoryVM>> GetCourseCategoryById(Guid Id)
        {
            throw new NotImplementedException();
        }
        public Task<Response<CourseVM>> GetCourseId(Guid Id)
        {
            throw new NotImplementedException();
        }
        public async Task<Response<List<CourseVM>>> GetCourses(Guid? courseCategoryId)
        {
            List<Course> data = null;
            if (!courseCategoryId.HasValue)
                data = await wiserContext.Courses.Include(x => x.CourseCategory).ToListAsync();
            else
                data = await wiserContext.Courses.Where(x => x.CourseCategoryId == courseCategoryId).Include(x => x.CourseCategory).ToListAsync();
            if (data != null && data.Any())
            {
                var dataVm = mapper.Map<List<CourseVM>>(data);
                return new Response<List<CourseVM>>()
                {
                    Success = true,
                    Count = dataVm.Count,
                    Data = dataVm,
                    Message = "Courses list found"
                };
            }
            return new Response<List<CourseVM>>()
            {
                Success = true,
                Count = 0,
                Data = null,
                Message = "Courses list not found"
            };
        }
        public Task<Response<DepartmentVM>> GetDepartmentById(Guid Id)
        {
            throw new NotImplementedException();
        }
        public async Task<Response<List<DepartmentVM>>> GetDepartments()
        {
            var data = new List<Department>();
            if (this.CurrentUserRoles != null && this.CurrentUserRoles.Any())
            {
                if (this.CurrentUserRoles.Contains(SystemRoles.Admin) || this.CurrentUserRoles.Contains(SystemRoles.SuperAdmin))
                    data = await wiserContext.Departments.ToListAsync();
                else
                {
                    if (this.CurrentUserRoles.Contains(SystemRoles.Teacher))
                    {
                        var user = await wiserContext.Users.FirstOrDefaultAsync(x => x.Id == CurrentUserId.ToString());
                        data = await wiserContext.Departments.Where(x => x.Id == user.DepartmentId).ToListAsync();
                    }
                }
            }
            else
            {
                data = await wiserContext.Departments.ToListAsync();
            }
            if (data.Any())
            {
                var dataVm = mapper.Map<List<DepartmentVM>>(data);
                return new Response<List<DepartmentVM>>()
                {
                    Success = true,
                    Count = dataVm.Count,
                    Data = dataVm,
                    Message = "Department list found"
                };
            }
            return new Response<List<DepartmentVM>>()
            {
                Success = true,
                Count = 0,
                Data = null,
                Message = "Department list not found"
            };
        }
        public async Task<Response<InstituteVM>> GetInstitute()
        {
            var data = await wiserContext.Institutes.FirstOrDefaultAsync();
            if (data != null)
            {
                var dataVm = mapper.Map<InstituteVM>(data);
                return new Response<InstituteVM>()
                {
                    Success = true,
                    Count = 1,
                    Data = dataVm,
                    Message = "Institute Details found"
                };
            }
            return new Response<InstituteVM>()
            {
                Success = true,
                Data = null,
                Message = "Institute Details not found"
            };
        }
        public async Task<Response<List<SubjectCategoryVM>>> GetSubjectCategories()
        {
            var data = await wiserContext.SubjectCategories.ToListAsync();
            if (data.Any())
            {
                var dataVm = mapper.Map<List<SubjectCategoryVM>>(data);
                return new Response<List<SubjectCategoryVM>>()
                {
                    Success = true,
                    Count = dataVm.Count,
                    Data = dataVm,
                    Message = "Subject category list found"
                };
            }
            return new Response<List<SubjectCategoryVM>>()
            {
                Success = true,
                Count = 0,
                Data = null,
                Message = "Subject category list not found"
            };
        }
        public async Task<Response<List<SubjectVM>>> GetSubjectsByCategory(Guid? CategoryId, Guid? DepartmentId, int semesterNo)
        {
            if (this.CurrentUserRoles!=null && this.CurrentUserRoles.Any() && this.CurrentUserRoles.Contains(SystemRoles.Teacher))
            {
                var user = await wiserContext.Users.FirstOrDefaultAsync(x => x.Id == CurrentUserId.ToString());
                DepartmentId = user.DepartmentId;
            }
            List<Subject> data = await wiserContext.Subjects.Include(x => x.Department).Include(x => x.SubjectCategory).ToListAsync();

            if (CategoryId.HasValue)
                data = data.Where(x => x.SubjectCategoryId == CategoryId).ToList();

            if (DepartmentId.HasValue)
                data = data.Where(x => x.DepartmentId == DepartmentId).ToList();

            if (semesterNo != 0)
                data = data.Where(x => x.SemesterNo == semesterNo).ToList();

            if (data != null && data.Any())
            {
                var dataVm = mapper.Map<List<SubjectVM>>(data);
                return new Response<List<SubjectVM>>()
                {
                    Success = true,
                    Count = dataVm.Count,
                    Data = dataVm,
                    Message = "Subject list found"
                };
            }
            return new Response<List<SubjectVM>>()
            {
                Success = true,
                Count = 0,
                Data = null,
                Message = "Subject list not found"
            };
        }
        public async Task<Response<CourseVM>> SaveCourse(CourseVM course)
        {
            var response = new Response<CourseVM>() { Success = true };
            if (course.Id.Equals(Constants.DEFAULT_GUID))
            {
                var map = mapper.Map<Course>(course);
                map.CourseCategory = null;
                map.CreatedDate = DateTime.Now;
                map.CreatedBy = CurrentUserId;
                await wiserContext.Courses.AddAsync(map);
                await wiserContext.SaveChangesAsync();
                course.Id = map.Id;
                response.Data = course;
                response.Message = "Course added successfully";
            }
            else
            {
                var storedData = await wiserContext.Courses.FirstOrDefaultAsync(x => x.Id == course.Id);
                if (storedData != null)
                {
                    storedData.ModifiedDate = DateTime.Now;
                    storedData.ModifiedBy = CurrentUserId;
                    mapper.Map(course, storedData);
                    storedData.CourseCategory = null;
                    await wiserContext.SaveChangesAsync();
                    course.Id = storedData.Id;
                    response.Data = course;
                    response.Message = "Course updated successfully";
                }
                else
                {
                    response.Message = "Course updation failed";
                    response.Errors.Add("You are trying to update the invalid Course");
                    response.Success = false;
                }
            }
            return response;
        }
        public async Task<Response<CourseCategoryVM>> SaveCourseCategory(CourseCategoryVM courseCategory)
        {
            var response = new Response<CourseCategoryVM>() { Success = true };
            if (courseCategory.Id.Equals(Constants.DEFAULT_GUID))
            {
                var Map = mapper.Map<CourseCategory>(courseCategory);
                Map.CreatedBy = CurrentUserId;
                Map.CreatedDate = DateTime.Now;
                await wiserContext.CourseCategories.AddAsync(Map);
                await wiserContext.SaveChangesAsync();
                courseCategory.Id = Map.Id;
                response.Data = courseCategory;
                response.Message = "Course Category added successfully";
            }
            else
            {
                var storedData = await wiserContext.CourseCategories.FirstOrDefaultAsync(x => x.Id == courseCategory.Id);
                if (storedData != null)
                {
                    storedData.ModifiedDate = DateTime.Now;
                    storedData.ModifiedBy = CurrentUserId;
                    mapper.Map(courseCategory, storedData);
                    await wiserContext.SaveChangesAsync();
                    courseCategory.Id = storedData.Id;
                    response.Data = courseCategory;
                    response.Message = "Course Category updated successfully";
                }
                else
                {
                    response.Message = "Department updation failed";
                    response.Errors.Add("You are trying to update the invalid course category");
                    response.Success = false;
                }
            }
            return response;
        }
        public async Task<Response<DepartmentVM>> SaveDepartment(DepartmentVM department)
        {
            var response = new Response<DepartmentVM>() { Success = true };
            if (department.Id.Equals(Constants.DEFAULT_GUID))
            {
                var departmentMap = mapper.Map<Department>(department);
                departmentMap.CreatedDate = DateTime.Now;
                departmentMap.CreatedBy = CurrentUserId;
                await wiserContext.Departments.AddAsync(departmentMap);
                await wiserContext.SaveChangesAsync();
                department.Id = departmentMap.Id;
                response.Data = department;
                response.Message = "Department added successfully";
            }
            else
            {
                var storedDepartment = await wiserContext.Departments.FirstOrDefaultAsync(x => x.Id == department.Id);
                if (storedDepartment != null)
                {
                    storedDepartment.ModifiedDate = DateTime.Now;
                    storedDepartment.ModifiedBy = CurrentUserId;
                    mapper.Map(department, storedDepartment);
                    await wiserContext.SaveChangesAsync();
                    department.Id = storedDepartment.Id;
                    response.Data = department;
                    response.Message = "Department updated successfully";
                }
                else
                {
                    response.Message = "Department updation failed";
                    response.Errors.Add("You are trying to update the invalid department");
                    response.Success = false;
                }
            }
            return response;
        }
        public async Task<Response<InstituteVM>> SaveInstitute(InstituteVM instituteVm)
        {
            var response = new Response<InstituteVM>() { Success = true };
            bool isCreateOperation = instituteVm.Id.Equals(Constants.DEFAULT_GUID);
            var errors = await ValidateInstiute(instituteVm, isCreateOperation);
            if (errors.Any())
            {
                response.Message = "Saving of Institute failed";
                response.Errors.AddRange(errors);
                response.Success = false;
            }
            else
            {

                if (isCreateOperation)
                {
                    var institute = mapper.Map<Institute>(instituteVm);
                    await wiserContext.Institutes.AddAsync(institute);
                    await wiserContext.SaveChangesAsync();
                    instituteVm.Id = institute.Id;
                    response.Data = instituteVm;
                    response.Message = "Institute added successfully";

                }
                else
                {
                    var storedInstitute = await wiserContext.Institutes.FirstOrDefaultAsync(x => x.Id == instituteVm.Id);
                    if (storedInstitute != null)
                    {
                        mapper.Map(instituteVm, storedInstitute);
                        await wiserContext.SaveChangesAsync();
                        instituteVm.Id = storedInstitute.Id;
                        response.Data = instituteVm;
                        response.Message = "Institute updated successfully";
                    }
                    else
                    {
                        response.Message = "Institute updation failed";
                        response.Errors.Add("You are trying to update the invalid institute details");
                        response.Success = false;
                    }
                }
            }

            return response;
        }
        public async Task<Response<SubjectVM>> SaveSubject(SubjectVM subject)
        {
            var response = new Response<SubjectVM>() { Success = true };
            if (subject.Id.Equals(Constants.DEFAULT_GUID))
            {
                var Map = mapper.Map<Subject>(subject);
                Map.CreatedDate = DateTime.Now;
                Map.CreatedBy = CurrentUserId;
                Map.Department = null;
                Map.SubjectCategory = null;
                await wiserContext.Subjects.AddAsync(Map);
                await wiserContext.SaveChangesAsync();
                subject.Id = Map.Id;
                response.Data = subject;
                response.Message = "Subject added successfully";
            }
            else
            {
                var storedData = await wiserContext.Subjects.FirstOrDefaultAsync(x => x.Id == subject.Id);
                if (storedData != null)
                {
                    storedData.ModifiedDate = DateTime.Now;
                    storedData.ModifiedBy = CurrentUserId;
                    mapper.Map(subject, storedData);
                    storedData.Department = null;
                    storedData.SubjectCategory = null;
                    await wiserContext.SaveChangesAsync();
                    subject.Id = storedData.Id;
                    response.Data = subject;
                    response.Message = "Subject updated successfully";
                }
                else
                {
                    response.Message = "Subject updation failed";
                    response.Errors.Add("You are trying to update the invalid Subject");
                    response.Success = false;
                }
            }
            return response;
        }
        public async Task<Response<SubjectCategoryVM>> SaveSubjectCategory(SubjectCategoryVM category)
        {
            var response = new Response<SubjectCategoryVM>() { Success = true };
            if (category.Id.Equals(Constants.DEFAULT_GUID))
            {
                var Map = mapper.Map<SubjectCategory>(category);
                Map.CreatedDate = DateTime.Now;
                Map.CreatedBy = CurrentUserId;
                await wiserContext.SubjectCategories.AddAsync(Map);
                await wiserContext.SaveChangesAsync();
                category.Id = Map.Id;
                response.Data = category;
                response.Message = "Subject Category added successfully";
            }
            else
            {
                var storedData = await wiserContext.SubjectCategories.FirstOrDefaultAsync(x => x.Id == category.Id);
                if (storedData != null)
                {
                    storedData.ModifiedDate = DateTime.Now;
                    storedData.ModifiedBy = CurrentUserId;
                    mapper.Map(category, storedData);
                    await wiserContext.SaveChangesAsync();
                    category.Id = storedData.Id;
                    response.Data = category;
                    response.Message = "Subject Category updated successfully";
                }
                else
                {
                    response.Message = "Subject category updation failed";
                    response.Errors.Add("You are trying to update the invalid subject category");
                    response.Success = false;
                }
            }
            return response;
        }

        #region Validations
        private async Task<List<string>> ValidateInstiute(InstituteVM instituteVM, bool isCreateOperation)
        {
            var email = instituteVM.Email?.Trim()?.ToLower();
            List<string> errors = new List<string>();

            if (!isCreateOperation)
            {
                var isEmailExisting = await wiserContext.Institutes.AnyAsync(x => x.Email == email && x.Id != instituteVM.Id);
                if (isEmailExisting)
                    errors.Add("Email is already existing");
            }
            else
            {
                var isEmailExisting = await wiserContext.Institutes.AnyAsync(x => x.Email == email);
                if (isEmailExisting)
                    errors.Add("Email is already existing");
            }
            return errors;
        }
        #endregion
    }
}
