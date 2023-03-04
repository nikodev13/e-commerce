namespace ECommerce.ApplicationCore.Shared.Abstractions;

public interface ISnowflakeIdProvider
{
    long GenerateId();
}