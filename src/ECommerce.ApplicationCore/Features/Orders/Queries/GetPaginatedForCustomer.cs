using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Orders.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.Constants;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Orders.Queries;

public record GetPaginatedOrdersQuery(int PageSize, int PageNumber, OrderStatus? OrderStatus) 
    : IQuery<PaginatedList<OrderReadModel>>;

public class GetPaginatedOrdersQueryHandler : IQueryHandler<GetPaginatedOrdersQuery, PaginatedList<OrderReadModel>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public GetPaginatedOrdersQueryHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<PaginatedList<OrderReadModel>> HandleAsync(GetPaginatedOrdersQuery query, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var baseQuery = _dbContext.Orders
            .Include(x => x.Payment)
            .Include(x => x.DeliveryAddress)
            .Include(x => x.OrderLines)
                .ThenInclude(x => x.Product)
            .Where(x => x.CustomerId == customerId);

        var (pageSize, pageNumber, orderStatus) = query;
        if (orderStatus is not null)
        {
            baseQuery = baseQuery.Where(x => x.Status == orderStatus);
        }

        var orders = await baseQuery.Skip(pageNumber * pageSize)
            .Take(query.PageSize)
            .Select(x => OrderReadModel.From(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var count = await _dbContext.Orders.CountAsync(cancellationToken);

        return new PaginatedList<OrderReadModel>(orders, pageSize, pageNumber, count);
    }
}
