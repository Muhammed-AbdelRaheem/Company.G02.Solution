using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels.Departments;

namespace Company.G02.PL.Mapping.Departments
{
    public class DepartmentProfile:Profile
    {
        public DepartmentProfile() { 
        
        CreateMap<Department,DepartmentViewModel>().ReverseMap();
        }
    }
}
