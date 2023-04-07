using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product> , IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product.ToTable("Products");
            
            product.HasKey(p => p.Id);
            product.Property(x => x.Id).ValueGeneratedNever();

            product.Property(p => p.Name).IsRequired();

            product.Property(p => p.Description);

            product.HasOne(p => p.Category)
                .WithMany().HasForeignKey(x => x.CategoryId);

            product.Property(p => p.Price).HasPrecision(19, 4);

            product.Property(p => p.InStockQuantity);
        }

        public void Configure(EntityTypeBuilder<Category> category)
        {
            category.ToTable("Categories");
    
            category.HasKey(c => c.Id);
            category.Property(c => c.Id).ValueGeneratedNever();

            category.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}