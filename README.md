# RealexAI - AI Sales Assistant

An intelligent sales assistant for real estate professionals that converts natural language queries into database operations, enabling quick access to sales data and customer insights.

## Features

- 🤖 **Natural Language Processing**: Ask questions in plain English
- 📊 **Database Query Generation**: Automatic conversion to SQL queries
- 💼 **Sales Intelligence**: Real-time access to customer, plot, and collection data
- 🚀 **MAUI Blazor Web**: Modern cross-platform web interface
- 📱 **Responsive Design**: Works seamlessly on desktop and mobile

## Technology Stack

- **Frontend**: .NET MAUI Blazor
- **Backend**: .NET Core
- **Database**: SQL Server / PostgreSQL
- **NLP Engine**: Integration with OpenAI or custom NLP
- **API**: RESTful API with authentication

## Project Structure

```
RealexAI/
├── RealexAI.Web/                 # MAUI Blazor Web Application
│   ├── Components/
│   ├── Pages/
│   ├── Services/
│   └── wwwroot/
├── RealexAI.API/                 # Backend API
│   ├── Controllers/
│   ├── Services/
│   ├── Models/
│   └── Data/
├── RealexAI.Core/                # Shared Business Logic
│   ├── Entities/
│   ├── Interfaces/
│   └── Services/
├── RealexAI.NLP/                 # NLP Engine
│   ├── QueryConverter/
│   └── EntityExtraction/
└── Tests/                         # Unit and Integration Tests
```

## Getting Started

### Prerequisites
- .NET 8.0 or higher
- Visual Studio 2022 or VS Code with C# extensions
- SQL Server or PostgreSQL

### Installation

1. Clone the repository
```bash
git clone https://github.com/YellumahantiKiran/RealexAI.git
cd RealexAI
```

2. Build the solution
```bash
dotnet build
```

3. Run database migrations
```bash
dotnet ef database update
```

4. Start the application
```bash
dotnet run --project RealexAI.Web
```

## Usage Examples

### Natural Language Queries
- "Show pending dues of customer ABC"
- "What plots are available in Venture X?"
- "Show customers interested in 200 sq yards plots"
- "Show today's collections"
- "List all properties sold in Q1 2026"
- "Who is the top performer this month?"

## API Endpoints

- `POST /api/query/execute` - Execute a natural language query
- `GET /api/customers` - List all customers
- `GET /api/plots` - List available plots
- `GET /api/collections/today` - Get today's collections
- `GET /api/reports/sales` - Sales reports

## Contributing

Contributions are welcome! Please follow these steps:
1. Create a feature branch
2. Commit your changes
3. Push to the branch
4. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues and questions, please create an issue on GitHub.
