using System.Linq.Expressions;
using ECommerce.API.Tests.Configuration;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Tests;

[CollectionDefinition(TestingCollection.Name)]
public class TestingCollection : ICollectionFixture<Testing>
{
    public const string Name = "Testing";
}

public class Testing : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IServiceScopeFactory _scopeFactory;
    private HttpClient Client { get; }

    public Testing()
    {
        _factory = new CustomWebApplicationFactory();
        Client = _factory.CreateClient();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public async ValueTask<List<TEntity>> FindEntities<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class
    {
        var service = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        return await service.Set<TEntity>().Where(selector).ToListAsync();
    }
    
    public async ValueTask AddEntities<TEntity>(params TEntity[] entities) where TEntity : class
    {
        var service = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        await service.Set<TEntity>().AddRangeAsync(entities);
        await service.SaveChangesAsync();
    }
    
    public async ValueTask DeleteEntities<TEntity>(Expression<Func<TEntity, bool>> selector) where TEntity : class
    {
        var service = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        await service.Set<TEntity>().Where(selector).ExecuteDeleteAsync();
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}