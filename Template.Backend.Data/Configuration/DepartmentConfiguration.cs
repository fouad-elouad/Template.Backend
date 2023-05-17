using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Backend.Model.Entities;

namespace Template.Backend.Data.Configuration
{

    /// <summary>
    ///  DepartmentConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments").HasKey(a => a.ID);

            // Properties
            builder.HasIndex(a => a.Name)
            .IsUnique();

            // one2Many relationships
            builder.HasMany<Employee>(a => a.Employees)
            .WithOne(b => b.Department)
            .HasForeignKey(b => b.DepartmentID)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
