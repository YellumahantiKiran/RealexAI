# Weather Dashboard

A modern, real-time weather dashboard built with Blazor WebAssembly and MudBlazor. Fetches weather data from the OpenWeatherMap API.

## Features

✅ **Current Weather Display**
- Temperature, feels-like temperature
- Humidity, wind speed, pressure
- UV index, cloud coverage, visibility
- Sunrise/Sunset times
- Weather icon indicators

✅ **Hourly Forecast**
- Next 24 hours weather prediction
- Temperature, humidity, wind speed
- Weather description and icons

✅ **5-Day Forecast**
- Daily weather predictions
- Max/Min temperatures
- Humidity, wind speed, precipitation
- Rain probability

✅ **City Search**
- Auto-complete city search
- Multiple result suggestions
- Easy location switching

✅ **Current Location**
- Get weather for current device location
- Geolocation API support (requires browser permission)

✅ **Responsive Design**
- Works on desktop, tablet, and mobile
- Beautiful gradient UI
- Real-time data updates

## Technology Stack

- **Frontend Framework**: Blazor WebAssembly (.NET 8.0)
- **UI Library**: MudBlazor 6.10.0
- **Weather Data**: OpenWeatherMap API
- **Reverse Geocoding**: Nominatim (OpenStreetMap)
- **Styling**: CSS3 with gradients and animations

## Getting Started

### Prerequisites

- .NET 8.0 SDK or higher
- Visual Studio 2022 or VS Code
- Modern web browser with JavaScript enabled

### Installation

1. Navigate to the WeatherDashboard directory:
```bash
cd WeatherDashboard
```

2. Build the project:
```bash
dotnet build
```

3. Run the application:
```bash
dotnet run
```

4. Open your browser and navigate to:
```
https://localhost:7000
```

## API Configuration

### OpenWeatherMap API

The application uses the free tier of OpenWeatherMap API:
- **API Key**: 91d58efda57e186cea59a10de7f6d333 (included for demo purposes)
- **Endpoint**: https://api.openweathermap.org/data/3.0/onecall
- **Rate Limit**: 60 calls/min for free tier

**To use your own API key:**
1. Create a free account at [openweathermap.org](https://openweathermap.org/api)
2. Generate an API key
3. Replace the `ApiKey` constant in `WeatherService.cs`

### Reverse Geocoding

Used for converting coordinates to city names:
- **Service**: Nominatim (OpenStreetMap)
- **Endpoint**: https://nominatim.openstreetmap.org/reverse
- **Rate Limit**: 1 request per second

## Project Structure

```
WeatherDashboard/
├── Models/
│   └── WeatherModels.cs          # Data transfer objects
├── Services/
│   ├── IWeatherService.cs        # Weather API interface
│   ├── WeatherService.cs         # Weather API implementation
│   ├── IGeocodingService.cs      # Geocoding interface
│   ├── GeocodingService.cs       # Geocoding implementation
│   ├── ILocationService.cs       # Location service interface
│   └── LocationService.cs        # Location service implementation
├── Pages/
│   └── Index.razor               # Main dashboard page
├── MainLayout.razor              # App layout
├── App.razor                     # Root component
├── Program.cs                    # Blazor configuration
├── wwwroot/
│   ├── css/
│   │   └── app.css              # Global styles
│   └── index.html               # HTML entry point
└── WeatherDashboard.csproj      # Project file
```

## Key Components

### WeatherService
Handles all API calls to OpenWeatherMap:
- `GetCurrentWeatherAsync()` - Fetches current conditions
- `GetHourlyForecastAsync()` - Fetches 48-hour forecast
- `GetDailyForecastAsync()` - Fetches 7-day forecast

### GeocodingService
Manages location name to coordinates conversion:
- `GetCoordinatesByCityAsync()` - Single city lookup
- `SearchCitiesAsync()` - Multi-city search for autocomplete

### LocationService
Handles device geolocation:
- `GetCurrentLocationAsync()` - Gets device coordinates

## Example Usage

```csharp
// Get weather for specific coordinates
var weather = await weatherService.GetCurrentWeatherAsync(51.5074m, -0.1278m);

// Search for cities
var cities = await geocodingService.SearchCitiesAsync("London");

// Get coordinates for a city
var coords = await geocodingService.GetCoordinatesByCityAsync("London");
```

## Weather Data Points

### Current Weather
- Temperature (°C)
- Feels like temperature
- Humidity (%)
- Wind speed (m/s)
- Wind direction (°)
- Atmospheric pressure (hPa)
- Cloudiness (%)
- UV index
- Visibility (m)
- Precipitation (mm)
- Rain probability (%)
- Dew point (°C)
- Sunrise/Sunset times

## Styling

### Color Scheme
- **Primary**: Purple gradient (#667eea to #764ba2)
- **Temperature**: Red-orange gradient
- **Humidity**: Purple gradient
- **Wind**: Pink-red gradient
- **Pressure**: Blue-cyan gradient

### Responsive Breakpoints
- **Mobile**: < 576px
- **Tablet**: 576px - 768px
- **Desktop**: > 768px

## Performance Optimization

- Lazy loading of forecast data
- Efficient JSON parsing with System.Text.Json
- HTTP caching of API responses
- Minimal re-renders in Blazor
- CSS animations for smooth transitions

## Browser Support

- Chrome/Edge 90+
- Firefox 88+
- Safari 14+
- Mobile browsers (iOS Safari, Chrome Mobile)

## Error Handling

- Graceful API error handling
- User-friendly error messages via snackbar notifications
- Fallback values for missing data
- Logging for debugging

## Future Enhancements

- [ ] Weather alerts and warnings
- [ ] Air quality index (AQI)
- [ ] Historical weather data
- [ ] Multiple locations saved
- [ ] Dark mode toggle
- [ ] Push notifications for weather changes
- [ ] Weather radar integration
- [ ] Pollen forecast
- [ ] UV protection recommendations
- [ ] Weather-based activity suggestions

## Troubleshooting

### Weather data not loading
- Check API key is valid
- Verify internet connection
- Check browser console for errors
- Ensure you're not exceeding API rate limits

### Location not found
- Verify city name spelling
- Try searching with country code
- Use alternative city names (e.g., "NYC" for New York)

### Slow performance
- Clear browser cache
- Check network speed
- Disable browser extensions
- Try a different browser

## License

MIT License - See LICENSE file for details

## Support

For issues and questions, please create an issue on GitHub.
