using ECommerce.ApplicationCore.Features.AddressBook.Commands;

namespace ECommerce.API.Endpoints.RequestBodies;

public record AddAddressToAddressBookRequestBody(string Street, string PostalCode, string City)
{
    public AddAddressToAddressBookCommand ToCommand() => new(Street, PostalCode, City);
}

public record UpdateAddressInAddressBookRequestBody(string Street, string PostalCode, string City)
{
    public UpdateAddressInAddressBookCommand ToCommand(long addressId) => new(addressId, Street, PostalCode, City);
}