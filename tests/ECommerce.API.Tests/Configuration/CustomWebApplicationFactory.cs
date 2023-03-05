using ECommerce.ApplicationCore.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Tests.Configuration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove<IUserContextProvider>();
            services.AddScoped<IUserContextProvider, FakeUserContextProvider>();
            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
        });

        builder.UseEnvironment("Development");
    }
}