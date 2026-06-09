using System.Text.Json;
using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherService> _logger;
    private const string ApiKey = "91d58efda57e186cea59a10de7f6d333"; // Free OpenWeatherMap API key
    private const string BaseUrl = "https://api.openweathermap.org/data/3.0/onecall";
    private const string GeocodingBaseUrl = "https://api.openweathermap.org/geo/1.0/direct";

    public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(decimal latitude, decimal longitude)
    {
        try
        {
            var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric&exclude=minutely";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Weather API error: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var current = root.GetProperty("current");
            var timezone = root.GetProperty("timezone").GetString() ?? "UTC";

            var weather = new CurrentWeatherDto
            {
                Description = current.GetProperty("weather")[0].GetProperty("main").GetString() ?? "",
                Icon = current.GetProperty("weather")[0].GetProperty("icon").GetString() ?? "",
                Temperature = (decimal)current.GetProperty("temp").GetDouble(),
                FeelsLike = (decimal)current.GetProperty("feels_like").GetDouble(),
                Humidity = current.GetProperty("humidity").GetInt32(),
                Pressure = current.GetProperty("pressure").GetInt32(),
                WindSpeed = (decimal)current.GetProperty("wind_speed").GetDouble(),
                WindDirection = current.TryGetProperty("wind_deg", out var windDeg) ? windDeg.GetInt32() : 0,
                Cloudiness = current.GetProperty("clouds").GetInt32(),
                UVIndex = (int)current.GetProperty("uvi").GetDouble(),
                Visibility = current.GetProperty("visibility").GetInt32(),
                DewPoint = (decimal)current.GetProperty("dew_point").GetDouble(),
                Latitude = latitude,
                Longitude = longitude,
                Sunrise = UnixTimeStampToDateTime(current.GetProperty("sunrise").GetInt64()),
                Sunset = UnixTimeStampToDateTime(current.GetProperty("sunset").GetInt64()),
            };

            // Get city and country from geocoding
            var geocode = await GetCityFromCoordinatesAsync(latitude, longitude);
            if (geocode != null)
            {
                weather.City = geocode.City;
                weather.Country = geocode.Country;
            }

            return weather;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching current weather: {ex.Message}");
            return null;
        }
    }

    public async Task<List<HourlyForecastDto>?> GetHourlyForecastAsync(decimal latitude, decimal longitude)
    {
        try
        {
            var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric&exclude=minutely,daily";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var hourly = new List<HourlyForecastDto>();
            var hourlyData = root.GetProperty("hourly");

            foreach (var item in hourlyData.EnumerateArray().Take(48))
            {
                hourly.Add(new HourlyForecastDto
                {
                    Time = UnixTimeStampToDateTime(item.GetProperty("dt").GetInt64()),
                    Temperature = (decimal)item.GetProperty("temp").GetDouble(),
                    Humidity = item.GetProperty("humidity").GetInt32(),
                    Description = item.GetProperty("weather")[0].GetProperty("main").GetString() ?? "",
                    Icon = item.GetProperty("weather")[0].GetProperty("icon").GetString() ?? "",
                    WindSpeed = (decimal)item.GetProperty("wind_speed").GetDouble(),
                    Pressure = item.GetProperty("pressure").GetInt32(),
                });
            }

            return hourly;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching hourly forecast: {ex.Message}");
            return null;
        }
    }

    public async Task<List<DailyForecastDto>?> GetDailyForecastAsync(decimal latitude, decimal longitude)
    {
        try
        {
            var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric&exclude=minutely,hourly";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var daily = new List<DailyForecastDto>();
            var dailyData = root.GetProperty("daily");

            foreach (var item in dailyData.EnumerateArray().Take(7))
            {
                daily.Add(new DailyForecastDto
                {
                    Date = UnixTimeStampToDateTime(item.GetProperty("dt").GetInt64()),
                    MaxTemp = (decimal)item.GetProperty("temp").GetProperty("max").GetDouble(),
                    MinTemp = (decimal)item.GetProperty("temp").GetProperty("min").GetDouble(),
                    AvgTemp = (decimal)item.GetProperty("temp").GetProperty("day").GetDouble(),
                    Humidity = item.GetProperty("humidity").GetInt32(),
                    Description = item.GetProperty("weather")[0].GetProperty("main").GetString() ?? "",
                    Icon = item.GetProperty("weather")[0].GetProperty("icon").GetString() ?? "",
                    WindSpeed = (decimal)item.GetProperty("wind_speed").GetDouble(),
                    Pressure = item.GetProperty("pressure").GetInt32(),
                    RainProbability = (int)(item.GetProperty("pop").GetDouble() * 100),
                    Precipitation = item.TryGetProperty("rain", out var rain) ? (decimal)rain.GetDouble() : 0,
                });
            }

            return daily;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching daily forecast: {ex.Message}");
            return null;
        }
    }

    private async Task<CoordinatesDto?> GetCityFromCoordinatesAsync(decimal latitude, decimal longitude)
    {
        try
        {
            var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            var address = root.GetProperty("address");

            var city = address.TryGetProperty("city", out var cityProp) ? cityProp.GetString() : 
                       address.TryGetProperty("town", out var townProp) ? townProp.GetString() :
                       address.TryGetProperty("village", out var villageProp) ? villageProp.GetString() : "Unknown";

            var country = address.TryGetProperty("country", out var countryProp) ? countryProp.GetString() : "Unknown";

            return new CoordinatesDto
            {
                Latitude = latitude,
                Longitude = longitude,
                City = city ?? "Unknown",
                Country = country ?? "Unknown"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting city from coordinates: {ex.Message}");
            return null;
        }
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}