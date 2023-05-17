using AutoMapper;
using Template.Backend.Api.Models;
using Template.Backend.Model.Entities;

namespace Template.Backend.Api.Automapper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CompanyDtoMapping();
            EmployeeDtoMapping();
            DepartmentDtoMapping();
        }


        private IMappingExpression<CompanyDto, Company> CompanyDtoMapping()
        {
            return CreateMap<CompanyDto, Company>()
                .ForMember(m => m.Employees, map => map.Ignore())
                .ForMember(m => m.RowVersion, map => map.Ignore())
                .ForMember(m => m.CreatedOn, map => map.Ignore());
        }

        private IMappingExpression<EmployeeDto, Employee> EmployeeDtoMapping()
        {
            return CreateMap<EmployeeDto, Employee>()
                .ForMember(m => m.Company, map => map.Ignore())
                .ForMember(m => m.Department, map => map.Ignore())
                .ForMember(m => m.RowVersion, map => map.Ignore())
                .ForMember(m => m.CreatedOn, map => map.Ignore());
        }

        private IMappingExpression<DepartmentDto, Department> DepartmentDtoMapping()
        {
            return CreateMap<DepartmentDto, Department>()
                .ForMember(m => m.Employees, map => map.Ignore())
                .ForMember(m => m.RowVersion, map => map.Ignore())
                .ForMember(m => m.CreatedOn, map => map.Ignore());
        }
    }
}
