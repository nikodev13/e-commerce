using ECommerce.ApplicationCore.Features.Orders.Exceptions;
using ECommerce.ApplicationCore.Features.Orders.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Orders.Queries;

public record GetOrderByIdForManagementQuery(long OrderId) : IQuery<ManagementOrderReadModel>;

public class GetByIdForManagementQueryHandler : IQueryHandler<GetOrderByIdForManagementQuery, ManagementOrderReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetByIdForManagementQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<ManagementOrderReadModel> HandleAsync(GetOrderByIdForManagementQuery query, CancellationToken cancellationToken)
    {
        var orderReadModel = await _dbContext.Orders
            .Include(x => x.Payment)
            .Include(x => x.DeliveryAddress)
            .Include(x => x.OrderLines)
            .ThenInclude(x => x.Product)
            .Where(x => x.Id == query.OrderId)
            .Select(x => ManagementOrderReadModel.From(x))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return orderReadModel ?? throw new OrderNotFoundException(query.OrderId);
    }
};