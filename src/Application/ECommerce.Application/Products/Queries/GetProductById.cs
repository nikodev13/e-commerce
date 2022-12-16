using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using ECommerce.Application.Products.ReadModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Queries;

public class GetProductByIdQuery : IQuery<ProductReadModel>
{
    public long Id { get; }

    public GetProductByIdQuery(long id)
    {
        Id = id;
    }
}

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductReadModel>
{
    private readonly IApplicationDatabase _database;

    public GetProductByIdQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<ProductReadModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _database.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
            return new NotFoundError($"Product with id {request.Id} not found.");

        var result = ProductReadModel.FromProduct(product);
        return result;
    }
}