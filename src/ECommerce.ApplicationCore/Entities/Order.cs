﻿namespace ECommerce.ApplicationCore.Entities;

public class Order
{
    public required long Id { get; init; }
    public required Guid CustomerId { get; init; }
    public CustomerAccount Customer { get; init; } = default!;
    public required DeliveryAddress DeliveryAddress { get; init; }
    public required List<OrderLine> OrderLines { get; init; }
    public OrderStatus Status { get; set; } = OrderStatus.Placed;
}

public class OrderLine
{
    public required long OrderId { get; init; }
    public Order Order { get; init; } = default!;
    public required long ProductId { get; init; }
    public Product Product { get; init; } = default!;
    public required uint Amount { get; set; }
    public required decimal Cost { get; set; }
}

public class DeliveryAddress
{
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
}

public enum OrderStatus
{
    Placed = 0,
    WaitingForPayment = 1,
    Canceled = 2,
    InRealization = 3,
    Sent = 4,
    Delivered = 5,
}
