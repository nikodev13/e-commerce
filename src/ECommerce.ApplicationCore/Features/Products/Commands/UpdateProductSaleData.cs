using System.Data.Common;
using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Products.Commands;

public record UpdateProductSaleDataCommand(long Id, decimal Price, uint Quantity, bool IsActive) : ICommand;

internal sealed class UpdateProductSaleDataCommandHandler : ICommandHandler<UpdateProductSaleDataCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<UpdateProductSaleDataCommandHandler> _logger;

    public UpdateProductSaleDataCommandHandler(IAppDbContext dbContext,
        IUserContextProvider userContextProvider,
        ILogger<UpdateProductSaleDataCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(UpdateProductSaleDataCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
            throw new ProductNotFoundException(request.Id);

        var userId = _userContextProvider.UserId!.Value;
        
        product.Price = request.Price;
        product.InStockQuantity = request.Quantity;
        product.IsActive = request.IsActive;
        product.LastModifiedBy = userId;
        product.LastModifiedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` updated sale data for product with id `{@productId}`.",
            userId, product.Id);
    }
}

internal sealed class UpdateProductSaleDataCommandValidator : AbstractValidator<UpdateProductSaleDataCommand>
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
            .GreaterThanOrEqualTo(0u);
        RuleFor(x => x.IsActive)
            .NotEmpty();
    }
}
