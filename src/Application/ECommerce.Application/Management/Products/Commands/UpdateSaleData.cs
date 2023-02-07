using ECommerce.Application.Management.Products.Exceptions;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Management.Products.Commands;

public class UpdateProductSaleDataCommand : ICommand
{
    public required long Id { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required bool IsActive { get; init; }
}

public class UpdateProductSaleDataCommandHandler : ICommandHandler<UpdateProductSaleDataCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<UpdateProductDetailsCommandHandler> _logger;

    public UpdateProductSaleDataCommandHandler(IAppDbContext dbContext,
        IUserContextProvider userContextProvider,
        ILogger<UpdateProductDetailsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(UpdateProductSaleDataCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
            throw new ProductNotFoundException(request.Id);

        product.Price = request.Price;
        product.InStockQuantity = request.Quantity;
        product.IsActive = request.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` updated sale data for product with id `{@productId}`.",
            _userContextProvider.UserId, product.Id.Value);
        
        return Unit.Value;
    }
}

public class UpdateProductSaleDataCommandValidator : AbstractValidator<UpdateProductSaleDataCommand>
{
    public UpdateProductSaleDataCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.IsActive)
            .NotEmpty();
    }
}
