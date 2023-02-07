using ECommerce.Domain.Management.Entities;
using ECommerce.Domain.Shared.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", SchemaNames.Catalog);
    
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(x => x.Value, x => new CategoryId(x));

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}