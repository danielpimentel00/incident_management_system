# Technical Documentation - Incident Management System

## Table of Contents
- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)
- [Design Patterns](#design-patterns)
- [Layer Descriptions](#layer-descriptions)
- [Database Schema](#database-schema)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Development Guidelines](#development-guidelines)

---

## Architecture Overview

This project follows **Clean Architecture** (also known as Onion Architecture or Hexagonal Architecture) principles, ensuring separation of concerns, testability, and maintainability.

### Core Principles

1. **Dependency Rule**: Dependencies point inward. Inner layers know nothing about outer layers.
2. **Independence**: Business logic is independent of frameworks, UI, and databases.
3. **Testability**: Business rules can be tested without external dependencies.
4. **Framework Agnostic**: The core business logic doesn't depend on any specific framework.

### Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                       │
│              (incident_management_system.API)                │
│  - Controllers/Endpoints                                     │
│  - Middleware                                                │
│  - Exception Handlers                                        │
│  - Dependency Injection Setup                                │
└───────────────────────────┬─────────────────────────────────┘
                            │ depends on
┌───────────────────────────▼─────────────────────────────────┐
│                     Application Layer                        │
│                    (IMS.Application)                         │
│  - Use Cases (Command/Query Handlers)                        │
│  - DTOs & View Models                                        │
│  - Validators (FluentValidation)                             │
│  - Behaviors (Logging, Validation)                           │
│  - Interface Definitions                                     │
└───────────────────────────┬─────────────────────────────────┘
                            │ depends on
┌───────────────────────────▼─────────────────────────────────┐
│                      Domain Layer                            │
│                     (IMS.Domain)                             │
│  - Entities (Business Models)                                │
│  - Value Objects                                             │
│  - Enums                                                     │
│  - Domain Events (future)                                    │
│  - Business Rules                                            │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                        │
│                   (IMS.Persistance)                          │
│  - DbContext (Entity Framework Core)                         │
│  - Repositories                                              │
│  - Migrations                                                │
│  - Database Configurations                                   │
└─────────────────────────────────────────────────────────────┘
        ▲
        │ implements interfaces from Application Layer
```

---

## Technology Stack

### Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| **.NET** | 10.0 | Runtime framework |
| **C#** | 14.0 | Programming language |
| **ASP.NET Core** | 10.0 | Web API framework |
| **Entity Framework Core** | 10.0.2 | ORM for database access |
| **PostgreSQL** | Latest | Relational database |
| **MediatR** | 14.0.0 | CQRS and Mediator pattern |
| **FluentValidation** | 12.1.1 | Input validation |

### Additional Libraries

- **Npgsql.EntityFrameworkCore.PostgreSQL** (10.0.0) - PostgreSQL provider for EF Core
- **Swashbuckle/Swagger** - API documentation
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT authentication

---

## Design Patterns

### 1. **CQRS (Command Query Responsibility Segregation)**

Separates read and write operations using MediatR:

```csharp
// Command (Write)
public record CreateIncidentCommand(string Title, string Description, int CreatedByUserId) 
    : IRequest<CreatedIncident>;

// Query (Read)
public record GetIncidentByIdQuery(int Id) 
    : IRequest<IncidentDetails?>;
```

### 2. **Repository Pattern**

Abstracts data access logic:

```csharp
public interface IIncidentsRepository
{
    Task<(List<Incident>, int)> GetAllIncidentsAsync(int pageNumber, int pageCount);
    Task<Incident?> GetIncidentByIdAsync(int id);
    Task<Incident> CreateIncidentAsync(Incident incident);
    // ... more methods
}
```

### 3. **Pipeline Behavior Pattern**

Cross-cutting concerns using MediatR behaviors:

- **ValidationBehavior**: Validates requests using FluentValidation
- **LoggingBehavior**: Logs request execution time and errors

### 4. **Dependency Injection**

Service registration using extension methods:

```csharp
builder.Services.AddApplicationServices();     // Application layer
builder.Services.AddPersistanceServices(config); // Infrastructure layer
```

### 5. **Minimal API Pattern**

Endpoint organization using `IEndpoint` interface:

```csharp
public interface IEndpoint
{
    void MapEndpoints(IEndpointRouteBuilder routes);
}
```

---

## Layer Descriptions

### 1. Domain Layer (IMS.Domain)

**Responsibility**: Contains core business logic and entities.

**Dependencies**: None (pure .NET)

**Key Components**:
- **Entities**: `Incident`, `User`, `IncidentComment`
- **Enums**: `IncidentStatus` (Open, Resolved)
- **Value Objects**: (Future implementation)

**Principles**:
- No dependencies on other layers
- Contains only business rules
- Framework-agnostic

---

### 2. Application Layer (IMS.Application)

**Responsibility**: Contains application business rules and use cases.

**Dependencies**: 
- IMS.Domain
- MediatR
- FluentValidation

**Key Components**:

#### Features (Vertical Slice Architecture)
```
Features/
├── Incidents/
│   ├── Commands/
│   │   ├── CreateIncident/
│   │   │   ├── CreateIncidentCommand.cs
│   │   │   ├── CreateIncidentCommandHandler.cs
│   │   │   └── CreateIncidentCommandValidator.cs
│   │   └── ...
│   └── Queries/
│       └── ...
```

#### Behaviors (Cross-cutting Concerns)

**ValidationBehavior**:
- Automatically validates all requests
- Uses FluentValidation validators
- Throws `ValidationException` on failure

**LoggingBehavior**:
- Logs request start and completion
- Tracks execution time
- Logs errors with context

#### Interface Definitions
- `IIncidentsRepository`
- `IUsersRepository`

---

### 3. Infrastructure Layer (IMS.Persistance)

**Responsibility**: Implements data access and external services.

**Dependencies**:
- IMS.Domain
- IMS.Application (for interfaces)
- Entity Framework Core
- Npgsql

**Key Components**:

#### DbContext
```csharp
public class IncidentDbContext : DbContext
{
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Entity configurations
    }
}
```

#### Repositories

**IncidentsRepository**:
- Implements `IIncidentsRepository`
- Handles CRUD operations for incidents
- Uses EF Core features:
  - `Include()` / `ThenInclude()` for eager loading
  - `AsNoTracking()` for read-only queries
  - `AsSplitQuery()` for performance optimization

**UsersRepository**:
- Implements `IUsersRepository`
- Manages user data operations

#### Migrations
- All EF Core migrations are stored here
- Use `IncidentDbContextFactory` for design-time operations

---

### 4. Presentation Layer (incident_management_system.API)

**Responsibility**: Handles HTTP requests and responses.

**Dependencies**:
- IMS.Application
- IMS.Persistance (for DI registration)
- ASP.NET Core

**Key Components**:

#### Endpoints
- Organized using `IEndpoint` pattern
- Minimal API approach
- Mapped in `Program.cs` via `MapEndpoints()`

#### Exception Handlers
```csharp
ValidationExceptionHandler  // Handles FluentValidation errors
GlobalExceptionHandler     // Catches all unhandled exceptions
```

#### Middleware
- `LoggingMiddleware`: Logs HTTP requests/responses
- Exception handling middleware
- Authentication/Authorization middleware

#### Configuration
```csharp
// Program.cs structure
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)...

var app = builder.Build();

// Configure middleware pipeline
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.UseMiddleware<LoggingMiddleware>();
app.MapEndpoints();
```

---

## Database Schema

### Entity Relationships

```
┌─────────────┐         ┌──────────────────┐         ┌────────────┐
│    User     │◄────────│     Incident     │────────►│  Incident  │
│             │ Created │                  │ Has     │  Comment   │
│ - Id        │ By      │ - Id             │ Many    │            │
│ - Username  │         │ - Title          │         │ - Id       │
│ - Email     │         │ - Description    │         │ - Content  │
│ - Role      │         │ - CreatedAt      │         │ - CreatedAt│
└─────────────┘         │ - ResolvedAt     │         │ - IncidentId│
                        │ - Status         │         │ - UserId   │
                        │ - CreatedByUserId│         └────────────┘
                        └──────────────────┘
```

### Tables

#### Incidents
| Column | Type | Constraints |
|--------|------|-------------|
| Id | integer | PK, Identity |
| Title | varchar(200) | NOT NULL |
| Description | varchar(1000) | NOT NULL |
| CreatedAt | timestamp with time zone | NOT NULL |
| ResolvedAt | timestamp with time zone | NULL |
| Status | integer (enum) | NOT NULL |
| CreatedByUserId | integer | FK → Users(Id), NOT NULL |

#### Users
| Column | Type | Constraints |
|--------|------|-------------|
| Id | integer | PK, Identity |
| Username | varchar(100) | NOT NULL |
| Email | varchar(200) | NOT NULL |
| Role | text | NOT NULL |

#### IncidentComment
| Column | Type | Constraints |
|--------|------|-------------|
| Id | integer | PK, Identity |
| Content | varchar(1000) | NOT NULL |
| CreatedAt | timestamp with time zone | NOT NULL |
| IncidentId | integer | FK → Incidents(Id), NOT NULL, ON DELETE CASCADE |
| UserId | integer | FK → Users(Id), NOT NULL, ON DELETE RESTRICT |

### Foreign Key Behaviors
- **Incident → User (CreatedBy)**: `ON DELETE RESTRICT`
- **IncidentComment → Incident**: `ON DELETE CASCADE`
- **IncidentComment → User**: `ON DELETE RESTRICT`

---

## API Endpoints

### Incidents

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/incidents` | Get all incidents (paginated) |
| GET | `/api/incidents/{id}` | Get incident by ID |
| POST | `/api/incidents` | Create new incident |
| PUT | `/api/incidents/{id}` | Update incident |
| PATCH | `/api/incidents/{id}/status` | Update incident status |
| DELETE | `/api/incidents/{id}` | Delete incident |

### Users

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get user by ID |
| POST | `/api/users` | Create new user |

### Health Checks

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Application health status |

---

## Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "PostgresDB": "Host=localhost;Port=5432;Database=incident_management;Username=incident_user;Password=incident_user"
  },
  "JWT": {
    "Authority": "http://localhost:8080/realms/Incident_management_system_realm",
    "Audience": "incident_management_api"
  }
}
```

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Development, Staging, Production
- Connection strings can be overridden via environment variables

---

## Development Guidelines

### Running the Application

1. **Prerequisites**:
   ```bash
   dotnet --version  # Should be 10.0+
   ```

2. **Database Setup**:
   ```bash
   # From solution root
   dotnet ef database update --project IMS.Persistance --startup-project incident_management_system.API
   ```

3. **Run Application**:
   ```bash
   dotnet run --project incident_management_system.API
   ```

4. **Access Swagger**:
   ```
   https://localhost:5001/swagger
   ```

### Adding New Features

#### 1. Add Entity (if needed)
```csharp
// IMS.Domain/Entities/YourEntity.cs
public class YourEntity
{
    public int Id { get; set; }
    // ... properties
}
```

#### 2. Create Command/Query
```csharp
// IMS.Application/Features/YourFeature/Commands/YourCommand/YourCommand.cs
public record YourCommand(string Param) : IRequest<YourResponse>;
```

#### 3. Create Handler
```csharp
// YourCommandHandler.cs
public class YourCommandHandler : IRequestHandler<YourCommand, YourResponse>
{
    public async Task<YourResponse> Handle(YourCommand request, CancellationToken ct)
    {
        // Implementation
    }
}
```

#### 4. Add Validator (if needed)
```csharp
// YourCommandValidator.cs
public class YourCommandValidator : AbstractValidator<YourCommand>
{
    public YourCommandValidator()
    {
        RuleFor(x => x.Param).NotEmpty().MaximumLength(100);
    }
}
```

#### 5. Create Endpoint
```csharp
// incident_management_system.API/Endpoints/YourEndpoint.cs
public class YourEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/your-resource", async (YourCommand cmd, IMediator mediator) =>
        {
            var result = await mediator.Send(cmd);
            return Results.Ok(result);
        });
    }
}
```

### Database Migrations

#### Create Migration
```bash
dotnet ef migrations add YourMigrationName --project IMS.Persistance --startup-project incident_management_system.API
```

#### Update Database
```bash
dotnet ef database update --project IMS.Persistance --startup-project incident_management_system.API
```

#### Remove Last Migration
```bash
dotnet ef migrations remove --project IMS.Persistance --startup-project incident_management_system.API
```

### Code Style Guidelines

1. **Use Records for DTOs**: `public record YourDto(string Property);`
2. **Use Expression-bodied Members**: `public string Name => _name;`
3. **Async All the Way**: Always use `async/await` for I/O operations
4. **Nullable Reference Types**: Enabled across all projects
5. **Vertical Slice Architecture**: Organize by feature, not by layer
6. **CQRS**: Separate commands (write) from queries (read)

### Testing Strategy

#### Unit Tests (Future)
- Test handlers in isolation
- Mock repositories
- Test validators

#### Integration Tests (Future)
- Test with real database (TestContainers)
- Test full request pipeline

#### Performance Considerations
- Use `AsNoTracking()` for read-only queries
- Use `AsSplitQuery()` for complex includes
- Implement pagination for large datasets
- Consider caching for frequently accessed data

---

## Security

### Authentication
- JWT Bearer tokens
- Configured via `JWT` section in appsettings.json
- Integration with external identity provider (Keycloak)

### Authorization
- Policy-based authorization (to be implemented)
- Role-based access control

### Best Practices
- Input validation using FluentValidation
- SQL injection prevention (EF Core parameterized queries)
- Exception handling middleware
- HTTPS enforcement (production)

---

## Logging

### Structured Logging
- Uses `ILogger<T>` from Microsoft.Extensions.Logging
- Logged via `LoggingBehavior` for all MediatR requests
- HTTP request/response logging via `LoggingMiddleware`

### Log Levels
- **Information**: Request start/completion, business events
- **Warning**: Validation failures, expected errors
- **Error**: Unhandled exceptions, system errors

---

## Performance Optimization

### Database
- **Indexes**: Add on frequently queried columns
- **Pagination**: Implemented for all list endpoints
- **Split Queries**: Used for complex includes
- **No Tracking**: Used for read-only queries

### Application
- **Pipeline Behaviors**: Minimal overhead
- **Async/Await**: Non-blocking I/O operations
- **Dependency Injection**: Scoped lifetimes for repositories

---

## Future Enhancements

### Short Term
- [ ] Unit and integration tests
- [ ] API versioning
- [ ] Rate limiting
- [ ] Response caching

### Medium Term
- [ ] Domain events
- [ ] Audit logging
- [ ] Soft delete pattern
- [ ] Advanced search/filtering

### Long Term
- [ ] CQRS with separate read/write databases
- [ ] Event sourcing
- [ ] Microservices decomposition
- [ ] Real-time notifications (SignalR)

---

## Troubleshooting

### Common Issues

**Issue**: Migration fails with "pending model changes"
```bash
# Solution: Create a new migration
dotnet ef migrations add FixPendingChanges --project IMS.Persistance --startup-project incident_management_system.API
```

**Issue**: DbContext not found during migration
```bash
# Solution: Ensure IncidentDbContextFactory is properly configured in IMS.Persistance
```

**Issue**: Circular dependency detection
```bash
# Solution: Check that Domain doesn't reference Application or Infrastructure
```

---

## References

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)

---

## Contributors

- Daniel Pimentel - [GitHub](https://github.com/danielpimentel00)

---

**Last Updated**: February 2026  
**Version**: 1.0.0
