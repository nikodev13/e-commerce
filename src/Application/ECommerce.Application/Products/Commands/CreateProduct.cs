using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
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
    public long CategoryId { get;  }

    public CreateProductCommand(string name, string description, long categoryId)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
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
    
    public async Task<Result<ProductReadModel>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _database.Products.AnyAsync(x => x.Name == request.Name, cancellationToken))
            return new AlreadyExistsError($"Product with name {request.Name} already exists.");
            
        var category = await _database.Categories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return new BadRequestError($"Category with id {request.CategoryId} does not exist.");
            
        var product = new Product
        {
            Id = _idService.GenerateId(),
            Name = request.Name,
            Description = request.Description,
            Category = category
        };

        await _database.Products.AddAsync(product, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);

        var result = ProductReadModel.FromProduct(product);
        return result;
    }
}