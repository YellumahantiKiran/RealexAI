namespace WeatherDashboard.Models;

public class CurrentWeatherDto
{
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
    public decimal Temperature { get; set; }
    public decimal FeelsLike { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
    public int Humidity { get; set; }
    public int Pressure { get; set; }
    public decimal WindSpeed { get; set; }
    public int WindDirection { get; set; }
    public int Cloudiness { get; set; }
    public int UVIndex { get; set; }
    public decimal Visibility { get; set; }
    public decimal Precipitation { get; set; }
    public int RainProbability { get; set; }
    public decimal DewPoint { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime Sunrise { get; set; }
    public DateTime Sunset { get; set; }
}

public class HourlyForecastDto
{
    public DateTime Time { get; set; }
    public decimal Temperature { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
    public decimal WindSpeed { get; set; }
    public int Pressure { get; set; }
}

public class DailyForecastDto
{
    public DateTime Date { get; set; }
    public decimal MaxTemp { get; set; }
    public decimal MinTemp { get; set; }
    public decimal AvgTemp { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
    public decimal WindSpeed { get; set; }
    public int Pressure { get; set; }
    public int RainProbability { get; set; }
    public decimal Precipitation { get; set; }
}

public class CoordinatesDto
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
}

public class LocationDto
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}