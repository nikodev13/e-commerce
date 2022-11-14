using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.Products.Categories.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Domain.Products;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(x => x.Value, x => new CategoryId(x));
        
        builder.Property(c => c.Name)
            .HasConversion(x => x.Value, x => new CategoryName(x));
    }
}