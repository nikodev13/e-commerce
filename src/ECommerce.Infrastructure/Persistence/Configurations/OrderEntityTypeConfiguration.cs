using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>, IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<Order> orders)
    {
        orders.ToTable("Orders");
        orders.HasKey(x => x.Id);
        orders.Property(x => x.Id).ValueGeneratedNever();
        orders.OwnsOne(x => x.DeliveryAddress);
    }

    public void Configure(EntityTypeBuilder<OrderLine> orderLines)
    {
        orderLines.ToTable("OrderLines");
        orderLines.HasKey(x => new { x.OrderId, x.ProductId });
        orderLines.Property(x => x.Quantity);
    }
}