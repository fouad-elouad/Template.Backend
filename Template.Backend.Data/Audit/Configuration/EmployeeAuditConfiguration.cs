using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Backend.Model.Audit.Entities;

namespace Template.Backend.Data.Audit.Configuration
{

    /// <summary>
    ///  EmployeeAuditConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class EmployeeAuditConfiguration : IEntityTypeConfiguration<EmployeeAudit>
    {
        public void Configure(EntityTypeBuilder<EmployeeAudit> builder)
        {
            builder.ToTable("EmployeeAudit").HasKey(a => a.EmployeeAuditID);
        }
    }
}
