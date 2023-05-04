using System;
using Template.Backend.Model.Common;

namespace Template.Backend.Model.Entities
{
    public class Employee : EmployeeBase, IEntity
    {
        public int ID { get; set; }
        public int RowVersion { get; set; }
        public DateTime? CreatedOn { get; set; }

        // ManyToOne
        public virtual Company Company { get; set; }
        public virtual Department Department { get; set; }

    }
}