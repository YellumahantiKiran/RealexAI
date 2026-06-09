# MAUI Blazor Web - AI Sales Assistant Guide

## What is MAUI Blazor Web?

MAUI stands for **.NET Multi-platform App UI**. Blazor is a framework for building interactive web applications with C#. MAUI Web Blazor combines both to create:

- **Single codebase** for multiple platforms
- **C# instead of JavaScript** for frontend logic
- **Component-based architecture**
- **Hot reload support** during development
- **Full .NET runtime** capabilities

## Project Overview

This AI Sales Assistant helps real estate sales executives:

1. **Ask natural language questions**
   - "Show pending dues of customer ABC"
   - "What plots are available in Venture X?"
   - "Show customers interested in 200 sq yards plots"
   - "Show today's collections"

2. **Get instant database queries**
   - AI converts questions to SQL
   - Executes queries automatically
   - Returns formatted results

3. **Manage sales data**
   - Customer information
   - Property/plot details
   - Payment collections
   - Sales analytics

## Technology Stack

```
Frontend:
├── .NET MAUI Blazor Web
├── MudBlazor Components
├── C# with Razor syntax
└── CSS3 styling

Backend:
├── ASP.NET Core 8.0
├── Entity Framework Core
├── SQL Server / PostgreSQL
└── OpenAI/Custom NLP

Database:
├── Customers table
├── Plots table
├── Collections table
└── CustomerPlotInterests table
```

## Project Structure

```
RealExAI.SalesAssistant/
│
├── RealExAI.Web/                    # MAUI Blazor Web Frontend
│   ├── Components/
│   │   ├── Layout/
│   │   │   ├── MainLayout.razor
│   │   │   └── NavMenu.razor
│   │   ├── Shared/
│   │   │   ├── WeatherCard.razor
│   │   │   ├── MetricsCard.razor
│   │   │   └── LoadingSpinner.razor
│   │   └── Pages/
│   │       ├── Dashboard.razor
│   │       ├── QueryAssistant.razor
│   │       ├── Customers.razor
│   │       ├── Plots.razor
│   │       └── Collections.razor
│   │
│   ├── Services/
│   │   ├── IQueryService.cs
│   │   ├── QueryService.cs
│   │   ├── ICustomerService.cs
│   │   ├── CustomerService.cs
│   │   ├── IPlotService.cs
│   │   ├── PlotService.cs
│   │   ├── ICollectionService.cs
│   │   ├── CollectionService.cs
│   │   ├── INLPService.cs
│   │   └── NLPService.cs
│   │
│   ├── Models/
│   │   ├── CustomerDTO.cs
│   │   ├── PlotDTO.cs
│   │   ├── CollectionDTO.cs
│   │   └── QueryResultDTO.cs
│   │
│   ├── wwwroot/
│   │   ├── css/
│   │   │   ├── app.css
│   │   │   ├── colors.css
│   │   │   ├── animations.css
│   │   │   └── responsive.css
│   │   ├── js/
│   │   │   └── interop.js
│   │   └── index.html
│   │
│   ├── App.razor
│   ├── MainLayout.razor
│   ├── Program.cs
│   ├── appsettings.json
│   └── RealExAI.Web.csproj
│
├── RealExAI.API/                    # Backend API
│   ├── Controllers/
│   │   ├── QueryController.cs
│   │   ├── CustomersController.cs
│   │   ├── PlotsController.cs
│   │   └── CollectionsController.cs
│   │
│   ├── Services/
│   │   ├── QueryService.cs
│   │   ├── NLPService.cs
│   │   ├── CustomerService.cs
│   │   ├── PlotService.cs
│   │   └── CollectionService.cs
│   │
│   ├── Data/
│   │   ├── Models/
│   │   │   ├── Customer.cs
│   │   │   ├── Plot.cs
│   │   │   ├── Collection.cs
│   │   │   └── CustomerPlotInterest.cs
│   │   └── RealExAIDbContext.cs
│   │
│   ├── Migrations/
│   ├── Program.cs
│   ├── appsettings.json
│   └── RealExAI.API.csproj
│
├── RealExAI.Core/                   # Shared Models & Interfaces
│   ├── Entities/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Constants/
│
└── Documentation/
    ├── SETUP.md
    ├── API-REFERENCE.md
    ├── USAGE-GUIDE.md
    └── EXAMPLES.md
```

