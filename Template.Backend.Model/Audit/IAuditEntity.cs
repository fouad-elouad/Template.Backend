using System;

namespace Template.Backend.Model.Audit
{
    public interface IAuditEntity
    {
        int RowVersion { get; set; }
        DateTime? CreatedDate { get; set; }
        int ID { get; set; }
        AuditOperations AuditOperation { get; set; }
        string LoggedUserName { get; set; }
        DateTime? CreatedOn{ get; set; }
    }

    public enum AuditOperations { INSERT = 1, UPDATE, DELETE }
}
