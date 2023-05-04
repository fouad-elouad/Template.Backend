using System;
using System.Collections.Generic;
using Template.Backend.Model.Common;

namespace Template.Backend.Model.Entities
{
    public class Department : DepartmentBase, IEntity
    {
        public int ID { get; set; }
        public int RowVersion { get; set; }
        public DateTime? CreatedOn { get; set; }

        // One2many relation
        public virtual ICollection<Employee> Employees { get; set; }
    }
}