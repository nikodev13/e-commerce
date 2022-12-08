using Microsoft.Extensions.Logging;
using Moq;

namespace ECommerce.Application.Tests.Services;

public static class LoggerMock
{
    public static Mock<ILogger<T>> GetLogger<T>()
    {
        return new Mock<ILogger<T>>();
    }
}