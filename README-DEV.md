# 🚀 Developer Guide - Lume-Server

This guide provides comprehensive instructions and best practices for developers contributing to the **Lume-Server** project.

---

## 📂 Project Structure

```
📦 Lume-Server
├── 📁 Lume.API               # Handles API endpoints and request routing
│   ├── 📁 Controllers        # API controllers for handling requests and responses
│   ├── 📄 Program.cs         # Application entry point
│   ├── 📄 appsettings.json   # Configuration settings
│   └── 📄 Lume.http          # Mock HTTP requests for testing
│
├── 📁 Lume.Application       # Business logic, use cases, and service interfaces
│   ├── 📁 Prenotations
│   │   ├── 📄 IPrenotationService.cs  # Interface for prenotation services
│   │   └── 📄 PrenotationService.cs   # Implementation of prenotation services
│   └── 📁 Extensions
│       └── 📄 ServiceCollectionExtensions.cs  # Dependency injection configurations
│
├── 📁 Lume.Domain            # Domain models and business rules
│   └── 📁 Repositories
│       └── 📄 IPrenotationRepository.cs  # Interface for prenotation data access
│
└── 📁 Lume.Infrastructure    # Data persistence and external integrations
    ├── 📁 Extensions
    │   └── 📄 ServiceCollectionExtensions.cs  # Infrastructure service registrations
    └── 📁 Persistence
        ├── 📁 Repositories
        │   └── 📄 PrenotationRepository.cs    # Repository implementation for prenotations
        └── 📄 RestaurantDbContext.cs         # Database context
```


## Technology Choices and Rationale

### 🛠️ Clean Architecture

Why Clean Architecture?
Clean Architecture promotes separation of concerns, enhancing maintainability, testability, and scalability by ensuring that business logic remains independent from external systems like databases or frameworks.

- How it works:

    - Defines clear layers (Domain, Application, Infrastructure, API) with strict dependency rules.

    - Business logic and entities reside at the core, unaffected by external changes.

    - Infrastructure and external dependencies are abstracted through interfaces, making them easily replaceable.

### 🛠️ MediatR

Why MediatR?
MediatR provides an in-process messaging system that promotes loose coupling between components. It simplifies request-response and command-query responsibilities.

- How it works:

    - Separates request handling logic from controllers.

    - Commands and Queries are handled by specific handlers, keeping controllers thin and business logic isolated.

### 🛠️ Entity Framework Core (EF Core)

Why EF Core?
EF Core is a robust Object-Relational Mapper (ORM) simplifying database interactions, reducing boilerplate code, and facilitating database schema migrations.

- How it works:

    - Maps database tables to domain entities, enabling easy CRUD operations.

    - Provides migration tools to manage database schema changes.

### 🛠️ Swagger

Why Swagger?
Swagger auto-generates interactive API documentation, significantly improving API discoverability and developer experience.

- How it works:

    - Automatically generates UI-based documentation of endpoints.

    - Facilitates testing APIs directly via the Swagger interface.

### 🛠️ AutoMapper

Why AutoMapper?
AutoMapper efficiently handles object-object mapping, significantly reducing manual mapping errors and improving code readability.

- How it works:

    - Automatically maps properties between different object types, like entities and DTOs.

    - Configurable mapping profiles simplify data transfer operations.

### 🛠️ FluentValidation

Why FluentValidation?
FluentValidation enables clear, maintainable validation logic, improving reliability and readability of input validation code.

- How it works:

    - Uses a fluent API to define validation rules for request models.

    - Easily integrates with ASP.NET Core pipeline to validate inputs before processing requests.

### 🛠️ Serilog

Why Serilog?
Serilog offers structured logging, facilitating easier debugging, monitoring, and analytics.

- How it works:

    - Records logs in a structured format (e.g., JSON), allowing complex querying and analysis.

    - Supports multiple sinks (console, file, databases, external services).

### 🛠️ Command Query Responsibility Segregation (CQRS)

Why CQRS?
CQRS separates commands (write operations) from queries (read operations), enhancing clarity, scalability, and performance.

- How it works:

    - Commands modify system state, managed separately from queries which retrieve data.

    - Enables optimizations for reading (e.g., caching) and writing (e.g., validation and business logic isolation).


---

## 🛠️ Getting Started

### 📌 Prerequisites

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- Visual Studio, JetBrains Rider, or Zeditor

### 🚧 Setup Instructions

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

API available at: [http://localhost:5155](http://localhost:5155)

---

## 🧩 Architecture and Patterns

### 🔄 CQRS Core Components

- **Commands** 📥
   - Modify system state
   - Implement `IRequest` from MediatR
   - Example: `AssignUserRoleCommand`

- **Command Handlers** 🛠️
   - Execute business logic
   - Dependencies injected via constructor
   - Example: `AssignUserRoleCommandHandler`

- **Queries** (To be implemented) 📤
   - Retrieve data without modifying state

- **Mediator** 🎯
   - Decouples controllers from handlers
   - Example: `await mediator.Send(command)`

### 🔑 Identity Management

- **Entities**
   - `ApplicationUser` (extends `IdentityUser<Guid>`)
   - `IdentityRole<Guid>`

- **Services**
   - `UserManager<ApplicationUser>`
   - `RoleManager<IdentityRole<Guid>>`
   - `SignInManager`

- **Role Assignment Flow**
   - HTTP Request ➡️ Command ➡️ Mediator ➡️ Handler ➡️ Services

### 📥 Data Flow
- **Request** ➡️ **Controller** ➡️ **Command Handler** ➡️ **Persistence** ➡️ **Response**

---

## 📜 Code Conventions

- **Dependency Injection** 🪛
   - Use primary constructors (C# 12+)

  ```csharp
  public class IdentityController(IMediator mediator) : ControllerBase
  ```

- **CQRS Naming** 📛
   - Commands: `VerbNounCommand` (e.g., `AssignUserRoleCommand`)
   - Command Handlers: `CommandNameHandler`
   - Queries: `VerbNounQuery`
   - Query Handlers: `QueryNameHandler`

- **Logging** 📊
   - Structured logging using `ILogger<T>`

  ```csharp
  logger.LogInformation("Assigning user role: {@Request}", request);
  ```

---

## 🎯 API Development Guidelines

### ✅ Coding Standards
- Follow **C# naming conventions**
- Use **dependency injection** consistently
- Clearly separate **business logic** and **data access**
- Always return **DTOs**, not database models

### 📖 API Documentation
- Swagger UI: `/swagger` or `/`

### 🚨 Error Handling
- Implement middleware for global exception handling
- Return suitable HTTP status codes (🟠 <span style="color:orange;">400</span>, 🔴 <span style="color:red;">500</span>, etc.)

---

## 🤝 Contributing

### 🌿 Branching Strategy
- `master` → Production-ready code
- `dev` → Development
- `feature/{feature-name}` → New features
- `bugfix/{issue-name}` → Bug fixes
- `docs` → Documentation updates

### 📝 Commit Messages

```bash
feature: Add reservation endpoint
fix: Correct SQL query in restaurant service
refactor: Optimize authentication logic
```

### 🔄 Pull Requests
- Create a dedicated branch for each issue
- Ensure proper formatting and passing tests

---

## 🧪 Testing *(TODO)*

## 🚀 Deployment *(TODO)*
