
using Template.Backend.Model.Enums;

namespace Template.Backend.Model.Audit
{
    public interface IAuditEntity
    {
        int RowVersion { get; set; }
        DateTime? CreatedDate { get; set; }
        int ID { get; set; }
        AuditOperations AuditOperation { get; set; }
        string? LoggedUserName { get; set; }
        DateTime? CreatedOn { get; set; }
    }

}
