using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Shared.ValueObjects;

public class Quantity
{
    public uint Value { get; }
    
    public Quantity(uint quantity)
    {
        if (quantity < 0)
        {
            throw new InvalidQuantityException();
        }
        Value = quantity;
    } 
    
    public static implicit operator uint(Quantity id) => id.Value;
    public static implicit operator Quantity(uint id) => new(id);
}