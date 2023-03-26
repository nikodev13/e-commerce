using ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;

namespace ECommerce.API.Endpoints.Requests;

public record RegisterCustomerAccountRequest(string FirstName, string LastName, string Email, string PhoneNumber)
{
    public RegisterCustomerAccountCommand ToCommand()
        => new(FirstName, LastName, Email, PhoneNumber);
}

public record UpdateCustomerFullNameRequest(string FirstName, string LastName)
{
    public UpdateCustomerFullNameCommand ToCommand()
        => new(FirstName, LastName);
}

public record UpdateCustomerContactDataRequest(string Email, string PhoneNumber)
{
    public UpdateCustomerContactDataCommand ToCommand()
        => new(Email, PhoneNumber);
}