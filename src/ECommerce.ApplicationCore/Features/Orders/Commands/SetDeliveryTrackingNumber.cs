using ECommerce.ApplicationCore.Features.Orders.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Orders.Commands;

public record SetDeliveryTrackingNumberCommand(long OrderId, string TrackingNumber) : ICommand; 

internal sealed class SetDeliveryTrackingNumberCommandHandler : ICommandHandler<SetDeliveryTrackingNumberCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<SetDeliveryTrackingNumberCommandHandler> _logger;

    public SetDeliveryTrackingNumberCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider, ILogger<SetDeliveryTrackingNumberCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(SetDeliveryTrackingNumberCommand command, CancellationToken cancellationToken)
    {
        var adminId = _userContextProvider.UserId!.Value;
        var (orderId, trackingNumber) = command;

        var order = await _dbContext.Orders
            .Include(x => x.Delivery)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null) throw new OrderNotFoundException(orderId);

        order.Delivery.TrackingNumber = trackingNumber;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Admin with id `{adminId}` set delivery tracking number for order with id `{orderId}`.", adminId, trackingNumber);
    }
}