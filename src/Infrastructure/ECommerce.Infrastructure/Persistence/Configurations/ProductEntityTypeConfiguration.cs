using ECommerce.Domain.Management.Entities;
using ECommerce.Domain.Management.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product.ToTable("Products", SchemaNames.Catalog);
            
            product.HasKey(p => p.Id);
            product.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new ProductId(x));

            product.Property(p => p.Name)
                .HasConversion(x => x.Value, x => new ProductName(x));

            product.Property(p => p.Description)
                .HasConversion(x => x.Value, x => new Description(x));

            product.HasOne(p => p.Category);

            product.Property(p => p.Price)
                .HasConversion(x => x.Value, x => new MoneyValue(x));
            
            product.Property(p => p.InStockQuantity)
                .HasConversion(x => x.Value, x => new Quantity(x));
        }
        
    }
}