using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Features.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Products.Queries;

public record GetProductByIdQuery(long Id) : IQuery<ProductReadModel>;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<ProductReadModel> HandleAsync(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var readModel = await _dbContext.Products
            .Select(x => ProductReadModel.From(x))
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (readModel is null)
            throw new CustomerProductNotFoundByIdException(query.Id);

        return readModel;
    }
}