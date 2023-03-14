using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Categories.Queries;

public class GetAllCustomerCategoriesQuery : IQuery<List<CustomerCategoryReadModel>> { }

public class GetAllCustomerCategoriesQueryHandler 
    : IQueryHandler<GetAllCustomerCategoriesQuery, List<CustomerCategoryReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllCustomerCategoriesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<List<CustomerCategoryReadModel>> HandleAsync(GetAllCustomerCategoriesQuery query, CancellationToken cancellationToken)
    {
        var readModels = await _dbContext.Products
            .Select(x => new CustomerCategoryReadModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);

        return readModels;
    }
}