using Template.Backend.Model.Entities;

namespace Template.Backend.IntegrationTest
{
    public static class Helper
    {

        public static IEnumerable<Company> CompanyFactory(int count, string namePrefix = "Name")
        {
            IList<Company> list = new List<Company>(count);
            for (int i = 1; i <= count; i++)
            {
                list.Add(
                    new Company
                    {
                        CreationDate = DateTime.Now,
                        ID = 0,
                        Name = namePrefix + i,
                        RowVersion = 0
                    });
            }
            return list;
        }

        public static IEnumerable<Employee> EmployeeFactory(int count, string namePrefix = "Name")
        {
            IList<Employee> list = new List<Employee>(count);
            for (int i = 1; i <= count; i++)
            {
                list.Add(
                    new Employee
                    {
                        CompanyID = 1,
                        BirthDate = DateTime.Now,
                        Address = "Address",
                        ID = 0,
                        Name = namePrefix + i,
                        RowVersion = 0
                    });
            }
            return list;
        }

        public static IEnumerable<Department> DepartmentFactory(int count, string namePrefix = "Name")
        {
            IList<Department> list = new List<Department>(count);
            for (int i = 1; i <= count; i++)
            {
                list.Add(
                    new Department
                    {
                        ID = 0,
                        Name = namePrefix + i,
                        RowVersion = 0
                    });
            }
            return list;
        }
    }
}