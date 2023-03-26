using ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;

namespace ECommerce.API.Endpoints.Requests;

public record CreateCustomerAddressRequest(string Street, string PostalCode, string City)
{
    public CreateCustomerAddressCommand ToCommand() => new(Street, PostalCode, City);
}

public record UpdateCustomerAddressRequest(string Street, string PostalCode, string City)
{
    public UpdateCustomerAddressCommand ToCommand(long addressId) => new(addressId, Street, PostalCode, City);
}