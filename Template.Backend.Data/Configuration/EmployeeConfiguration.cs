using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Backend.Model.Entities;

namespace Template.Backend.Data.Configuration
{

    /// <summary>
    ///  EmployeeConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            // Properties
            builder.HasIndex(a => a.Name)
           .IsUnique();

            builder.Property(a => a.Name).IsRequired();

            builder.Property(a => a.BirthDate).IsRequired();
            builder.Property(a => a.Address).IsRequired();
            builder.Property(a => a.CompanyID).IsRequired();

            builder.Property(a => a.DepartmentID).IsRequired(false);
            builder.Property(a => a.Phone).IsRequired(false);
        }
    }
}
