namespace ECommerce.Domain.SeedWork;

public abstract class BusinessRuleBase
{
    public abstract string Message { get; }
    public abstract bool IsBroken();
}