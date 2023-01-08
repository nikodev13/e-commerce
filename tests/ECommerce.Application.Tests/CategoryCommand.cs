using ECommerce.Application.Categories.Commands;
using ECommerce.Application.Categories.ReadModels;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Services;
using ECommerce.Application.Tests.Persistence;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;
using ECommerce.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace ECommerce.Application.Tests;

public class CategoryCommands : IClassFixture<DatabaseFixture>
{
    private readonly ISnowflakeIdService _idService;
    private readonly ApplicationDbContext _dbContext;
    
    public CategoryCommands(DatabaseFixture databaseFixture)
    {
        _idService = new SnowflakeIdService();
        _dbContext = databaseFixture.ApplicationDbContext;
    }

    [Fact]
    public void CreateCategoryCommand_ShouldThrowExceptions_WhenCategoryAlreadyExists()
    {
        var category = Category.CreateNew("test", _idService);
        _dbContext.Categories.Add(category);

        var commandHandler = new CreateCategoryCommandHandler(_dbContext, _idService, new Mock<ILogger<CreateCategoryCommandHandler>>().Object);
        async Task<CategoryReadModel> Action() => await commandHandler.Handle(new CreateCategoryCommand(category.Name), CancellationToken.None);

        Assert.ThrowsAsync<AlreadyExistsException>(Action);
    }
    
    [Fact]
    public void CreateCategoryCommand_ShouldCreateNewCategory()
    {
        var commandHandler = new CreateCategoryCommandHandler(_dbContext, _idService, new Mock<ILogger<CreateCategoryCommandHandler>>().Object);
        async Task<CategoryReadModel> Action() => await commandHandler.Handle(new CreateCategoryCommand("newCategory"), CancellationToken.None);

        Assert.ThrowsAsync<AlreadyExistsException>(Action);
    }
    
    [Fact]
    public void Update_ShouldThrowExceptions_WhenCategoryAlreadyExists()
    {
        var category = Category.CreateNew("test", _idService);
        _dbContext.Categories.Add(category);

        var commandHandler = new UpdateCategoryCommandHandler(_dbContext, new Mock<ILogger<UpdateCategoryCommandHandler>>().Object);
        async Task<Unit> Action() => await commandHandler.Handle(new UpdateCategoryCommand(category.Id, category.Name), CancellationToken.None);

        Assert.ThrowsAsync<AlreadyExistsException>(Action);
    }
}