# 📇 Contacts Management System

A Contacts Management System built with **C#**, **.NET 8**, **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**, following **Clean Architecture** principles.

The project focuses on building a maintainable and extensible backend application by separating business rules, application logic, infrastructure concerns, and API responsibilities into independent layers.

It demonstrates practical implementation of **Domain-Driven Design concepts**, **Repository Pattern**, **Dependency Injection**, **DTOs**, **Asynchronous Programming**, and **Entity Framework Core Change Tracking**.

---

## 🚀 Key Features

### 👥 Contact Management

- Retrieve contacts.
- Retrieve a contact by ID.
- Create new contacts.
- Update existing contacts.
- Delete contacts.
- Manage contact information.
- Associate contacts with countries.
- Validate required fields, field lengths, email format, phone characters, and date of birth.
- Reject duplicate email addresses in application-level create and update operations.
- Verify that the selected country exists before saving a contact.

### 🌍 Country Management

- Retrieve country information.
- Associate contacts with countries.
- Maintain relationships between contacts and countries.

### 🛡️ Domain Business Rules

- Encapsulate business rules inside Domain Entities.
- Control modifications through domain methods.
- Protect entity state from invalid changes.
- Keep business logic independent of infrastructure and API concerns.

### 🌐 RESTful API

- Expose contact operations through HTTP endpoints.
- Use HTTP methods according to their intended purpose.
- Return appropriate HTTP status codes.
- Handle exceptions through centralized exception middleware.
- Transfer data using DTOs instead of exposing domain entities directly.

---

## 🛠️ Technology Stack

| Category | Technology |
|---|---|
| Language | C# |
| Framework | .NET 8 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server |
| Architecture | Clean Architecture |
| Design Pattern | Repository Pattern |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| Data Transfer | DTOs |
| Querying | LINQ |
| Programming | Asynchronous Programming |
| IDE | Visual Studio 2022 |
| Version Control | Git & GitHub |

---

## 🏛️ Architecture

The solution follows **Clean Architecture**, separating the application into four main projects.

Each layer has a specific responsibility, and dependencies are organized to keep the Domain independent of external technologies.

### Architecture Overview

```text
                 +-------------------------------+
                 |       ContactsSystemApi       |
                 |                               |
                 | Controllers / Middleware      |
                 +---------------+---------------+
                                 |
                                 ▼
                 +-------------------------------+
                 |   ContactsSystemApplication  |
                 |                               |
                 | Services / DTOs / Requests   |
                 | Interfaces                   |
                 +---------------+---------------+
                                 |
                                 ▼
                 +-------------------------------+
                 |     ContactsSystemDomain     |
                 |                               |
                 | Entities / Business Rules    |
                 +-------------------------------+

                                 ▲
                                 |
                 +-------------------------------+
                 | ContactsSystemInfrastructure |
                 |                               |
                 | EF Core / DbContext          |
                 | Repositories / Data Access   |
                 +-------------------------------+
```

*Note: Infrastructure implements the interfaces defined by the inner layers. The API is the application's entry point and composes the dependencies through Dependency Injection.*

### Solution Projects

| Project | Responsibility |
|---|---|
| **ContactsSystemDomain** | Domain entities and business rules |
| **ContactsSystemApplication** | Application services, DTOs, requests, and interfaces |
| **ContactsSystemInfrastructure** | EF Core, DbContext, repository implementations, and database access |
| **ContactsSystemApi** | RESTful API, controllers, middleware, and application configuration |

---

## 🔄 Data Flow

A typical request follows this flow:

```text
Client
  |
  ▼
ASP.NET Core Web API
  |
  ▼
Controller
  |
  ▼
Application Service
  |
  ▼
Repository Interface
  |
  ▼
Repository Implementation
  |
  ▼
Entity Framework Core
  |
  ▼
SQL Server
```

The API Controller communicates with the Application layer rather than accessing the database directly.

The Application layer depends on abstractions, while Infrastructure provides their concrete implementations.

This separation reduces coupling and makes the application easier to maintain and test.

---
## 🎬 Project Demo

A demonstration video is included in the repository:

