using ECommerce.Infrastructure.Database;
using ECommerce.Infrastructure.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class InfrastructureInstaller
{
    public static async Task<IServiceCollection> InstallInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionString"];

        serviceCollection.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));

        var databaseSeeder = new ECommerceDatabaseSeeder(options => options.UseSqlServer(connectionString));
        databaseSeeder.Seed();

        return serviceCollection;
    }

}