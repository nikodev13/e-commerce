using ECommerce.API.Products.Categories;
using ECommerce.Application;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// adding and configuring clean architecture layers
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // seed sample data to the ECommerceDb 
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<ECommerceDbSeeder>();
    await seeder.SeedSampleData();
}

app.UseHttpsRedirection();

app.RegisterCategoryEndpoints();

app.Run();