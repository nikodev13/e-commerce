using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Orders.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Orders.Commands;

public record PlaceOrderCommand(List<PlaceOrderCommand.OrderLine> OrderLines, PlaceOrderCommand.DeliveryOptions Delivery) 
    : ICommand<long>
{
    public record OrderLine(long ProductId, uint Quantity);
    public record DeliveryOptions(DeliveryOperator Operator, string Street, string PostalCode, string City);
}

internal sealed class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, long>
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
        decimal totalCost = 0;
        // build order lines
        foreach (var product in products)
        {
            // check quantities
            var commandOrderLine = command.OrderLines.First(x => x.ProductId == product.Id);
            if (product.InStockQuantity < commandOrderLine.Quantity) 
                throw new ProductOutOfStockException(product.Id, product.InStockQuantity);
            
            // create domain order line
            orderLines.Add(new OrderLine
            {
                OrderId = orderId,
                ProductId = product.Id,
                Quantity = commandOrderLine.Quantity,
                UnitPrice = product.Price
            });

            totalCost += decimal.Round(commandOrderLine.Quantity * product.Price,2);

            // debit stock
            product.InStockQuantity -= commandOrderLine.Quantity;
        }
        
        // create Payment
        var payment = Payment.Create(PaymentType.DotPay, totalCost);
        
        // create order
        var order = new Order
        {
            Id = orderId,
            CustomerId = customerId,
            PaymentId = payment.Id,
            Payment = payment,
            Delivery = new Delivery
            {
                Operator = command.Delivery.Operator,
                Street = command.Delivery.Street,
                PostalCode = command.Delivery.PostalCode,
                City = command.Delivery.City
            },
            OrderLines = orderLines,
        };

        await _appDbContext.Orders.AddAsync(order, cancellationToken);
        var affectedRows =  await _appDbContext.SaveChangesAsync(cancellationToken);
        if (affectedRows == 0)
        {
            throw new PlaceOrderException("Something went wrong with placing order. Please try again later.");
        }
        
        _logger.LogInformation("Customer with id `{customerId}` placed order with id {orderId}.", customerId, order.Id);
        
        return order.Id;
    }
}

internal sealed class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.Delivery).SetValidator(new AddressValidator());
        RuleForEach(x => x.OrderLines).SetValidator(new OrderLineValidator());
    }
    
    class OrderLineValidator : AbstractValidator<PlaceOrderCommand.OrderLine>
    {
        public OrderLineValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0u);
        }
    }
    
    class AddressValidator : AbstractValidator<PlaceOrderCommand.DeliveryOptions>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.PostalCode).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
        }
    }
}