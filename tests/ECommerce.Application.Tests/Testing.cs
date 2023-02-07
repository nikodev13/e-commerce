using System.Linq.Expressions;
using System.Windows.Input;
using ECommerce.Application.Management.Abstractions;
using ECommerce.Application.Tests.Users;
using ECommerce.Application.Tests.Utilities;
using ECommerce.Application.Users.Abstractions;
using MediatR;
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

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        var mediator = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISender>();

        return mediator.Send(request);
    }

    public IAppDbContext GetUsersDbContext() =>
        _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IAppDbContext>();
    
    public IAppDbContext GetManagementDbContext() =>
        _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IAppDbContext>();

    public void Dispose()
    {
        GetUsersDbContext().Users.RemoveRange(DummyUsers.Data);
        _factory.Dispose();
    }

    public async Task InitializeAsync()
    {
        var usersDbContext = GetUsersDbContext();
        usersDbContext.Users.AddRange(DummyUsers.Data);
        await usersDbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DisposeAsync()
    {
        var usersDbContext = GetUsersDbContext();
        usersDbContext.Users.RemoveRange(DummyUsers.Data);
        await usersDbContext.SaveChangesAsync(CancellationToken.None);
    }
}