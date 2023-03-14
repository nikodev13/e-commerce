using ECommerce.ApplicationCore.Features.Management.Categories.Commands;
using ECommerce.ApplicationCore.Features.Management.Categories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Management.Categories;

[Collection(TestingCollection.Name)]
public class UpdateCategory
{
    private readonly Testing _testing;

    public UpdateCategory(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task UpdateCategory_Successfully()
    {
        // arrange
        var db = _testing.GetAppDbContext();
        var id = DummyCategories.Data[0].Id;
        var command = new UpdateCategoryCommand
        {
            Id = id,
            Name = "aa"
        };

        // act
        await _testing.ExecuteCommandAsync(command);

        // assert
        var exist = await db.Categories.AnyAsync(x => x.Name == command.Name);
        Assert.True(exist);
    }
    
    [Fact]
    public async Task UpdateCategory_WithNameThatAlreadyExists_Successfully()
    {
        // arrange
        var id = DummyCategories.Data[0].Id;
        var name = DummyCategories.Data[1].Name;
        var command = new UpdateCategoryCommand
        {
            Id = id,
            Name = name
        };

        // act
        async Task Action() => await _testing.ExecuteCommandAsync(command);

        // assert
        await Assert.ThrowsAsync<CategoryAlreadyExistsException>(Action);
    }
    
    [Fact]
    public async Task UpdateCategory_WithInvalidId_ThrowsCategoryNotFoundException()
    {
        // arrange
        var id = 1;
        var command = new UpdateCategoryCommand
        {
            Id = id,
            Name = "aa"
        };

        // act
        async Task Action() => await _testing.ExecuteCommandAsync(command);

        // assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(Action);
    }
}