## Installation & Setup

### Prerequisites

```bash
# Required software
- .NET 8.0 SDK (https://dotnet.microsoft.com/download)
- Visual Studio 2022 / VS Code
- SQL Server LocalDB or PostgreSQL
- Git
```

### Step 1: Clone Repository

```bash
git clone https://github.com/YellumahantiKiran/RealExAI-SalesAssistant.git
cd RealExAI-SalesAssistant
```

### Step 2: Install Dependencies

```bash
# Restore NuGet packages
dotnet restore
```

### Step 3: Configure Database

**Edit `RealExAI.API/appsettings.json`:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RealExAI;Trusted_Connection=true;"
  },
  "OpenAI": {
    "ApiKey": "your-api-key-here",
    "Model": "gpt-3.5-turbo"
  }
}
```

### Step 4: Create Database

```bash
cd RealExAI.API

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration
dotnet ef database update

cd ..
```

### Step 5: Run Application

**Terminal 1 - API:**
```bash
cd RealExAI.API
dotnet run
# API runs on https://localhost:7001
```

**Terminal 2 - Web App:**
```bash
cd RealExAI.Web
dotnet run
# Web runs on https://localhost:7000
```

## MAUI Blazor Web Components

### Component Example: Query Assistant

```razor
@page "/query"
@using MudBlazor
@inject IQueryService QueryService
@inject ISnackbar Snackbar

<PageTitle>Query Assistant</PageTitle>

<MudContainer MaxWidth="MaxWidth.Lg" Class="py-8">
    <MudText Typo="Typo.h3" Class="mb-4">AI Query Assistant</MudText>
    
    <!-- Search Input -->
    <MudCard Class="mb-4">
        <MudCardContent>
            <MudStack Spacing="3">
                <MudTextField 
                    @bind-Value="userQuery" 
                    Label="Ask a question"
                    FullWidth="true"
                    Lines="3"
                    Placeholder="e.g., Show pending dues of customer ABC"
                    Variant="Variant.Outlined"
                />
                
                <MudButton 
                    Variant="Variant.Filled" 
                    Color="Color.Primary"
                    OnClick="ExecuteQuery"
                    StartIcon="@Icons.Material.Filled.Send"
                >
                    Execute Query
                </MudButton>
            </MudStack>
        </MudCardContent>
    </MudCard>
    
    <!-- Results Display -->
    @if (queryResults != null)
    {
        <MudCard>
            <MudCardHeader>
                <MudText Typo="Typo.h6">Results</MudText>
            </MudCardHeader>
            <MudCardContent>
                <MudTable Items="queryResults" Striped="true" Hover="true">
                    <HeaderContent>
                        @foreach (var column in GetColumns())
                        {
                            <MudTh>@column</MudTh>
                        }
                    </HeaderContent>
                    <RowTemplate>
                        @foreach (var value in GetRowValues(context))
                        {
                            <MudTd>@value</MudTd>
                        }
                    </RowTemplate>
                </MudTable>
            </MudCardContent>
        </MudCard>
    }
</MudContainer>

