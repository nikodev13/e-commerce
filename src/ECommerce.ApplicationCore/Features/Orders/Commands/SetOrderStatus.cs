using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Orders.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Orders.Commands;

public record SetOrderStatusCommand(long OrderId, OrderStatus OrderStatus) : ICommand;

public class  SetOrderStatusCommandHandler : ICommandHandler<SetOrderStatusCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<SetOrderStatusCommandHandler> _logger;

    public SetOrderStatusCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider, ILogger<SetOrderStatusCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(SetOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var adminId = _userContextProvider.UserId!.Value;
        var (orderId, newStatus) = command;
        var order = await _dbContext.Orders
            .Where(x => x.Id == orderId)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null) throw new OrderNotFoundException(orderId);
        var oldStatus = order.Status;
        order.Status = newStatus;
        order.OperatedBy = adminId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Order status was changed from {oldStatus} to {newStatus} for order with id `{orderId}`. Operated by admin with id {adminId}.", oldStatus, newStatus, order, adminId);
    }
}