using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Customer.Account;

public class CustomerAccountReadModel
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    
    private CustomerAccountReadModel() { }

    public static CustomerAccountReadModel FromCustomerAccount(CustomerAccount customerAccount)
    {
        return new CustomerAccountReadModel
        {
            FirstName = customerAccount.FirstName,
            LastName = customerAccount.FirstName,
            Email = customerAccount.Email,
            PhoneNumber = customerAccount.PhoneNumber
        };
    }
}