using ECommerce.ApplicationCore.Features.Orders.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Orders.Commands;

public record ChangeOrderDeliveryAddressCommand(long OrderId, string Street, string PostalCode, string City) : ICommand;

internal sealed class ChangeOrderDeliveryAddressCommandHandler : ICommandHandler<ChangeOrderDeliveryAddressCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<ChangeOrderDeliveryAddressCommandHandler> _logger;

    public ChangeOrderDeliveryAddressCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider, ILogger<ChangeOrderDeliveryAddressCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(ChangeOrderDeliveryAddressCommand command, CancellationToken cancellationToken)
    {
        var adminId = _userContextProvider.UserId!.Value;
        var orderId = command.OrderId;

        var order = await _dbContext.Orders
            .Include(x => x.Delivery)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

        if (order is null) throw new OrderNotFoundException(orderId);

        order.Delivery.Street = command.Street;
        order.Delivery.PostalCode = command.PostalCode;
        order.Delivery.City = command.City;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Admin with id `{adminId}` changed delivery address of order with id `{orderId}`", adminId, orderId);
    }
}