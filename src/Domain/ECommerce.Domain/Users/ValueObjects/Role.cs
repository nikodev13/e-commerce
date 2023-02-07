using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Users.ValueObjects;

public class Role
{
    public static readonly IEnumerable<string> AvailableRoles = new[] { "User", "Admin" };

    public static Role User => new("User");
    public static Role Admin => new("Admin");
    
    public string Value { get; }
    public Role(string value)
    {
        var role = AvailableRoles.SingleOrDefault(x => string.Equals(value, x, StringComparison.OrdinalIgnoreCase));
        Value = role ?? throw new InvalidRoleException();
    }
}

public class InvalidRoleException : DomainException
{
    public InvalidRoleException() : base("Invalid user role.") { }
}

