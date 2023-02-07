using IdGen;

namespace ECommerce.Domain.Shared.Services;

public interface ISnowflakeIdProvider
{
    long GenerateId();
}

public class SnowflakeIdProvider : ISnowflakeIdProvider
{
    private readonly IdGenerator _generator;

    public SnowflakeIdProvider()
    {
        
        // Let's say we take jun 13st 2022 as our epoch
        var epoch = new DateTime(2022, 6, 13, 0, 0, 0, DateTimeKind.Local);

        // Create an ID with 45 bits for timestamp, 2 for generator-id
        // and 16 for sequence
        var structure = new IdStructure(45, 2, 16);

        // Prepare options
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));

        // Create an IdGenerator with it's generator-id set to 0, our custom epoch
        // and id-structure
        
        _generator = new IdGenerator(0, options);
    }
    
    public long GenerateId()
    {
        return _generator.CreateId();
    }
}