using ECommerce.Infrastructure.Database;
using ECommerce.Infrastructure.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        serviceCollection.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));

        return serviceCollection;
    }

}