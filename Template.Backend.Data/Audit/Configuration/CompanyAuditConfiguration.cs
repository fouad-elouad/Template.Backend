using Template.Backend.Model.Audit.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Template.Backend.Data.Audit.Configuration
{

    /// <summary>
    ///  CompanyAuditConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class CompanyAuditConfiguration : EntityTypeConfiguration<CompanyAudit>
    {
        public CompanyAuditConfiguration()
        {
            ToTable("CompanyAudit").HasKey(a => a.CompanyAuditID);
        }
    }
}