```text
Demo.wmv
```

The video provides a visual demonstration of the application.

---
## 🧱 Project Responsibilities

### 1. ContactsSystemDomain

The Domain project contains the core business entities and rules.

**Responsibilities:**

- Define domain entities.
- Encapsulate business rules.
- Protect entity state.
- Control modifications through domain methods.
- Remain independent of EF Core and ASP.NET Core.

**Example structure:**

```text
ContactsSystemDomain
│
└── Entities
    └── Contact.cs
```

Domain entities are responsible for maintaining valid business states rather than allowing arbitrary modifications.

### 2. ContactsSystemApplication

The Application project coordinates the application's use cases.

**Responsibilities:**

- Implement application services.
- Define repository interfaces.
- Define service interfaces.
- Define DTOs and request models.
- Coordinate application operations.
- Keep API and infrastructure details outside the application logic.

**Example structure:**

```text
ContactsSystemApplication
│
├── Contacts
│   ├── Dtos
│   ├── Requests
│   ├── Interfaces
│   └── Services
│       └── ContactService.cs
│
└── Interfaces
```

The Application layer communicates with repositories through interfaces rather than depending on their implementations.

### 3. ContactsSystemInfrastructure

The Infrastructure project implements data access and external technical concerns.

**Responsibilities:**

- Configure Entity Framework Core.
- Define the DbContext.
- Implement repository interfaces.
- Execute database queries.
- Persist changes to SQL Server.
- Configure entity mappings and relationships.

**Example structure:**

```text
ContactsSystemInfrastructure
│
├── Data
│   └── ContactsDbContext.cs
│
├── Models
│
└── Repositories
    └── ContactRepository.cs
```

Infrastructure is responsible for translating application data operations into database operations.

### 4. ContactsSystemApi

The API project is the application's entry point.

**Responsibilities:**

- Expose RESTful endpoints.
- Handle HTTP requests and responses.
- Receive request models.
- Call application services.
- Configure Dependency Injection.
- Configure middleware.
- Handle centralized exception responses.

**Example structure:**

```text
ContactsSystemApi
│
├── Controllers
│   └── ContactsController.cs
│
├── Middleware
│   └── ApiExceptionMiddleware.cs
│
├── Program.cs
└── appsettings.json
```

---

## 🧩 Key Design Patterns and Principles

### Clean Architecture

Separates business logic from technical implementation details.

### Repository Pattern

Encapsulates data access operations behind interfaces.

### Dependency Injection

Provides dependencies through constructor injection instead of manually creating concrete implementations.

### Domain Entities

Encapsulate business rules and control how entity data is modified.

### DTOs (Data Transfer Objects)

Transfer data between application boundaries without exposing internal domain entities.

### Separation of Concerns

Each project is responsible for a specific part of the application.

### SOLID Principles

The architecture is designed around principles such as:

- Single Responsibility Principle (SRP).
- Open/Closed Principle (OCP).
- Liskov Substitution Principle (LSP).
- Interface Segregation Principle (ISP).
- Dependency Inversion Principle (DIP).

---

## 🗄️ Entity Framework Core

The project uses **Entity Framework Core** as its Object-Relational Mapper (ORM).

It provides an abstraction for working with SQL Server through C# objects and LINQ queries.

### Implemented Concepts

- DbContext.
- Entity Mapping.
- LINQ Queries.
- Asynchronous Database Operations.
- Change Tracking.
- SaveChangesAsync.
- Entity State Management.
- Repository Implementations.

### Change Tracking

The project uses EF Core Change Tracking to detect modifications made to tracked entities.

For example:

```csharp
var contact = await context.Contacts
    .FirstOrDefaultAsync(c => c.ContactId == id);

if (contact is null)
    return false;

contact.SetPhone(request.Phone);

await context.SaveChangesAsync();
```

EF Core detects the modified properties and generates the appropriate SQL UPDATE statement.

For read-only queries, `AsNoTracking()` can be used to avoid unnecessary change tracking.

---

## 🔌 Dependency Injection

The application uses the built-in Microsoft Dependency Injection container.

