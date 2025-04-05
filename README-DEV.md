# Developer Guide - Lume-Server

This document provides instructions and guidelines for developers contributing to the **Lume-Server** project.

## 1. Project Structure

```markdown
/Lume.API              # Handles API endpoints and request routing
│── /Controllers       # API controllers managing requests and responses
│── Program.cs         # Main entry point of the application
│── appsettings.json   # Configuration file for the API settings
│── Lume.http          # Collection of mock HTTP requests for testing

/Lume.Application      # Contains business logic, use cases, and service interfaces
│── /Prenotations      # Manages prenotation services and related DTOs
│   │── IPrenotationService.cs  # Interface defining prenotation service methods
│   │── PrenotationService.cs  # Implementation of prenotation services
│── /Extensions        # Application-level extensions for dependency injection
│   │── ServiceCollectionExtensions.cs  # Service registrations and configurations

/Lume.Domain           # Core domain models and business rules
│── /Repositories      # Defines repository interfaces for database interactions
│   │── IPrenotationRepository.cs  # Interface for prenotation data access

/Lume.Infrastructure   # Implementation of data persistence and external service integrations
│── /Extensions        # Infrastructure-level extensions
│   │── ServiceCollectionExtensions.cs  # Registers infrastructure services
│── /Persistence       # Database access and repository implementations
│   │── /Repositories  # Implements repository interfaces
│   │   │── PrenotationRepository.cs  # Concrete repository for prenotations
│   │── RestaurantDbContext.cs  # Database context handling entity mappings
```

## 2. Getting Started

### Prerequisites

- **.NET SDK 9.0+** - [Download](https://dotnet.microsoft.com/download)
- **Visual Studio / JetBrains Rider / Zeditor**

### Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone https://github.com/school-project-restaurant/Lume-Server.git
   cd Lume-Server
   ```

2. **Start the API:**

   ```bash
   cd Lume.API
   dotnet run
   ```
   The API will be running on **[http://localhost:5155](http://localhost:5155)**


## 3. Architecture and Patterns

### CQRS Core Components:

1. **Commands**
   - Represent requests that modify system state
   - Implement MediatR's `IRequest` interface
   - Example: `AssignUserRoleCommand` defines parameters needed to assign a role

2. **Command Handlers**
   - Implement `IRequestHandler<TCommand>`
   - Contain the actual business logic
   - Receive dependencies via DI in constructor
   - Example: `AssignUserRoleCommandHandler` contains logic to find user/role and associate them

3. **Queries (To be implemented)**
   - Will represent requests that read data without modifying state
   - Will implement `IRequest<TResponse>` where TResponse is the return type

4. **Mediator**
   - Used in controllers via injected `IMediator`
   - Decouples request sender from handler
   - Example: `await mediator.Send(command)` sends command to its appropriate handler

### 4. Identity Management System Implementation:

1. **Core Entities**
   - `ApplicationUser`: Extends IdentityUser<Guid> to add custom properties
   - `IdentityRole<Guid>`: Used to define roles in the system

2. **Core Services**
   - `UserManager<ApplicationUser>`: Manages CRUD operations on users
   - `RoleManager<IdentityRole<Guid>>`: Manages CRUD operations on roles
   - `SignInManager`: Manages authentication (login/logout)

3. **Role Assignment Flow**
   - Controller receives HTTP request and creates a command
   - MediatR routes command to its handler
   - Handler uses UserManager and RoleManager to find and associate user and role

### 5. Data Flow in the Application with Request-Response Flow:

1. **API Request**
   - HTTP client sends request to controller
   - Example: POST to `/api/identity/userRole` with user/role data

2. **Controller**
   - Creates appropriate command from request data
   - Sends command to mediator with `mediator.Send(command)`

3. **Command Handler**
   - Receives command and executes business logic
   - Uses services (e.g., UserManager) to interact with domain

4. **Data Persistence**
   - Handler modifies state via domain services or repositories
   - Changes saved to database

5. **Response**
   - Controller returns appropriate HTTP response
   - Example: 204 NoContent for successful operations with no data to return
### 6. Project-specific code conventions:

1. **Dependency Injection**
   - Use constructors with primary constructor syntax (C# 12+)
   - Example: `public class IdentityController(IMediator mediator) : ControllerBase`

2. **CQRS Naming**
   - Commands: `VerbNounCommand` (e.g., `AssignUserRoleCommand`)
   - Command Handlers: `CommandNameHandler` (e.g., `AssignUserRoleCommandHandler`)
   - Queries: `VerbNounQuery` (e.g., `GetUserRolesQuery`)
   - Query Handlers: `QueryNameHandler` (e.g., `GetUserRolesQueryHandler`)

3. **Logging**
   - Use injected ILogger<T> for structured logging
   - Use templates with objects for detailed logging
   - Example: `logger.LogInformation("Assigning user role: {@Request}", request)`







## 7. API Development Guidelines

### Coding Standards

- Follow **C# naming conventions**.
- Use **dependency injection** instead of static classes.
- Separate **business logic (Services)** from **data access (Repositories)**.
- Always return **DTOs** instead of direct database models.

### API Documentation

- **Swagger UI** is available at **`/swagger`** or at **`/`** with a redirect.

### Error Handling

- Use **middleware** for **global exception handling**.
- Return appropriate **HTTP status codes** (e.g., `400 Bad Request`, `500 Internal Server Error`).

## 8. Contributing

### Branching Strategy

- `master` → Production-ready code
- `dev` → Ongoing development
- `feature/{feature-name}` → New features
- `bugfix/{issue-name}` → Bug fixes
- `docs` → Documentation

### 9. Commit Message Convention

Follow this format:

```bash
feature: Add reservation endpoint
fix: Correct SQL query in restaurant service
refactor: Optimize authentication logic
```

### 10. Pull Requests

- **Create a new branch** before working on an issue.
- Ensure **code is formatted** and **unit tests pass**.

## 5. Testing (TODO)

## 6. Deployment (TODO)

