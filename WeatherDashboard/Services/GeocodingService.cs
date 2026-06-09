using System.Text.Json;
using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeocodingService> _logger;
    private const string ApiKey = "91d58efda57e186cea59a10de7f6d333";
    private const string BaseUrl = "https://api.openweathermap.org/geo/1.0/direct";

    public GeocodingService(HttpClient httpClient, ILogger<GeocodingService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CoordinatesDto?> GetCoordinatesByCityAsync(string city)
    {
        try
        {
            var url = $"{BaseUrl}?q={Uri.EscapeDataString(city)}&limit=1&appid={ApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Geocoding API error: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            if (root.GetArrayLength() == 0)
                return null;

            var result = root[0];
            return new CoordinatesDto
            {
                Latitude = (decimal)result.GetProperty("lat").GetDouble(),
                Longitude = (decimal)result.GetProperty("lon").GetDouble(),
                City = result.GetProperty("name").GetString() ?? city,
                Country = result.TryGetProperty("country", out var countryProp) ? countryProp.GetString() ?? "" : ""
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting coordinates: {ex.Message}");
            return null;
        }
    }

    public async Task<List<string>?> SearchCitiesAsync(string query)
    {
        try
        {
            var url = $"{BaseUrl}?q={Uri.EscapeDataString(query)}&limit=10&appid={ApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var cities = new List<string>();
            foreach (var item in root.EnumerateArray())
            {
                var name = item.GetProperty("name").GetString();
                var country = item.TryGetProperty("country", out var countryProp) ? countryProp.GetString() : "";
                var state = item.TryGetProperty("state", out var stateProp) ? stateProp.GetString() : "";

                var displayName = string.IsNullOrEmpty(state) 
                    ? $"{name}, {country}"
                    : $"{name}, {state}, {country}";

                cities.Add(displayName);
            }

            return cities;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching cities: {ex.Message}");
            return null;
        }
    }
}