Dependencies are registered during application startup.

Examples include:

- DbContext.
- Repository implementations.
- Application services.

Constructor Injection is used to provide required dependencies.

Example:

```csharp
public class ContactService(
    IContactRepository contactRepository) : IContactService
{
    // Application logic
}
```

This allows the Application layer to depend on abstractions rather than concrete infrastructure classes.

---

## ⚠️ Exception Handling

The API uses centralized exception handling through custom middleware.

Instead of repeating exception handling logic inside every controller, the middleware intercepts unhandled exceptions and returns consistent HTTP error responses.

This helps separate error handling from business and controller logic.

---

## 🗃️ Database

The application uses:

```text
Microsoft SQL Server
```

Main database entities include:

```text
Contacts
Countries
AspNetUsers / AspNetRoles (ASP.NET Core Identity)
RefreshTokens
```

The database stores contact information and the relationships between contacts and countries.

---

## ⚙️ Installation

### Requirements

- Windows.
- Visual Studio 2022 or newer.
- .NET 8 SDK.
- Microsoft SQL Server.
- SQL Server Management Studio (SSMS).

### 1. Clone the Repository

```bash
git clone https://github.com/Albarafahed/ContactsSystemCleanArchitecture.git
```

### 2. Open the Solution

Open the solution file in Visual Studio.

### 3. Configure the Connection String

Configure the connection string outside source control. The API reads the `ConnectionStrings:DefaultConnection` setting and intentionally does not include a real connection string in `appsettings.json`.

For local development, use .NET User Secrets from the API project directory:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=ContactsDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

Alternatively, set the `ConnectionStrings__DefaultConnection` environment variable. Replace the server and database values with your own SQL Server configuration. Do not commit credentials or production connection strings.

### 4. Configure the Database

Make sure the database schema is available and matches the EF Core model configuration.

### 5. Configure Authentication

Run `Database/AuthSchema.sql` against the backed-up `ContactsDB` database. Then set the JWT values using the User Secrets commands in the Authentication section below. The API intentionally fails at startup if the JWT signing key or database connection string is missing or invalid.

### 6. Build and Run

- Build the solution.
- Set `ContactsSystemApi` as the Startup Project.
- Run the application. Swagger is enabled in the Development environment.

---

## 📂 Project Structure

```text
ContactsSystemCleanArch
│
├── ContactsSystemApi
│   ├── Controllers
│   │   └── ContactsController.cs
│   │
│   ├── Middleware
│   │   └── ApiExceptionMiddleware.cs
│   │
│   ├── Program.cs
│   └── appsettings.json
│
├── ContactsSystemApplication
│   ├── Contacts
│   │   ├── Dtos
│   │   ├── Interfaces
│   │   ├── Requests
│   │   └── Services
│   │
│   └── Interfaces
│
├── ContactsSystemDomain
│   └── Entities
│       └── Contact.cs
│
├── ContactsSystemInfrastructure
│   ├── Data
│   │   └── ContactsDbContext.cs
│   │
│   ├── Models
│   └── Repositories
│
└── README.md
```

*The structure above is representative. Adjust folder names to match the actual solution.*

---

## 📝 Current Scope

This project now includes contact management, ASP.NET Core Identity authentication, JWT access tokens, refresh-token rotation, role-based access, and per-user contact ownership. The security section below documents the setup and known limitations.

---

## 🔐 Authentication & Authorization

The authentication implementation uses **ASP.NET Core Identity** with JWT Bearer authentication.

### Implemented endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/auth/register` | Register a standard User account |
| POST | `/api/auth/login` | Verify credentials and issue tokens |
| POST | `/api/auth/refresh` | Rotate a refresh token |
| POST | `/api/auth/logout` | Revoke the supplied refresh token |

### Security controls

