namespace ECommerce.ApplicationCore.Entities;

public class Payment
{
    public required Guid Id { get; init; }
    public required decimal Value { get; init; }
    public required PaymentType Type { get; init; }
    public required bool IsPaid { get; set; }

    private Payment() { }

    public static Payment Create(PaymentType type, decimal value)
    {
        return new Payment
        {
            Id = Guid.NewGuid(),
            Value = value,
            Type = type,
            IsPaid = false
        };
    }
}

public enum PaymentType
{
    CashOnDelivery,
    DotPay,
}