using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Persistence;

public class DatabaseFixture : IDisposable
{
    private readonly string _connectionString = "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;Trust Server Certificate=true;";

    public ApplicationDbContext ApplicationDbContext { get; }
    
    public DatabaseFixture()
    {
        ApplicationDbContext = new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connectionString).Options);
    }
    
    public void Dispose()
    {
        ApplicationDbContext.Dispose();
    }
}