@code {
    private string userQuery = "";
    private List<Dictionary<string, object>>? queryResults;

    private async Task ExecuteQuery()
    {
        if (string.IsNullOrEmpty(userQuery))
            return;

        try
        {
            var result = await QueryService.ExecuteNaturalLanguageQuery(userQuery);
            queryResults = result.Results;
            Snackbar.Add("Query executed successfully", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
    }

    private List<string> GetColumns()
    {
        return queryResults?.FirstOrDefault()?.Keys.ToList() ?? new();
    }

    private List<object?> GetRowValues(Dictionary<string, object> row)
    {
        return row.Values.Cast<object?>().ToList();
    }
}
```

### Service Example: Query Service

```csharp
public interface IQueryService
{
    Task<QueryResultDTO> ExecuteNaturalLanguageQuery(string query);
}

public class QueryService : IQueryService
{
    private readonly HttpClient _httpClient;

    public QueryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<QueryResultDTO> ExecuteNaturalLanguageQuery(string query)
    {
        try
        {
            var request = new { query };
            var response = await _httpClient.PostAsJsonAsync("/api/query/execute", request);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<QueryResultDTO>() 
                    ?? new QueryResultDTO { Success = false };
            }
            return new QueryResultDTO { Success = false, ErrorMessage = "API Error" };
        }
        catch (Exception ex)
        {
            return new QueryResultDTO { Success = false, ErrorMessage = ex.Message };
        }
    }
}
```

## NLP Query Processing

### How It Works

1. **User Input**: "Show pending dues of customer ABC"
2. **NLP Processing**: AI analyzes the query
3. **SQL Generation**: Converts to: `SELECT c.Name, c.PendingDues FROM Customers WHERE PendingDues > 0`
4. **Database Query**: Executes the generated SQL
5. **Results Display**: Shows formatted results in table

### Supported Query Patterns

```
1. Pending Dues:
   "Show pending dues of customer ABC"
   "List customers with outstanding payments"
   "Who owes money?"

2. Plot Availability:
   "What plots are available in Venture X?"
   "Show available properties"
   "List empty plots"

3. Customer Interests:
   "Show customers interested in 200 sq yards plots"
   "Who wants large plots?"
   "List buyers for premium properties"

4. Collections:
   "Show today's collections"
   "How much did we collect today?"
   "List today's payments"

5. Analytics:
   "Top performing sales executives"
   "Total sales this month"
   "Customer acquisition by region"
```

## API Endpoints

### Query API
```
POST /api/query/execute
Body: { "query": "Show pending dues of customer ABC" }
Response: { "sql": "...", "results": [...], "success": true }
```

### Customer API
```
GET /api/customers
GET /api/customers/{id}
GET /api/customers?search=name
POST /api/customers
PUT /api/customers/{id}
DELETE /api/customers/{id}
```

### Plot API
```
GET /api/plots
GET /api/plots?venture=VentureName
GET /api/plots/{id}
POST /api/plots
PUT /api/plots/{id}
DELETE /api/plots/{id}
```

### Collection API
```
GET /api/collections/today
GET /api/collections?from=DATE&to=DATE
GET /api/collections/{id}
POST /api/collections
PUT /api/collections/{id}
DELETE /api/collections/{id}
```

## Key Features Implementation

### 1. Dashboard
- Real-time metrics (customers, plots, collections, dues)
- Quick action buttons
- Recent activity feed

### 2. Query Assistant
- Natural language input
- Auto-complete suggestions
- Generated SQL display
- Results in tables/charts

### 3. Customer Management
- View all customers
- Search functionality
- Edit customer details
- Track pending dues

### 4. Plot Management
- View available plots
- Filter by venture/size
- Track plot status
- Customer interest mapping

### 5. Collections Tracking
- Today's collections
- Date range reports
- Payment mode tracking
- Collection analytics

## Styling with MudBlazor

### Available Components

```csharp
// Layout
MudAppBar, MudDrawer, MudContainer, MudGrid, MudItem

// Forms
MudTextField, MudAutoComplete, MudSelect, MudButton

// Display
MudText, MudCard, MudTable, MudList, MudChip

// Feedback
MudSnackbar, MudDialog, MudAlert, MudProgressLinear

// Icons
MudIcon (@Icons.Material.Filled.*)

// Colors
Color.Primary, Color.Success, Color.Warning, Color.Error

// Typography
Typo.h1, Typo.h3, Typo.body1, Typo.body2, Typo.caption
```

## Best Practices

### 1. Component Structure
```razor
@page "/page"
@using YourNamespace
@inject IService Service

<PageTitle>Page Title</PageTitle>

<ChildComponent Parameter="value" />

@code {
    private string property = "";
    
    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }
    
    private async Task LoadData() { }
}
```

### 2. Error Handling
```csharp
try
{
    var result = await Service.GetDataAsync();
    Snackbar.Add("Success", Severity.Success);
}
catch (Exception ex)
{
    Logger.LogError(ex, "Error");
    Snackbar.Add($"Error: {ex.Message}", Severity.Error);
}
```

### 3. Responsive Design
```razor
<MudGrid Spacing="2">
    <MudItem xs="12" sm="6" md="4">Content</MudItem>
    <MudItem xs="12" sm="6" md="4">Content</MudItem>
    <MudItem xs="12" sm="6" md="4">Content</MudItem>
