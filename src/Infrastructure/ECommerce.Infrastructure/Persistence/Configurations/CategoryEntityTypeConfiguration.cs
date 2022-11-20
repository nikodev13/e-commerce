using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.ProductsContext.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects;
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
        
        builder.Property(c => c.Name)
            .HasConversion(x => x.Value, x => new CategoryName(x));
        
    }
}