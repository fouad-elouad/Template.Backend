using Template.Backend.Model.Common;
using Template.Backend.Model.Enums;

namespace Template.Backend.Model.Audit.Entities
{
    public class DepartmentAudit : DepartmentBase, IAuditEntity
    {
        public int DepartmentAuditID { get; set; }
        public int RowVersion { get; set; }
        public int ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public AuditOperations AuditOperation { get; set; }
        public string? LoggedUserName { get; set; }
    }
}
