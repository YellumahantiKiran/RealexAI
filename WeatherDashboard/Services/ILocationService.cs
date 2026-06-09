using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public interface ILocationService
{
    Task<LocationDto?> GetCurrentLocationAsync();
}