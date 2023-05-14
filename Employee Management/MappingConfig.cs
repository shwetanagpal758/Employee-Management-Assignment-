using AutoMapper;
using Employee_Management.Controllers.Models;
using Employee_Management.Controllers.Models.Dto;
using Employee_Management.Models;
using Employee_Management.Models.Dto;

namespace Employee_Management
{
    public class MappingConfig : Profile
    {  
        public MappingConfig() { 
        CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Designation, DesignationDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Employees, EmployeesDto>().ReverseMap();



        
        }



    }
}
