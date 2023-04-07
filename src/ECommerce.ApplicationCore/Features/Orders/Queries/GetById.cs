using System.Xml.Schema;
using ECommerce.ApplicationCore.Features.Orders.Exceptions;
using ECommerce.ApplicationCore.Features.Orders.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Orders.Queries;

public record GetOrderByIdQuery(long Id) : IQuery<OrderReadModel>;

internal sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderReadModel> {
    
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public GetOrderByIdQueryHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<OrderReadModel> HandleAsync(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;

        var order = await _dbContext.Orders
            .Where(x => x.CustomerId == customerId && x.Id == query.Id)
            .Include(x => x.Payment)
            .Include(x => x.DeliveryAddress)
            .Include(x => x.OrderLines)
            .ThenInclude(x => x.Product)
            .Select(x => OrderReadModel.From(x))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return order ?? throw new OrderNotFoundException(query.Id);
    }
}