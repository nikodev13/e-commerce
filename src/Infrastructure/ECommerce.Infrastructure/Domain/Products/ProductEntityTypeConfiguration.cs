using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Infrastructure.Domain.Products
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new ProductId(x));   
        }
    }
}
