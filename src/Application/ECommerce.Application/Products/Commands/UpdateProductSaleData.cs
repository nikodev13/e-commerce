using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Commands;

public class UpdateProductSaleDataCommand : ICommand
{
    public long Id { get; }
    public decimal Price { get; }
    public int Quantity { get; }
    
    public UpdateProductSaleDataCommand(long id, decimal price, int quantity)
    {
        Id = id;
        Price = price;
        Quantity = quantity;
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
            .NotEmpty();
    }
}

public class UpdateProductSaleDataCommandHandler : ICommandHandler<UpdateProductSaleDataCommand>
{
    private readonly IApplicationDatabase _database;

    public UpdateProductSaleDataCommandHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Unit> Handle(UpdateProductSaleDataCommand request, CancellationToken cancellationToken)
    {
        var product = await _database.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
            throw new NotFoundException($"Product with ID `{request.Id}` does not exist.");

        product.Price = request.Price;
        product.InStockQuantity = request.Quantity;

        await _database.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}