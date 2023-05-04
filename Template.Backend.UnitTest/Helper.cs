using Newtonsoft.Json;
using Template.Backend.Api.Models;
using System.Web.Http.Results;

namespace Template.Backend.UnitTest
{
    public class Helper
    {
        public const string UserName = "UnitTest";

        public static T DeserializeObject<T>(ResponseMessageResult responseResult) where T : class
        {
            string json = responseResult.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static CompanyDto CompanyDtoFactory(string name)
        {
            return new CompanyDto
            {
                Name = name,
                CreationDate = new System.DateTime(2020,01,01)
            };
        }

        public static EmployeeDto EmployeeDtoFactory(string name, int companyID, int? departmentID = null)
        {
            return new EmployeeDto
            {
                Name = name,
                Address = "TTEST",
                BirthDate = new System.DateTime(2001, 01, 01),
                CompanyID = companyID,
                DepartmentID = departmentID,
                Phone = null
            };
        }

        public static DepartmentDto DepartmentDtoFactory(string name)
        {
            return new DepartmentDto
            {
                Name = name
            };
        }
    }
}