using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Infrastructure.Domain.Products
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product.HasKey(p => p.Id);
            product.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new ProductId(x));

            product.Property(p => p.Name)
                .HasConversion(x => x.Value, x => new ProductName(x));

            product.Property(p => p.Description)
                .HasConversion(x => x.Value, x => new Description(x));

            product.HasOne(p => p.Category);
            
            product.Ignore(p => p.ProductOffers);
            
            product.OwnsMany<ProductOffer>("_productOffers", offer =>
            {
                offer.HasKey(o => o.Id);
                offer.Property(o => o.Id)
                    .HasConversion(x => x.Value, x => new ProductOfferId(x));

                offer.WithOwner().HasForeignKey(o => o.ProductId);
                offer.Property(o => o.ProductId)
                    .HasConversion(x => x.Value, x => new ProductId(x));

                offer.Property(o => o.Price)
                    .HasConversion(x => x.Value, x => new MoneyValue(x));

                offer.Property(o => o.Quantity)
                    .HasConversion(x => x.Value, x => new Quantity(x));
            });
        }
    }
}
