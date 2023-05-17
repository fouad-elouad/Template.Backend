using Template.Backend.Model.Common;

namespace Template.Backend.Model.Entities
{
    public class Company : CompanyBase, IEntity
    {
        public int ID { get; set; }

        public int RowVersion { get; set; }
        public DateTime? CreatedOn { get; set; }

        // One2many relation
        public IList<Employee> Employees { get; private set; } = new List<Employee>();
    }
}