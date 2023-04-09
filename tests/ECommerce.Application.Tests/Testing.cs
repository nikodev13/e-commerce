using ECommerce.Application.Tests.Categories;
using ECommerce.Application.Tests.Customers;
using ECommerce.Application.Tests.Products;
using ECommerce.Application.Tests.Users;
using ECommerce.Application.Tests.Utilities;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Tests;


[CollectionDefinition(Name)]
public class TestingCollection : ICollectionFixture<Testing>
{
    public const string Name = "Testing";
}

public class Testing : IDisposable, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IServiceScopeFactory _scopeFactory;

    public Testing()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }
    
    public async Task<TResult> ExecuteQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
    {
        var handler = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.HandleAsync(query, cancellationToken);
    }

    public async Task<TResult> ExecuteCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>
    {
        var handler = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.HandleAsync(command, cancellationToken);
    }
    
    public async Task ExecuteCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        var handler = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }

    public IAppDbContext GetAppDbContext() =>
        _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IAppDbContext>();

    public void Dispose()
    {
        _factory.Dispose();
    }

    public async Task InitializeAsync()
    {
        var usersDbContext = GetAppDbContext();
        usersDbContext.Users.AddRange(DummyUsers.Data);
        usersDbContext.Customers.AddRange(DummyCustomersAccounts.Data);
        usersDbContext.Categories.AddRange(DummyCategories.Data);
        usersDbContext.Products.AddRange(DummyProducts.Data);
        await usersDbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DisposeAsync()
    {
        var usersDbContext = GetAppDbContext();
        usersDbContext.Users.RemoveRange(DummyUsers.Data);
        usersDbContext.Customers.RemoveRange(DummyCustomersAccounts.Data);
        usersDbContext.Categories.RemoveRange(DummyCategories.Data);
        usersDbContext.Products.RemoveRange(DummyProducts.Data);
        await usersDbContext.SaveChangesAsync(CancellationToken.None);
    }
}