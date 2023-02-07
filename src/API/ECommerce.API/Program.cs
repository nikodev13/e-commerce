using ECommerce.API.Configuration;
using ECommerce.API.Endpoints.Management;
using ECommerce.API.Endpoints.Users;
using ECommerce.API.Middleware;
using ECommerce.Application;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// adding and configuring clean architecture layers
builder.Services.ConfigureApplicationServices()
    .ConfigureInfrastructureServices(configuration)

    .AddScoped<IUserContextProvider, UserContextProvider>()
    .AddHttpContextAccessor()
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
app.RegisterCategoryEndpoints();
app.RegisterProductEndpoints();

app.Run();

public partial class Program { }