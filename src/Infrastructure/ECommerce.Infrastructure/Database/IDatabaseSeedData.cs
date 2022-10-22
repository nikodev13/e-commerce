namespace ECommerce.Infrastructure.Database;

public interface IDatabaseSeedData<T> where T : class
{
    ICollection<T> GetEntityData();
}