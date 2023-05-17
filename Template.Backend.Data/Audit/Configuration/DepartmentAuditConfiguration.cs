using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Backend.Model.Audit.Entities;


namespace Template.Backend.Data.Audit.Configuration
{

    /// <summary>
    ///  DepartmentAuditConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class DepartmentAuditConfiguration : IEntityTypeConfiguration<DepartmentAudit>
    {
        public void Configure(EntityTypeBuilder<DepartmentAudit> builder)
        {
            builder.ToTable("DepartmentAudit").HasKey(a => a.DepartmentAuditID);
        }
    }
}
