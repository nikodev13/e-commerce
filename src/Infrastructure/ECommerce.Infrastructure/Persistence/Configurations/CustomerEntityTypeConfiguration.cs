using ECommerce.Domain.Addresses.ValueObjects;
using ECommerce.Domain.Customers;
using ECommerce.Domain.Customers.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new CustomerId(x));

        builder.Property(x => x.FirstName)
            .HasConversion(x => x.Value, x => new FirstName(x));
        builder.Property(x => x.LastName)
            .HasConversion(x => x.Value, x => new LastName(x));
        
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x));
        builder.Property(x => x.PhoneNumber)
            .HasConversion(x => x.Value, x => new PhoneNumber(x));

        builder.OwnsMany(x => x.Addresses, addressesBuilder =>
        {
            addressesBuilder.ToTable("Addresses");
            addressesBuilder.HasKey(x => x.Id);
            addressesBuilder.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new AddressId(x));

            addressesBuilder.WithOwner().HasForeignKey(x => x.CustomerId);
            
            addressesBuilder.Property(x => x.Street)
                .HasConversion(x => x.Value, x => new Street(x));
            addressesBuilder.Property(x => x.PostalCode)
                .HasConversion(x => x.Value, x => new PostalCode(x));
            addressesBuilder.Property(x => x.City)
                .HasConversion(x => x.Value, x => new City(x));
        });

    }
}