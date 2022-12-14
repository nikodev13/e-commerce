using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using ECommerce.Application.Products.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Queries;

public class GetProductByIdQuery : IQuery<ProductDto>
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

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDatabase _database;

    public GetProductByIdQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _database.Products.FirstOrDefaultAsync(cancellationToken);

        if (product is null)
            return new NotFoundError($"Product with id {request.Id} not found.");

        var result = ProductDto.FromProduct(product);
        return result;
    }
}