using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Users.ValueObjects;

public class UserId
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidUserIdException();

        Value = value;
    }

    public static implicit operator Guid(UserId id) => id.Value;
    public static implicit operator UserId(Guid id) => new(id);
}

public class InvalidUserIdException : DomainException
{
    public InvalidUserIdException() : base("Invalid user id.") { }
}