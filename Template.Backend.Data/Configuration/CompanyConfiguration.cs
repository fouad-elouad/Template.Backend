using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Backend.Model.Entities;

namespace Template.Backend.Data.Configuration
{

    /// <summary>
    ///  CompanyConfiguration class
    ///  Allows database configuration to be performed for an entity type in a model
    /// </summary>
    class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasIndex(a => a.Name)
            .IsUnique();

            builder.Property(a => a.CreationDate).IsRequired();

            // one2Many relationships
            builder.HasMany<Employee>(a => a.Employees)
            .WithOne(b => b.Company)
            .HasForeignKey(b => b.CompanyID).IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
