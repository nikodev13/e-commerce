using ECommerce.ApplicationCore.Features.Customers.Commands;

namespace ECommerce.API.Endpoints.RequestBodies;

public record RegisterCustomerRequestBody(string FirstName, string LastName, string Email, string PhoneNumber)
{
    public RegisterCustomerCommand ToCommand() => new(FirstName, LastName, Email, PhoneNumber);
}

public record UpdateCustomerFullNameRequestBody(string FirstName, string LastName)
{
    public UpdateCustomerFullNameCommand ToCommand() => new(FirstName, LastName);
}

public record UpdateCustomerContactDataRequestBody(string Email, string PhoneNumber)
{
    public UpdateCustomerContactDataCommand ToCommand() => new(Email, PhoneNumber);
}