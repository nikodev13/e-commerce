using ECommerce.Application.Common.Services;
using ECommerce.Application.Tests.Persistence;
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

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}