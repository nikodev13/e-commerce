﻿using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class WishlistProductsEntityTypeConfiguration : IEntityTypeConfiguration<WishlistProduct>
{
    public void Configure(EntityTypeBuilder<WishlistProduct> product)
    {
        product.ToTable("WishlistProducts");
        product.HasKey(x => new { x.CustomerId, x.ProductId });
        product.Property(x => x.CustomerId).ValueGeneratedNever();
        product.Property(x => x.ProductId).ValueGeneratedNever();
        product.HasOne(x => x.Product).WithOne();
    }
}