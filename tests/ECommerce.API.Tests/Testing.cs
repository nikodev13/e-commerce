using System.Linq.Expressions;
using ECommerce.API.Tests.Configuration;
using ECommerce.API.Tests.DummyData;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Tests;

[CollectionDefinition(Name)]
public class TestingCollection : ICollectionFixture<Testing>
{
    public const string Name = "Testing";
}

public class Testing : IDisposable, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IServiceScopeFactory _scopeFactory;
    public HttpClient Client { get; }

    public Testing()
    {
        _factory = new CustomWebApplicationFactory();
        Client = _factory.CreateClient();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public AppDbContext GetDbContext()
    {
        var service = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        return service;
    }

    public async Task InitializeAsync()
    {
        await CleanUp();
        
        var db = GetDbContext();
        await db.Users.AddRangeAsync(DummyUsers.Data);
        await db.Categories.AddRangeAsync(DummyCategories.Data);
        await db.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await CleanUp();
    }

    private async Task CleanUp()
    {
        var db = GetDbContext();
        await db.Users.Where(x => DummyUsers.Data.Contains(x)).ExecuteDeleteAsync();
        await db.Categories.Where(x => DummyCategories.Data.Contains(x)).ExecuteDeleteAsync();
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}