namespace Template.Backend.Api.Models
{
    public class SearchEmployeeDto : EmployeeDto
    {
        public DateTime? startBirthDate { set; get; }
        public DateTime? endBirthDate { set; get; }
    }
}