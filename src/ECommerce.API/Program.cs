using ECommerce.API.Configuration;
using ECommerce.API.Endpoints;
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

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.RegisterUserEndpoints();

app.RegisterProductsEndpoints();
app.RegisterCategoriesEndpoints();

app.RegisterOrderEndpoints();
app.RegisterCustomerAccountEndpoints();

app.Run();

public partial class Program { }