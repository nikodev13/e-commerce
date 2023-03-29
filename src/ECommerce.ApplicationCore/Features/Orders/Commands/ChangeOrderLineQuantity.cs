using ECommerce.ApplicationCore.Features.Orders.Exceptions;
using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Orders.Commands;

public record ChangeOrderLineQuantityCommand(long OrderId, long ProductId, uint NewQuantity) : ICommand;

public class ChangeOrderLineQuantityQueryHandler : ICommandHandler<ChangeOrderLineQuantityCommand>
{
    private readonly IAppDbContext _dbContext;

    public ChangeOrderLineQuantityQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask HandleAsync(ChangeOrderLineQuantityCommand command, CancellationToken cancellationToken)
    {
        var (orderId, productId, newQuantity) = command;

        var orderLine = await _dbContext.OrderLines
            .Where(x => x.OrderId == orderId && x.ProductId == productId)
            .FirstOrDefaultAsync(cancellationToken);

        if (orderLine is null) throw new OrderLineNotFoundException(orderId, productId);

        if (newQuantity == 0)
        {
            _dbContext.OrderLines.Remove(orderLine);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return;
        }
        
        var product = await _dbContext.Products.Where(x => x.Id == productId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null) throw new ProductNotFoundException(productId);

        var quantityDifference = (int)newQuantity - product.InStockQuantity;
        
        if (product.InStockQuantity > quantityDifference)
            throw new ProductOutOfStockException(productId, product.InStockQuantity);

        product.InStockQuantity += newQuantity;
        orderLine.Quantity = newQuantity;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
} 
