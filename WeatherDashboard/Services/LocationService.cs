using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public class LocationService : ILocationService
{
    private readonly ILogger<LocationService> _logger;

    public LocationService(ILogger<LocationService> logger)
    {
        _logger = logger;
    }

    public async Task<LocationDto?> GetCurrentLocationAsync()
    {
        try
        {
            // This would require browser geolocation API via JS interop
            // For now, returning a default location
            await Task.Delay(100);
            return new LocationDto
            {
                Latitude = 51.5074m,
                Longitude = -0.1278m
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting location: {ex.Message}");
            return null;
        }
    }
}