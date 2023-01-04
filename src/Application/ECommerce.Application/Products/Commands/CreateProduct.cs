using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Products.ReadModels;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Commands;

public class CreateProductCommand : ICommand<ProductReadModel>
{
    public string Name { get; }
    public string Description { get; }
    public long CategoryId { get; }

    public decimal Price { get; }
    public uint Quantity { get; }

    public CreateProductCommand(string name, string description, long categoryId, decimal price, uint quantity)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        Price = price;
        Quantity = quantity;
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);
        RuleFor(x => x.CategoryId)
            .NotEmpty();
        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Quantity)
            .NotEmpty();
    }
}

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductReadModel>
{
    private readonly IApplicationDatabase _database;
    private readonly ISnowflakeIdService _idService;

    public CreateProductCommandHandler(IApplicationDatabase database, ISnowflakeIdService idService)
    {
        _database = database;
        _idService = idService;
    }
    
    public async Task<ProductReadModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _database.Products.AnyAsync(x => x.Name == request.Name, cancellationToken))
            throw new AlreadyExistsException($"Product with name {request.Name} already exists.");
            
        var category = await _database.Categories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (category is null)
            throw new BadRequestException($"Category with id {request.CategoryId} does not exist.");
            
        var product = new Product
        {
            Id = _idService.GenerateId(),
            Name = request.Name,
            Description = request.Description,
            Category = category,
            Price = request.Price,
            Quantity = request.Quantity
        };

        await _database.Products.AddAsync(product, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);

        var result = ProductReadModel.FromProduct(product);
        return result;
    }
}