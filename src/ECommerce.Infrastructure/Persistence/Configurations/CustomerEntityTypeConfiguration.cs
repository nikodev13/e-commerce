using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<CustomerAccount>
{
    public void Configure(EntityTypeBuilder<CustomerAccount> customerAccounts)
    {
        customerAccounts.ToTable("CustomerAccounts");
        customerAccounts.HasKey(x => x.Id);
        customerAccounts.Property(x => x.Id);

        customerAccounts.Property(x => x.FirstName).IsRequired();
        customerAccounts.Property(x => x.LastName).IsRequired();
        customerAccounts.Property(x => x.Email)
            .IsRequired();
        customerAccounts.Property(x => x.PhoneNumber)
            .IsRequired();
    }
}