namespace ECommerce.Domain.Shared.ValueObjects;

public class MoneyValue
{
    public decimal Value { get; }

    public MoneyValue(decimal value)
    {
        Value = value;
    }
    
    public static implicit operator decimal(MoneyValue money) => money.Value;
    public static implicit operator MoneyValue(decimal moneyValue) => new(moneyValue);
}