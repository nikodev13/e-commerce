using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Customers.Orders.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;

namespace ECommerce.ApplicationCore.Features.Customers.Orders.Commands;

public class PlaceOrderCommand : ICommand<long>
{
    public required List<OrderLine> OrderLines { get; init; }
    public required Address DeliveryAddress { get; init; }

    public record OrderLine(long ProductId, uint Amount);
    public record Address(string Street, string PostalCode, string City);
}

public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, long>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ISnowflakeIdProvider _idProvider;
    private readonly ILogger<PlaceOrderCommandHandler> _logger;

    public PlaceOrderCommandHandler(IAppDbContext appDbContext,
        IUserContextProvider userContextProvider,
        ISnowflakeIdProvider idProvider,
        ILogger<PlaceOrderCommandHandler> logger)
    {
        _appDbContext = appDbContext;
        _userContextProvider = userContextProvider;
        _idProvider = idProvider;
        _logger = logger;
    }
    
    public async ValueTask<long> HandleAsync(PlaceOrderCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;

        var productIds = command.OrderLines.Select(x => x.ProductId);
        
        var products = await _appDbContext.Products
            .Where(x => productIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var orderId = _idProvider.GenerateId();
        var orderLines = new List<OrderLine>(10);
        // build order lines
        foreach (var product in products)
        {
            // check quantities
            var commandOrderLine = command.OrderLines.First(x => x.ProductId == product.Id);
            if (product.InStockQuantity < commandOrderLine.Amount) 
                throw new ProductOutOfStockException(product.Id, product.InStockQuantity);
            
            // create domain order line
            var orderLine = new OrderLine
            {
                OrderId = orderId,
                ProductId = product.Id,
                Amount = commandOrderLine.Amount,
                Cost = product.Price
            };

            // debit stock
            product.InStockQuantity -= commandOrderLine.Amount;
        }
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        // create order
        var order = new Order
        {
            Id = orderId,
            CustomerId = customerId,
            DeliveryAddress = new DeliveryAddress
            {
                Street = command.DeliveryAddress.Street,
                PostalCode = command.DeliveryAddress.PostalCode,
                City = command.DeliveryAddress.City
            },
            OrderLines = orderLines
        };

        await _appDbContext.Orders.AddAsync(order, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        // order placed
        
        _logger.LogInformation("Customer with id `{customerId}` placed order with id {orderId}.", customerId, order.Id);
        
        return order.Id;
    }
}

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.DeliveryAddress).SetValidator(new AddressValidator());
        RuleForEach(x => x.OrderLines).SetValidator(new OrderLineValidator());
    }
    
    public class OrderLineValidator : AbstractValidator<PlaceOrderCommand.OrderLine>
    {
        public OrderLineValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0u);
        }
    }
    
    public class AddressValidator : AbstractValidator<PlaceOrderCommand.Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
        }
    }
}