namespace ECommerce.Application.Shared.Results;

public sealed class None
{
    public static None Value { get; } = new None();

    private None()
    {
    }
}