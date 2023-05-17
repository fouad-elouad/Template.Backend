using Template.Backend.Api.Models;
using Template.Backend.Model.Entities;
using Template.Backend.Model;

namespace Template.Backend.UnitTest;

internal class Helper
{

    public static void IncrementRowVersion<Entity>(Entity entity) where Entity : IEntity
    {
        entity.RowVersion++;
    }

    public static IEnumerable<Company> CompanyFactory(int count)
    {
        IList<Company> companies = new List<Company>(count);
        for (int i = 1; i <= count; i++)
        {
            companies.Add(
                new Company
                {
                    CreatedOn = DateTime.Now,
                    CreationDate = DateTime.Now,
                    ID = i,
                    Name = "Name" + i,
                    RowVersion = 0
                });
        }
        return companies;
    }

    public static IEnumerable<CompanyDto> CompanyDtoFactory(int count)
    {
        IList<CompanyDto> companies = new List<CompanyDto>(count);
        for (int i = 1; i <= count; i++)
        {
            companies.Add(
                new CompanyDto
                {
                    CreationDate = DateTime.Now,
                    ID = i,
                    Name = "Name" + i,
                });
        }
        return companies;
    }

    public static IEnumerable<Employee> EmployeeFactory(int count)
    {
        IList<Employee> employees = new List<Employee>(count);
        for (int i = 1; i <= count; i++)
        {
            employees.Add(
                new Employee
                {
                    Address = "Address",
                    BirthDate = new DateTime(2020,10,10),
                    CompanyID = 1,
                    CreatedOn = DateTime.Now,
                    ID = i,
                    Name = "Name" + i,
                    RowVersion = 0
                });
        }
        return employees;
    }

    public static IEnumerable<EmployeeDto> EmployeeDtoFactory(int count)
    {
        IList<EmployeeDto> employees = new List<EmployeeDto>(count);
        for (int i = 1; i <= count; i++)
        {
            employees.Add(
                new EmployeeDto
                {
                    ID = i,
                    Name = "Name" + i,
                    Address = "Address",
                    BirthDate = new DateTime(2020, 10, 10),
                    CompanyID = 1,
                });
        }
        return employees;
    }

    public static IEnumerable<Department> DepartmentFactory(int count)
    {
        IList<Department> departments = new List<Department>(count);
        for (int i = 1; i <= count; i++)
        {
            departments.Add(
                new Department
                {
                    CreatedOn = DateTime.Now,
                    ID = i,
                    Name = "Name" + i,
                    RowVersion = 0
                });
        }
        return departments;
    }

    public static IEnumerable<DepartmentDto> DepartmentDtoFactory(int count)
    {
        IList<DepartmentDto> departments = new List<DepartmentDto>(count);
        for (int i = 1; i <= count; i++)
        {
            departments.Add(
                new DepartmentDto
                {
                    ID = i,
                    Name = "Name" + i,
                });
        }
        return departments;
    }
}
