using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Application.Tests.Users;
using ECommerce.Application.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using ICommand = ECommerce.Application.Shared.CQRS.ICommand;

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
    
    public async Task<TResult> ExecuteQuerydAsync<TResult>(IQuery<TResult> query)
    {
        var dispatcher = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IQueryDispatcher>();
        return await dispatcher.DispatchAsync(query, CancellationToken.None);
    }

    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        var dispatcher = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ICommandDispatcher>();
        return await dispatcher.DispatchAsync(command, CancellationToken.None);
    }
    
    public async Task ExecuteCommandAsync(ICommand command)
    {
        var dispatcher = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ICommandDispatcher>();
        await dispatcher.DispatchAsync(command, CancellationToken.None);
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
        await usersDbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DisposeAsync()
    {
        var usersDbContext = GetAppDbContext();
        usersDbContext.Users.RemoveRange(DummyUsers.Data);
        await usersDbContext.SaveChangesAsync(CancellationToken.None);
    }
}