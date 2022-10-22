namespace ECommerce.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public Address FirstAddress { get; set; }
    public Address? SecondAddress { get; set; }

    public Cart Cart { get; set; }
    //public ICollection<Order> Orders { get; set; }
}