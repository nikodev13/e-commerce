using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ECommerce.Infrastructure.Logging;

internal static class SerilogConfiguration
{
    public static void ConfigureSerilog(this IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            var logTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
            builder.ClearProviders();
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen,
                    outputTemplate: logTemplate)
                .WriteTo.File(path: "logs/logs",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: logTemplate)
                .WriteTo.File(path: "logs/errors",
                    rollingInterval: RollingInterval.Day, 
                    restrictedToMinimumLevel: LogEventLevel.Error,
                    outputTemplate: logTemplate)
                .CreateLogger();
            builder.AddSerilog(logger);
        });
    }
}