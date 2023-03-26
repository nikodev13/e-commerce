using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.ReadModels;

public record CustomerAccountReadModel(string FirstName, string LastName, string Email, string PhoneNumber, List<AddressReadModel> Addresses)
{
    public static CustomerAccountReadModel From(CustomerAccount account)
        => new(account.FirstName,
            account.LastName,
            account.Email,
            account.PhoneNumber,
            account.Addresses.Select(AddressReadModel.From).ToList());
}