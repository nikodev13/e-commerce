namespace ECommerce.ApplicationCore.Entities;

public class Order
{
    public required long Id { get; init; }
    public required Guid CustomerId { get; init; }
    public Customer Customer { get; init; } = default!;
    public required Delivery Delivery { get; init; }
    public required Guid PaymentId { get; init; }
    public Payment Payment { get; init; } = default!;
    public required List<OrderLine> OrderLines { get; init; }
    public OrderStatus Status { get; set; } = OrderStatus.Placed;
    public DateTime PlacedAt { get; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
    public Guid? OperatedBy { get; set; }
}

public class OrderLine
{
    public required long OrderId { get; init; }
    public required long ProductId { get; init; }
    public Product Product { get; init; } = default!;
    public required uint Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
}

public enum DeliveryOperator
{
    DPD,
    InPost
}

public class Delivery
{
    public required DeliveryOperator Operator { get; set; }
    public string? TrackingNumber { get; set; }
    public required string Street { get; set; }
    public required string PostalCode { get; set; }
    public required string City { get; set; }
}

public enum OrderStatus
{
    Placed = 0,
    WaitingForPayment = 1,
    Paid = 2,
    InRealization = 3,
    Sent = 4,
    Delivered = 5,
    Canceled = 6,
}
