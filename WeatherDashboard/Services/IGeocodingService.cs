using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public interface IGeocodingService
{
    Task<CoordinatesDto?> GetCoordinatesByCityAsync(string city);
    Task<List<string>?> SearchCitiesAsync(string query);
}