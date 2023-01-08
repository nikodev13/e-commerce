namespace ECommerce.Domain.Shared.ValueObjects;

public class Quantity
{
    public int Value { get; }
    
    public Quantity(int quantity)
    {
        Value = quantity;
    } 
    
    public static implicit operator int(Quantity id) => id.Value;
    public static implicit operator Quantity(int id) => new(id);
}