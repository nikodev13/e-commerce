﻿using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> addresses)
    {
        addresses.ToTable("AddressBooks");

        addresses.HasKey(x => new { x.Id, x.CustomerId });
        addresses.Property(x => x.Id).ValueGeneratedNever();
        addresses.Property(x => x.CustomerId).ValueGeneratedNever();
        
        addresses.Property(x => x.Street).HasMaxLength(100);
        addresses.Property(x => x.PostalCode).HasMaxLength(6);
        addresses.Property(x => x.City).HasMaxLength(50);
    }
}