using ECommerce.API.Configuration;
using ECommerce.API.Endpoints;
using ECommerce.API.Middleware;
using ECommerce.Application;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// adding and configuring clean architecture layers
services.ConfigureApplicationServices();
services.ConfigureInfrastructureServices(configuration);

services.AddScoped<IUserContextService, UserContextService>();
services.AddHttpContextAccessor();
services.AddScoped<ExceptionMiddleware>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // seed sample data to the ECommerceDb 
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>();
    seeder.SeedSampleData();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.RegisterCategoryEndpoints();
app.RegisterProductEndpoints();

app.RegisterUserEndpoints();

app.Run();