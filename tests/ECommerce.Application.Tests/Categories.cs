using ECommerce.Application.Categories.Commands;
using ECommerce.Application.Common.Results.Errors;
using ECommerce.Application.Common.Services;
using ECommerce.Application.Tests.Persistence;
using ECommerce.Application.Tests.Services;
using ECommerce.Domain.Shared.Services;

namespace ECommerce.Application.Tests;

public class Categories : IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly ISnowflakeIdService _idService;

    public Categories()
    {
        _dbContext = new AppDbContext();
        _idService = new SnowflakeIdService();
    }

    [Fact]
    public async void CreateCategoryCommandHandler_ForCreatingCategoryThatExistsInDb_ReturnsAlreadyExistsError()
    {
        var logger = LoggerMock.GetLogger<CreateCategoryCommandHandler>().Object;
        var command = new CreateCategoryCommand("CategoryCreate");
        var handler = new CreateCategoryCommandHandler(_dbContext, _idService, logger);
        
        await handler.Handle(command, default);
        var result = await handler.Handle(command, default);
        
        Assert.False(result.IsSuccess);
        Assert.IsType<AlreadyExistsError>(result.Error);
    }
    
    [Fact]
    public async void UpdateCategoryCommandHandler_ForUpdatingCategoryThatNotExistsInDb_ReturnsNotFoundException()
    {
        var command = new UpdateCategoryCommand(123456789, "CategoryUpdate");
        var handler = new UpdateCategoryCommandHandler(_dbContext);
        
        var result = await handler.Handle(command, default);
        
        Assert.False(result.IsSuccess);
        Assert.IsType<NotFoundError>(result.Error);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}