using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product.ToTable("Products");
            
            product.HasKey(p => p.Id);
            product.Property(x => x.Id);

            product.Property(p => p.Name).IsRequired();

            product.Property(p => p.Description);

            product.HasOne(p => p.Category);

            product.Property(p => p.Price);

            product.Property(p => p.InStockQuantity);
        }
        
    }
}