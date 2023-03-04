using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Customer.Orders.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Customer.Orders.Commands;

public class PlaceOrderCommand : ICommand<long>
{
    public required List<OrderLine> OrderLines { get; init; }
    public required Address DeliveryAddress { get; init; }
    
    public class OrderLine
    {
        public required long ProductId { get; init; }
        public required uint Amount { get; init; }
    }
    
    public class Address
    {
        public required string Street { get; init; }
        public required string PostalCode { get; init; }
        public required string City { get; init; }
    }
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

        // check quantities
        foreach (var product in products)
        {
            var orderLine = command.OrderLines.First(x => x.ProductId == product.Id);
            if (product.InStockQuantity < orderLine.Amount) 
                throw new ProductOutOfStockException(product.Id, product.InStockQuantity);
        }
        
        // debit stock
        foreach (var product in products)
        {
            var orderLine = command.OrderLines.First(x => x.ProductId == product.Id);
            product.InStockQuantity -= orderLine.Amount;
        }
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        // create order
        var order = new Order
        {
            Id = _idProvider.GenerateId(),
            CustomerId = customerId,
            DeliveryAddress = new DeliveryAddress
            {
                Street = command.DeliveryAddress.Street,
                PostalCode = command.DeliveryAddress.PostalCode,
                City = command.DeliveryAddress.City
            },
            OrderLines = command.OrderLines.Select(x => new OrderLine
            {
                OrderId = _idProvider.GenerateId(),
                ProductId = x.ProductId,
                Amount = x.Amount
            }).ToList()
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