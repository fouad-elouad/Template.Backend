using Template.Backend.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Template.Backend.Data.Configuration
{

    /// <summary>
    ///  EmployeeConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("Employees");

            // Properties
            Property(a => a.Name).IsRequired()
            .HasMaxLength(256)
            .HasColumnAnnotation(
            IndexAnnotation.AnnotationName,
            new IndexAnnotation(
            new IndexAttribute("UI_Name") { IsUnique = true }));

            Property(a => a.BirthDate).IsRequired();
            Property(a => a.Address).IsRequired();
            Property(a => a.CompanyID).IsRequired();

            Property(a => a.DepartmentID).IsOptional();
            Property(a => a.Phone).IsOptional();
        }
    }
}
