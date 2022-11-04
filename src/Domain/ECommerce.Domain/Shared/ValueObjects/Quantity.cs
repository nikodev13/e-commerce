namespace ECommerce.Domain.Shared.ValueObjects;

public class Quantity
{
    public uint Value { get; }
    
    public Quantity(uint quantity)
    {
        Value = quantity;
    } 
    
    public static implicit operator uint(Quantity id) => id.Value;
    public static implicit operator Quantity(uint id) => new(id);
}