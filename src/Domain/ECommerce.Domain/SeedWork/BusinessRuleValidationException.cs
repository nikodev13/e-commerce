namespace ECommerce.Domain.SeedWork;

public class BusinessRuleValidationException : Exception
{
    public BusinessRuleBase BrokenRule { get; }
    
    public BusinessRuleValidationException(BusinessRuleBase brokenRule) : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
    }

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
    }
}