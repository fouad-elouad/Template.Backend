using Template.Backend.Model.Audit.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Template.Backend.Data.Audit.Configuration
{

    /// <summary>
    ///  EmployeeAuditConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class EmployeeAuditConfiguration : EntityTypeConfiguration<EmployeeAudit>
    {
        public EmployeeAuditConfiguration()
        {
            ToTable("EmployeeAudit").HasKey(a => a.EmployeeAuditID);
        }
    }
}
