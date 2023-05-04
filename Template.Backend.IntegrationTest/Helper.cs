using Template.Backend.Data;
using Template.Backend.Data.Repositories;
using Template.Backend.Model.Entities;
using System.Data.Entity;


namespace Template.Backend.IntegrationTest
{
    /// <summary>
    /// Helper class
    /// </summary>
    public class Helper
    {
        public const string UserName = "UnitTest";

        public static void InitializeLocalDatabase(IDbFactory dbFactory)
        {
            IDbContext context = dbFactory.Init();
            if (context.Database.Exists())
            {
                // set the database to SINGLE_USER so it can be dropped
                string cmd = "ALTER DATABASE [" + context.Database.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmd);
                context.Database.Delete();
            }
            context.Database.Initialize(true);
        }

        public static Company CompanyFactory(string name)
        {
            return new Company
            {
                Name = name,
                CreationDate = new System.DateTime(2020, 01, 01)
            };
        }

        public static Employee EmployeeFactory(string name, int companyID, int? departmentID = null)
        {
            return new Employee
            {
                Name = name,
                Address = "TTEST",
                BirthDate = new System.DateTime(2001, 01, 01),
                CompanyID = companyID,
                DepartmentID = departmentID,
                Phone = null
            };
        }

        public static Department DepartmentFactory(string name)
        {
            return new Department
            {
                Name = name
            };
        }
    }
}