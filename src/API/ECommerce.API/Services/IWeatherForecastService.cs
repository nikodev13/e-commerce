namespace ECommerce.API.Services;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> Get();
}