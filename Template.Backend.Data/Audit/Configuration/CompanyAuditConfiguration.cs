using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Backend.Model.Audit.Entities;

namespace Template.Backend.Data.Audit.Configuration
{

    /// <summary>
    ///  CompanyAuditConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class CompanyAuditConfiguration : IEntityTypeConfiguration<CompanyAudit>
    {
        public void Configure(EntityTypeBuilder<CompanyAudit> builder)
        {
            builder.ToTable("CompanyAudit").HasKey(a => a.CompanyAuditID);
        }
    }
}
