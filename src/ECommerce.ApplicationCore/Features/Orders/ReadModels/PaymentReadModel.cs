using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Orders.ReadModels;

public record PaymentReadModel(Guid PaymentId, string Type, decimal Value, bool IsPaid)
{
    public static PaymentReadModel From(Payment payment)
        => new(payment.Id, payment.Type.ToString(), payment.Value, payment.IsPaid);
}