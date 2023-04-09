using ECommerce.ApplicationCore.Entities;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>,
    IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<Order> orders)
    {
        orders.ToTable("Orders");
        orders.HasKey(x => x.Id);
        orders.Property(x => x.Id).ValueGeneratedNever();
        orders.OwnsOne(x => x.Delivery, delivery =>
        {
            delivery.Property(x => x.Street).IsRequired();
            delivery.Property(x => x.PostalCode).IsRequired();
            delivery.Property(x => x.City).IsRequired();
            delivery.Property(x => x.Operator).IsRequired();
            delivery.Property(x => x.TrackingNumber).IsRequired(false);
        });
        orders.HasOne(x => x.Payment).WithOne();
    }

    public void Configure(EntityTypeBuilder<OrderLine> orderLines)
    {
        orderLines.ToTable("OrderLines");
        orderLines.HasKey(x => new { x.OrderId, x.ProductId });
        orderLines.Property(x => x.Quantity);
        orderLines.Property(x => x.UnitPrice).HasPrecision(19, 4);
    }
}