- Password hashing and validation are delegated to ASP.NET Core Identity.
- Failed login attempts trigger Identity lockout settings.
- Access tokens are signed JWTs with issuer, audience, signature, and lifetime validation.
- Refresh tokens are generated using a cryptographically secure random generator.
- Only SHA-256 hashes of refresh tokens are stored in the database.
- Refresh-token rotation revokes the previous token; a SQL Server rowversion protects against concurrent reuse.
- Authentication endpoints use a fixed-window rate limit.
- Contact endpoints require authentication. A regular User can access only contacts linked to their user ID; Admin can access all contacts.
- Existing contacts remain unassigned (`UserId = NULL`) after the schema update and are not exposed to regular users.

### Database setup for authentication

1. Back up your existing `ContactsDB`.
2. Run `Database/AuthSchema.sql` against `ContactsDB`. It creates the ASP.NET Core Identity tables, refresh-token table, and nullable `Contacts.UserId` relationship without deleting existing contacts.
3. Configure the connection string as described below.
4. Configure JWT settings using User Secrets from the API project directory:

```bash
dotnet user-secrets set "Jwt:Issuer" "ContactsSystemApi"
dotnet user-secrets set "Jwt:Audience" "ContactsSystemClient"
dotnet user-secrets set "Jwt:Key" "<a-random-secret-at-least-32-utf8-bytes>"
dotnet user-secrets set "Jwt:AccessTokenMinutes" "15"
dotnet user-secrets set "Jwt:RefreshTokenDays" "7"
```

Generate a unique secret for each environment. Never commit the real signing key or connection string.

### Admin account provisioning

Public registration always creates the `User` role; it does not allow clients to self-register as Admin. The schema script creates the `Admin` role. To promote a trusted account, first register it, then assign its user ID to the Admin role using a controlled database operation. Do not expose an unrestricted public role-assignment endpoint.

### Current limitations

- Email confirmation and password-reset delivery are not configured.
- Admin promotion is an operator-controlled database step.
- Legacy contacts require an explicit ownership review before they can be assigned to users.
- Access JWTs remain valid until their short expiry; logout revokes the refresh token, not already-issued access tokens.
- A focused Domain unit-test suite is included. Authentication, repository ownership, and SQL Server integration tests are still required.
- GitHub Actions currently verifies the API build and Domain unit tests. A live SQL Server run after applying the schema has not yet been verified.

---

## 🧪 Automated Tests

A focused Domain unit-test project is included at `ContactSystem.Tests`.

Run the tests from the repository root:

```bash
dotnet test ContactSystem.Tests/ContactSystem.Tests.csproj --configuration Release
```

GitHub Actions also builds the API and runs these Domain tests on pushes and pull requests. Authentication flows, user-to-user ownership isolation, and SQL Server integration still need dedicated integration tests.

---

## 🚀 Future Improvements

Potential improvements include:

### API

- Pagination.
- Filtering and searching.
- API versioning.
- Swagger/OpenAPI documentation improvements.

### Architecture

- Unit of Work Pattern, if justified by the application's transaction requirements.
- Domain-specific exceptions.
- FluentValidation.
- Mapping profiles.
- More comprehensive testing.

### Security

- Email confirmation and password-reset workflows.
- Automated Admin provisioning with a secure one-time bootstrap process.
- Broader authorization policies and security integration tests.

### Testing

- Authentication endpoint integration tests.
- Cross-user contact ownership tests.
- SQL Server repository integration tests.
- Application service tests.

---

## 📚 Learning Objectives

This project provides practical experience with:

- C#.
- .NET 8.
- ASP.NET Core Web API.
- Clean Architecture.
- Domain Entities.
- Business Rules.
- Entity Framework Core.
- SQL Server.
- LINQ.
- Change Tracking.
- Repository Pattern.
- Dependency Injection.
- DTOs.
- Application Services.
- RESTful API Design.
- Asynchronous Programming.
- Middleware.
- SOLID Principles.
- Separation of Concerns.
- Git & GitHub.

---

## 👤 Author

**Albara Fahed Alharissy**

.NET Backend Developer

- GitHub: https://github.com/Albarafahed
- LinkedIn: https://www.linkedin.com/in/albara-csharp-developer/

---

## ⭐ Support

If you find this project useful, consider giving it a ⭐ on GitHub.

Feedback and suggestions are welcome.

---

## 📄 License

No license file is currently included in this repository.