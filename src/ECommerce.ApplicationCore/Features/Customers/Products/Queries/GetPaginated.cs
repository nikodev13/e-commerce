using System.Linq.Expressions;
using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Customers.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.Constants;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Products.Queries;

public class GetPaginatedCustomerProductsQuery : IQuery<PaginatedList<CustomerProductReadModel>>
{
    public required int PageSize { get; set; } = 1;
    public required int PageNumber { get; set; } = 25;
    public string? SearchPhrase { get; init; }
    public string? SortBy { get; init; }
    public SortDirection? SortDirection { get; init; }
}

public class GetPaginatedCustomerProductsQueryHandler
    : IQueryHandler<GetPaginatedCustomerProductsQuery, PaginatedList<CustomerProductReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetPaginatedCustomerProductsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<PaginatedList<CustomerProductReadModel>> HandleAsync(GetPaginatedCustomerProductsQuery query, CancellationToken cancellationToken)
    {
        var queryBase = _dbContext.Products
            .Include(x => x.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchPhrase))
        {
            queryBase = queryBase.Where(x => x.Name.Contains(query.SearchPhrase)
                                             || x.Category.Name.Contains(query.SearchPhrase));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            var sortBy =  $"{query.SortBy[0].ToString().ToUpper()}{query.SortBy[1..].ToLower()}";
            Expression<Func<Product, object>> sortByColumn = sortBy switch
            {
                nameof(Product.Name) => x => x.Name,
                nameof(Product.Category) => x => x.Category.Name,
                _ => x => x.Name
            };

            var sortDirection = query.SortDirection ?? SortDirection.ASC;
            queryBase = sortDirection is SortDirection.ASC
                ? queryBase.OrderBy(sortByColumn)
                : queryBase.OrderByDescending(sortByColumn);
        }
        
        var result = await queryBase.Skip(query.PageSize * query.PageNumber)
            .Take(query.PageSize)
            .Select(x => CustomerProductReadModel.From(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var totalItems = await _dbContext.Products.CountAsync(cancellationToken);

        return new PaginatedList<CustomerProductReadModel>(result, query.PageSize, query.PageNumber, totalItems);
    }
}