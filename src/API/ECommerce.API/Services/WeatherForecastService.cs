namespace ECommerce.API.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly ILogger<WeatherForecastService> _logger;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastService(ILogger<WeatherForecastService> logger)
    {
        _logger = logger;
    }
    
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("aaa");
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}