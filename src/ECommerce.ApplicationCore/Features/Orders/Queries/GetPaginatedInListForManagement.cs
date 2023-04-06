using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Orders.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Orders.Queries;

public record GetPaginatedOrdersInListForManagementQuery(int PageSize, int PageNumber, OrderStatus OrderStatus = OrderStatus.Paid) 
    : IQuery<PaginatedList<OrderInListReadModel>>;

public class GetPaginatedInListForManagementQueryHandler : IQueryHandler<GetPaginatedOrdersInListForManagementQuery, PaginatedList<OrderInListReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetPaginatedInListForManagementQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<PaginatedList<OrderInListReadModel>> HandleAsync(GetPaginatedOrdersInListForManagementQuery query, CancellationToken cancellationToken)
    {
        var (pageSize, pageNumber, orderStatus) = query;

        var orderInListReadModels = await _dbContext.Orders
            .Where(x => x.Status == orderStatus)
            .Skip(pageSize * pageNumber)
            .Take(pageNumber)
            .Select(x => OrderInListReadModel.From(x))
            .ToListAsync(cancellationToken);

        var totalCount = await _dbContext.Orders.CountAsync(cancellationToken);
        
        return new PaginatedList<OrderInListReadModel>(orderInListReadModels, pageSize, pageNumber, totalCount);
    }
}