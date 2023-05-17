using Template.Backend.Model.Common;

namespace Template.Backend.Model.Entities
{
    public class Employee : EmployeeBase, IEntity
    {
        public int ID { get; set; }
        public int RowVersion { get; set; }
        public DateTime? CreatedOn { get; set; }

        // ManyToOne
        public Company? Company { get; set; }
        public Department? Department { get; set; }

    }
}