
namespace Template.Backend.Model.Common
{
    public abstract class EmployeeBase
    {
        public string? Name { set; get; }
        public string? Address { set; get; }
        public string? Phone { set; get; }
        public DateTime? BirthDate { get; set; }
        public int? CompanyID { get; set; }
        public int? DepartmentID { get; set; }

    }
}