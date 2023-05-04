using System.Collections.Generic;
using Template.Backend.Model.Common;

namespace Template.Backend.Api.Models
{
    public class CompanyDto : CompanyBase
    {
        public int ID { get; set; }
        public virtual ICollection<EmployeeDto> Employees { get; set; }
    }
}