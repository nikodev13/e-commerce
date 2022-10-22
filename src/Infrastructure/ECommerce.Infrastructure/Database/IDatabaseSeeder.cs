using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Database;

public interface IDatabaseSeeder<T> where T : DbContext
{
    void Seed();
}