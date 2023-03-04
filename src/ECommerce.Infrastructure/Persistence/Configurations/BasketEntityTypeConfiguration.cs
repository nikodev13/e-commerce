using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Basket>, IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<Basket> baskets)
    {
        baskets.ToTable("Baskets");
        
        baskets.HasKey(x => x.CustomerId);
        baskets.Property(x => x.CustomerId);

        baskets.HasMany(x => x.Items).WithOne().HasPrincipalKey(x => x.CustomerId);
    }

    public void Configure(EntityTypeBuilder<BasketItem> basketItems)
    {
        basketItems.ToTable("BasketItems");

        basketItems.HasKey(x => x.Id);
        basketItems.Property(x => x.Id);

        basketItems.Property(x => x.Quantity).IsRequired();
        basketItems.Property(x => x.ProductId).IsRequired();
    }
}