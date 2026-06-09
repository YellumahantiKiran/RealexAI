using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public interface IWeatherService
{
    Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude);
    Task<List<HourlyForecastDto>?> GetHourlyForecastAsync(decimal latitude, decimal longitude);
    Task<List<DailyForecastDto>?> GetDailyForecastAsync(decimal latitude, decimal longitude);
}