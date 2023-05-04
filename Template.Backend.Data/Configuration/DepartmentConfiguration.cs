using Template.Backend.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Template.Backend.Data.Configuration
{

    /// <summary>
    ///  DepartmentConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            ToTable("Departments").HasKey(a => a.ID);

            // Properties
            Property(a => a.Name).IsRequired()
            .HasMaxLength(256)
            .HasColumnAnnotation(
            IndexAnnotation.AnnotationName,
            new IndexAnnotation(
            new IndexAttribute("UI_Name") { IsUnique = true }));

            // one2Many relationships
            HasMany<Employee>(a => a.Employees)
            .WithOptional(b => b.Department)
            .HasForeignKey<int?>(b => b.DepartmentID)
            .WillCascadeOnDelete(false);
        }
    }
}
