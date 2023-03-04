using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orders)
    {
        orders.HasKey(x => x.Id);
        orders.OwnsOne(x => x.DeliveryAddress, addresses =>
        {
        });

        orders.OwnsMany(x => x.OrderLines, orderLines =>
        {
            orderLines.HasKey(x => x.OrderId);
        });
    }
}