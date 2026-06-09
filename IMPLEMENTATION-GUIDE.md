# MAUI Blazor Web - Complete Implementation Guide

## Part 1: Project Structure & Dependencies

### Frontend: RealExAI.Web
Key NuGet packages:
- Microsoft.AspNetCore.Components.WebAssembly (8.0.0)
- MudBlazor (6.10.0)
- System.Net.Http.Json (8.0.0)

### Backend: RealExAI.API
Key NuGet packages:
- Microsoft.AspNetCore.OpenApi (8.0.0)
- Swashbuckle.AspNetCore (6.4.0)
- Microsoft.EntityFrameworkCore (8.0.0)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.0)

## Part 2: Service Layer

### IQueryService Implementation
- ExecuteNaturalLanguageQuery(string query)
- Handles HTTP communication with backend
- Returns QueryResultDTO with SQL, results, and status
- Error handling and logging

### INLPService Implementation
- ConvertQueryToSqlAsync(string query)
- Pattern matching for natural language queries
- Supported patterns:
  1. Pending dues queries
  2. Available plots queries
  3. Customer interests queries
  4. Collections queries

### ICustomerService Implementation
- GetCustomersAsync, GetCustomerByIdAsync
- CreateCustomerAsync, UpdateCustomerAsync, DeleteCustomerAsync
- Search functionality

## Part 3: Components

### QueryAssistant.razor Component
- Natural language input field
- Execute and Clear buttons
- Generated SQL display
- Results table with MudDataGrid
- Loading states and error handling
- Copy SQL and Export results functionality

## Part 4: Database Models

### Customer
- Id, Name, Email, Phone, City
- TotalInvested, PendingDues, IsActive
- Navigation: Collections, PlotInterests

### Plot
- Id, VentureName, PlotNumber
- SizeInSqYards, Status, Price, Location
- Navigation: CustomerInterests

### Collection
- Id, CustomerId, Amount, CollectionDate
- PaymentMode, ReferenceNumber, Notes
- Foreign Key: Customer

### CustomerPlotInterest
- Id, CustomerId, PlotId, InterestDate

## Part 5: API Endpoints

### Query Controller
POST /api/query/execute
Request: { "query": "natural language question" }
Response: { "generatedSql": "...", "results": [...], "success": true }

### Customer Controller
GET /api/customers - Get all customers
GET /api/customers/{id} - Get specific customer
POST /api/customers - Create customer
PUT /api/customers/{id} - Update customer
DELETE /api/customers/{id} - Delete customer

## Part 6: Running the Application

### Step 1: Database Setup
dotnet ef migrations add InitialCreate
dotnet ef database update

### Step 2: Start Backend API
cd RealExAI.API
dotnet run
Runs on: https://localhost:7001

### Step 3: Start Frontend
cd RealExAI.Web
dotnet run
Runs on: https://localhost:7000

### Step 4: Access Application
Open https://localhost:7000 in browser

## Part 7: Supported Queries

Query 1: Show pending dues of customer ABC
- Finds all customers with pending payments
- Sorted by amount descending

Query 2: What plots are available in Venture X?
- Lists all available plots
- Filterable by venture name
- Shows size, price, and location

Query 3: Show customers interested in 200 sq yards plots
- Joins customer and plot interest tables
- Filters by specific size requirements

Query 4: Show today's collections
- Displays collections received today
- Shows payment methods and amounts

## Part 8: Key Features

1. Natural Language Processing
   - Pattern-based query recognition
   - Dynamic SQL generation
   - Support for multiple query types

2. User Interface
   - Responsive MudBlazor components
   - Real-time query execution
   - Results visualization
   - Error notifications

3. Data Management
   - CRUD operations for customers
   - Customer-plot interest tracking
   - Collection history management
   - Venture and plot management

4. Database Design
   - Normalized schema with relationships
   - Cascading delete for data integrity
   - DateTime tracking for audits
   - Status-based filtering

## Part 9: Debugging

Enable Debug Logging:
- Set LogLevel.Debug in appsettings.json
- Check Visual Studio Output window
- Browser console for client errors

Common Issues:
1. Database not found: Check connection string and SQL Server instance
2. API connection failed: Verify API running on port 7001
3. Component not rendering: Check Blazor script in index.html
4. Query returns empty: Verify database has sample data

## Part 10: Configuration

appsettings.json settings:
- ConnectionStrings.DefaultConnection: SQL Server connection
- Logging levels: Set to Debug for development
- CORS settings: Allow frontend origin
- API settings: Base URL, timeouts

## Part 11: Next Steps

1. Add authentication/authorization
2. Implement real-time notifications
3. Add data export (PDF/Excel)
4. Create analytics dashboard
5. Add advanced search filters
6. Implement caching strategy
7. Create mobile app with MAUI

## Resources

- MAUI: https://learn.microsoft.com/dotnet/maui/
- Blazor: https://learn.microsoft.com/aspnet/core/blazor/
- MudBlazor: https://mudblazor.com/
- Entity Framework: https://learn.microsoft.com/ef/core/
- ASP.NET Core: https://learn.microsoft.com/aspnet/core/

Happy coding with your AI Sales Assistant!