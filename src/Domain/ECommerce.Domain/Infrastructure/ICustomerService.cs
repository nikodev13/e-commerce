using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Infrastructure;

public interface ICustomerService
{
    Task<Customer?> GetById(Guid id);
    
}