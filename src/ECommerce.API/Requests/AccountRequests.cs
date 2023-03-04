namespace ECommerce.API.Requests;

public class RegisterCustomerAccountRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}

public class UpdateCustomerFullNameRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}

public class UpdateCustomerContactDataRequest
{
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}