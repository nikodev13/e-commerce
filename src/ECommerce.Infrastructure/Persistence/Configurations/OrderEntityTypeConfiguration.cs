using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>,
    IEntityTypeConfiguration<OrderLine>,
    IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Order> orders)
    {
        orders.ToTable("Orders");
        orders.HasKey(x => x.Id);
        orders.Property(x => x.Id).ValueGeneratedNever();
        orders.OwnsOne(x => x.DeliveryAddress);
        orders.HasOne(x => x.Payment).WithOne();
    }

    public void Configure(EntityTypeBuilder<OrderLine> orderLines)
    {
        orderLines.ToTable("OrderLines");
        orderLines.HasKey(x => new { x.OrderId, x.ProductId });
        orderLines.Property(x => x.Quantity);
    }

    public void Configure(EntityTypeBuilder<Payment> orders)
    {
        orders.HasKey(x => x.Id);
        orders.Property(x => x.Value)
            .HasPrecision(19, 4);
    }
}