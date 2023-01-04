using ECommerce.Domain.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Seeders;

public interface IEntitySeedDataProvider<out TEntity>
{
    IEnumerable<TEntity> GetData();
}

public class ApplicationDbSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISnowflakeIdService _idService;

    public ApplicationDbSeeder(ApplicationDbContext dbContext, ISnowflakeIdService idService)
    {
        _dbContext = dbContext;
        _idService = idService;
    }
    
    public void SeedSampleData()
    {
        _dbContext.SeedProductContextSampleData();
    }    
}