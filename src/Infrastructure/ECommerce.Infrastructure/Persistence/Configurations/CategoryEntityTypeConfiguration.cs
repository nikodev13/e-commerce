using ECommerce.Domain.Products;
using ECommerce.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
    
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(x => x.Value, x => new CategoryId(x));

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}