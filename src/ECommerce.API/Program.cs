using ECommerce.API.Configuration;
using ECommerce.API.Endpoints;
using ECommerce.API.Endpoints.Customers;
using ECommerce.API.Endpoints.Management;
using ECommerce.API.Endpoints.Users;
using ECommerce.API.Middleware;
using ECommerce.ApplicationCore;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// adding and configuring clean architecture layers
builder.Services.ConfigureApplicationServices()
    .Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true)
    .ConfigureInfrastructureServices(configuration)
    
    .AddHttpContextAccessor()
    .AddScoped<IUserContextProvider, UserContextProvider>()
    .AddScoped<ExceptionMiddleware>()

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
}

app.UseAuthentication();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.RegisterUserEndpoints();
app.RegisterCustomerAccountEndpoints();
app.RegisterCustomerAddressEndpoints();
app.RegisterCategoryEndpoints();
app.RegisterProductEndpoints();

app.Run();

public partial class Program { }