using System.Diagnostics;
using System.Linq.Expressions;
using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.Constants;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Management.Products.Queries;

public class GetPaginatedProductsQuery : IQuery<PaginatedList<ProductReadModel>>
{
    public required int PageSize { get; set; } = 1;
    public required int PageNumber { get; set; } = 25;
    public required string? SearchPhrase { get; init; }
    public required string? SortBy { get; init; }
    public required SortDirection? SortDirection { get; init; }
}

public class GetAllProductsQueryHandler : IQueryHandler<GetPaginatedProductsQuery, PaginatedList<ProductReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllProductsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<PaginatedList<ProductReadModel>> HandleAsync(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .Include(x => x.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            query = query.Where(x => x.Name.Contains(request.SearchPhrase)
                                     || x.Category.Name.Contains(request.SearchPhrase));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            Expression<Func<Product, object>> sortByColumn = request.SortBy switch
            {
                nameof(Product.Name) => x => x.Name,
                nameof(Product.Category) => x => x.Category.Name,
                _ => x => x.Name
            };

            var sortDirection = request.SortDirection ?? SortDirection.ASC;
            query = sortDirection is SortDirection.ASC
                ? query.OrderBy(sortByColumn)
                : query.OrderByDescending(sortByColumn);
        }
        
        var result = await query.Skip(request.PageSize * request.PageNumber)
            .Take(request.PageSize)
            .Select(x => ProductReadModel.FromProduct(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var totalItems = await _dbContext.Products.CountAsync(cancellationToken);

        return new PaginatedList<ProductReadModel>(result, request.PageSize, request.PageNumber, totalItems);
    }
}
