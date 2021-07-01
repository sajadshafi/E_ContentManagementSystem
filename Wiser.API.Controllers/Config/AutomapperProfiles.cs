using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wiser.API.Entities.BusinessModels;
using Wiser.API.Entities.Models;

namespace Wiser.API.BL.Config
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Institute, InstituteVM>().ReverseMap();
            CreateMap<Department, DepartmentVM>().ReverseMap();
            CreateMap<CourseCategory, CourseCategoryVM>().ReverseMap();
            CreateMap<Course, CourseVM>()
                .ForMember(dest => dest.CourseCategoryName, act => act.MapFrom(src => src.CourseCategory.Name))
                .ReverseMap();
            CreateMap<SubjectCategory, SubjectCategoryVM>().ReverseMap();
            CreateMap<Subject, SubjectVM>()
                .ForMember(dest => dest.SubjectCategoryName, act => act.MapFrom(src => src.SubjectCategory.Name))
                .ForMember(dest => dest.DepartmentName, act => act.MapFrom(src => src.Department.DepartmentName))
                .ReverseMap();
            CreateMap<SubjectAllotment, SubjectAllotmentVM>()
                .ForMember(dest => dest.CoreSubjects, act => act.MapFrom(src => ToListItems(src.CoreSubjects)))
                .ForMember(dest => dest.GESubjects, act => act.MapFrom(src => ToListItems(src.GESubjects)))
                .ForMember(dest => dest.SkillSubjects, act => act.MapFrom(src => ToListItems(src.SkillSubjects)));

            CreateMap<SubjectAllotmentVM, SubjectAllotment>()
                .ForMember(dest => dest.CoreSubjects, act => act.MapFrom(src => JoinItems(src.CoreSubjects)))
                .ForMember(dest => dest.GESubjects, act => act.MapFrom(src => JoinItems(src.GESubjects)))
                .ForMember(dest => dest.SkillSubjects, act => act.MapFrom(src => JoinItems(src.SkillSubjects)));

            CreateMap<EContent, EContentVM>()
                .ForMember(dest=>dest.SubjectNameCode,act=>act.MapFrom(src=>src.Subject.SubjectName+" ("+src.Subject.SubjectCode+")"))
                .ForMember(dest=>dest.SemesterNo,act=>act.MapFrom(src=>src.Subject.SemesterNo))
                .ForMember(dest=>dest.eFileVMs,act=>act.MapFrom(src=>src.EFiles))
                .ForMember(dest=>dest.CourseName,act=>act.MapFrom(src=>src.Course.CourseName))
                .ForMember(dest=>dest.NameOfUser,act=>act.MapFrom(src=>src.SystemUser.Name))
                .ForMember(dest => dest.UnitName, act => act.MapFrom(src => GetUnitName(src.Unit)))
                .ReverseMap();
            CreateMap<EFile, EFileVM>()
                .ForMember(dest=>dest.Active,act=>act.MapFrom(src=>!src.IsDeleted))
                .ReverseMap();
        }

        private static List<string> ToListItems(string items)
        {
            return string.IsNullOrWhiteSpace(items) ? new List<string>() : items.Split("|").ToList();
        }

        private static string JoinItems(List<string> items)
        {
            return items != null && items.Any() ? string.Join('|', items) : null;
        }

        private static string GetUnitName(int unit)
        {
            switch (unit)
            {
                case 0:
                    return "General";
                case 1:
                    return "Unit 1";
                case 2:
                    return "Unit 2";
                case 3:
                    return "Unit 3";
                case 4:
                    return "Unit 4";
                case 5:
                    return "Unit 5";
                default:
                    return "General";
            }
        }
    }
}
