using ECommerce.Infrastructure.Database;
using ECommerce.Infrastructure.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class InfrastructureInstaller
{
    public static IServiceCollection InstallInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionString"];

        serviceCollection.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));

        return serviceCollection;
    }

}