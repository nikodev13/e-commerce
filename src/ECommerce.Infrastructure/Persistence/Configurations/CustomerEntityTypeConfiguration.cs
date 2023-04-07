using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> customer)
    {
        customer.ToTable("Customers");
        customer.HasKey(x => x.Id);
        customer.Property(x => x.Id).ValueGeneratedNever();

        customer.Property(x => x.FirstName).IsRequired();
        customer.Property(x => x.LastName).IsRequired();
        customer.Property(x => x.Email).IsRequired();
        customer.Property(x => x.PhoneNumber).IsRequired();
        customer.Property(x => x.RegisteredAt).IsRequired();
    }
}