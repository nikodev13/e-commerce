namespace ECommerce.Domain.SeedWork;

public abstract class Entity
{
    protected void CheckRule(BusinessRuleBase businessRule)
    {
        if (businessRule.IsBroken())
        {
            throw new BusinessRuleValidationException(businessRule);
        }
    }
}