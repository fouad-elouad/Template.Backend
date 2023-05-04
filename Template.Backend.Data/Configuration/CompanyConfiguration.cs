using Template.Backend.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Template.Backend.Data.Configuration
{

    /// <summary>
    ///  CompanyConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            ToTable("Companies");

            Property(a => a.Name).IsRequired()
            .HasMaxLength(256)
            .HasColumnAnnotation(
            IndexAnnotation.AnnotationName,
            new IndexAnnotation(
            new IndexAttribute("UI_Name") { IsUnique = true }));

            Property(a => a.CreationDate).IsRequired();

            // one2Many relationships
            HasMany<Employee>(a => a.Employees)
            .WithRequired(b => b.Company)
            .HasForeignKey<int?>(b => b.CompanyID)
            .WillCascadeOnDelete(false);
        }
    }
}