</MudGrid>
```

## Common Queries

```sql
-- 1. Pending Dues
SELECT c.Id, c.Name, c.Email, c.PendingDues
FROM Customers c
WHERE c.PendingDues > 0
ORDER BY c.PendingDues DESC;

-- 2. Available Plots
SELECT p.Id, p.PlotNumber, p.VentureName, p.SizeInSqYards, p.Price
FROM Plots p
WHERE p.Status = 'Available'
ORDER BY p.SizeInSqYards DESC;

-- 3. Customer Interests
SELECT DISTINCT c.Id, c.Name, c.Email, p.PlotNumber, p.SizeInSqYards
FROM Customers c
JOIN CustomerPlotInterests cpi ON c.Id = cpi.CustomerId
JOIN Plots p ON cpi.PlotId = p.Id
WHERE p.SizeInSqYards = 200;

-- 4. Today's Collections
SELECT c.Name, col.Amount, col.PaymentMode, col.CollectionDate
FROM Collections col
JOIN Customers c ON col.CustomerId = c.Id
WHERE CAST(col.CollectionDate AS DATE) = CAST(GETDATE() AS DATE);
```

## Debugging Tips

### Enable Logging
```csharp
builder.Logging.SetMinimumLevel(LogLevel.Debug);
```

### Browser DevTools
- F12 to open
- Console tab for errors
- Network tab for API calls
- Elements tab to inspect components

### Breakpoints
```csharp
System.Diagnostics.Debugger.Break();
```

## Performance Optimization

1. **Lazy Loading**: Load components on demand
2. **Caching**: Cache API responses
3. **Virtualization**: Virtual lists for large datasets
4. **Minification**: Minify CSS/JS in production
5. **Bundle Size**: Use tree shaking

## Deployment

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "RealExAI.Web.dll"]
```

### Azure App Service
```bash
az webapp create --resource-group myGroup --plan myPlan --name myApp --runtime DOTNETCORE:8
az webapp deployment source config-zip -g myGroup -n myApp --src app.zip
```

## Additional Resources

- [MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [MudBlazor Components](https://mudblazor.com/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)

## Support & Troubleshooting

### Common Issues

**Issue**: API not responding
```bash
# Check if API is running
netstat -ano | findstr :7001  # Windows
lsof -i :7001                  # Mac/Linux
```

**Issue**: Database connection error
```bash
# Verify connection string
# Check database exists
# Re-run migrations
dotnet ef database update
```

**Issue**: NLP not working
```bash
# Verify OpenAI API key
# Check API quota
# Review query patterns
```

## License

MIT License - See LICENSE file for details

## Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

---

**Repository**: https://github.com/YellumahantiKiran/RealExAI-SalesAssistant

**Last Updated**: June 2026
