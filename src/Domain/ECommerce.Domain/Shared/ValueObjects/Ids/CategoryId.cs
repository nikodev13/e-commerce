namespace ECommerce.Domain.Shared.ValueObjects.Ids;

public class CategoryId
{
    public long Value { get; }

    public CategoryId(long id)
    {
        Value = id;
    }

    public static implicit operator long(CategoryId id) => id.Value;
    public static implicit operator CategoryId(long id) => new(id);
}