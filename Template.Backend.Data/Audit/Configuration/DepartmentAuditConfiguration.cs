using Template.Backend.Model.Audit.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Template.Backend.Data.Audit.Configuration
{

    /// <summary>
    ///  DepartmentAuditConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class DepartmentAuditConfiguration : EntityTypeConfiguration<DepartmentAudit>
    {
        public DepartmentAuditConfiguration()
        {
            ToTable("DepartmentAudit").HasKey(a => a.DepartmentAuditID);
        }
    }
}
