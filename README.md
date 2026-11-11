# Contacts Management System

A secure Contacts Management RESTful API built with **C#, .NET 8, ASP.NET Core Web API, Entity Framework Core, and SQL Server**, following **Clean Architecture** principles.

The project focuses on building a maintainable backend application with clear separation of concerns, domain-driven business rules, secure authentication and authorization, controlled data access, and activity auditing.

---

## Key Features

### Contact Management
- Retrieve all accessible contacts.
- Retrieve a contact by ID.
- Create contacts.
- Update existing contacts.
- Delete contacts.
- Associate contacts with countries.
- Enforce contact ownership.

### Country Management
- Retrieve country information.
- Associate contacts with countries.
- Maintain contact-country relationships.

### Authentication & Authorization
- JWT Bearer Authentication.
- User registration and login.
- Role-Based Access Control (RBAC).
- Admin and User roles.
- Ownership-based data access.
- Secure refresh token management.
- Refresh token rotation and revocation.
- Logout and session invalidation.

### API Security
- HTTPS configuration.
- CORS configuration.
- Rate Limiting.
- Password hashing.
- Claims-based authorization.
- Centralized exception handling.
- Protection of user-owned contact data.

### Logging & Auditing
- Record authentication events.
- Record contact management activities.
- Track the user responsible for an operation.
- Record the affected entity and its identifier.
- Store operation outcomes and UTC timestamps.
- Associate audit events with Trace IDs when available.
- Admin-only Audit Logs API.
- Pagination and filtering for audit records.

### Audit Logs API

Administrators can retrieve audit records through:

`GET /api/AuditLogs`

Supported query parameters:

| Parameter | Description |
|---|---|
| userId | Filter by user ID |
| action | Filter by operation |
| entityName | Filter by entity |
| fromUtc | Start date in UTC |
| toUtc | End date in UTC |
| pageNumber | Page number |
| pageSize | Number of records per page |

Example:

`GET /api/AuditLogs?userId=1&entityName=Contact&action=Update&pageNumber=1&pageSize=10`

Access is restricted to users with the Admin role.

---

## Technology Stack

| Category | Technology |
|---|---|
| Language | C# |
| Framework | .NET 8 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server |
| Architecture | Clean Architecture |
| Data Access | Repository Pattern |
| Transactions | Unit of Work |
| Authentication | JWT Bearer |
| Authorization | RBAC & Ownership-Based |
| Security | Password Hashing, Rate Limiting |
| Logging | Audit Logging |
| Querying | LINQ |
| API Documentation | Swagger / OpenAPI |
| IDE | Visual Studio 2022 |
| Version Control | Git & GitHub |

---

## Architecture

The solution follows Clean Architecture with four main projects.

```text
ContactsSystemCleanArch
│
├── ContactSystemApi
│   ├── Controllers
│   ├── Middleware
│   ├── Services
│   ├── Program.cs
│   └── appsettings.json
│
├── ContactsSystemApplication
│   ├── AuditLogs
│   ├── Contacts
│   ├── Countries
│   ├── Interfaces
│   ├── Requests
│   └── Services
│
├── ContactsSystemDomain
│   └── Entities
│
└── ContactsSystemInfrastructure
    ├── Persistence
    │   ├── Configurations
    │   └── ContactsDbContext.cs
    ├── Repositories
    └── Security
```

### Layer Responsibilities

**1. Domain Layer**
- Contains core domain entities.
- Encapsulates business rules.
- Controls entity state through domain methods.
- Remains independent of infrastructure and API concerns.

**2. Application Layer**
- Contains application services and use cases.
- Defines repository and service interfaces.
- Contains DTOs and request models.
- Coordinates business operations.
- Defines audit logging and query abstractions.

**3. Infrastructure Layer**
- Implements repository interfaces.
- Configures Entity Framework Core.
- Provides database access through DbContext.
- Implements persistence and security-related services.
- Handles database configurations and relationships.

**4. API Layer**
- Exposes RESTful endpoints.
- Handles HTTP requests and responses.
- Configures authentication and authorization.
- Registers application dependencies.
- Uses middleware for centralized exception handling.

### Dependency Direction

The Application layer depends on Domain abstractions and entities. Infrastructure implements Application interfaces, while the API acts as the application's entry point and composes dependencies through Dependency Injection.

---

## Security Architecture

The project implements security through the following layers:

| Layer | Security Mechanism |
|---|---|
| 1 | HTTPS & CORS |
| 2 | JWT Authentication |
| 3 | Role-Based Authorization |
| 4 | Ownership-Based Authorization |
| 5 | Refresh Tokens & Logout |
| 6 | Rate Limiting |
| 7 | Logging & Auditing |

### Ownership Protection

Contact ownership is assigned using the authenticated user's identity.

- Users can access their own contacts.
- Administrators can access contacts according to their administrative permissions.
- Users cannot change contact ownership through update requests.
- Unauthorized access to another user's contact is rejected.

### Password & Session Security

- Passwords are stored using password hashing.
- Access tokens are used for authenticated API requests.
- Refresh tokens support session renewal.
- Refresh tokens are stored securely as hashes.
- Revoked or expired refresh tokens cannot be used to renew sessions.

---

## Logging & Auditing

Audit records are stored in the `AuditLogs` database table.

Each record contains:

| Field | Description |
|---|---|
| AuditLogId | Unique audit record identifier |
| UserId | User responsible for the event, when available |
| Action | Operation performed |
| EntityName | Entity affected |
| EntityId | Identifier of the affected entity |
| Outcome | Operation result |
| TimestampUtc | UTC event timestamp |
| TraceId | Request correlation identifier, when available |

Audit logging supports monitoring authentication events and contact management operations.

Audit records are exposed through an administrator-restricted API endpoint.

---

## Database

The application uses Microsoft SQL Server with Entity Framework Core.

Main entities include:

- Contacts
- Countries
- Users
- RefreshTokens
- AuditLogs

The database contains relationships between users, contacts, and countries, as well as session and audit information.

Entity configurations are maintained separately from domain entities.

---

## Data Access & Design Patterns

### Repository Pattern
Encapsulates database operations behind interfaces.

### Unit of Work
Coordinates operations that require database transactions.

### Dependency Injection
Provides application dependencies through constructor injection.

### DTOs
Transfers data across application boundaries without exposing domain entities directly.

### Asynchronous Programming
Uses asynchronous database and application operations.

### EF Core Change Tracking
Tracks modifications to entities and persists changes through `SaveChangesAsync`.

### Separation of Concerns
Keeps domain rules, application logic, infrastructure, and API responsibilities separate.

---

## Installation & Setup

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

### 3. Configure the Database Connection

Configure the connection string for the API project.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ContactsDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Adjust the SQL Server instance and authentication settings according to your environment.

**Security note:** Do not commit passwords, JWT signing keys, or other sensitive configuration values to GitHub. Use User Secrets or environment variables for development and deployment secrets.

### 4. Configure the Database

Make sure the SQL Server database schema is available and compatible with the EF Core model configurations.

The database must contain the required tables and relationships.

### 5. Configure JWT Settings

Provide the required JWT configuration securely before running the application.

Do not store production signing keys in source control.

### 6. Build and Run

- Build the solution.
- Set `ContactSystemApi` as the Startup Project.
- Run the application.
- Open Swagger to explore and test the API endpoints.

---

## API Documentation

Swagger / OpenAPI provides interactive API documentation.

Use it to:

- Explore available endpoints.
- Test contact operations.
- Authenticate using JWT.
- Test role-based access restrictions.
- Test ownership protection.
- Test audit log filtering and pagination.

---

## Testing

Important testing scenarios include:

- Successful and failed login attempts.
- Successful and failed registration attempts.
- Refresh token renewal and rejection.
- Logout and token revocation.
- Access restrictions based on user roles.
- Contact ownership enforcement.
- Contact creation, updating, and deletion.
- Audit record generation.
- Admin-only access to audit logs.
- Pagination and filtering validation.

---

## Current Project Scope

This project focuses on secure backend development and contact management.

Its main objectives are to demonstrate:

- Clean Architecture.
- Secure RESTful API development.
- Authentication and authorization.
- Ownership-based access control.
- Database design and persistence.
- Transaction management.
- Audit logging and monitoring.

Features should only be considered complete after implementation and appropriate testing.

---

## Future Improvements

Potential areas for further development:

### API & Validation
- FluentValidation.
- API versioning.
- Advanced search capabilities.

### Testing
- Unit testing.
- Integration testing.
- Automated security testing.

### Infrastructure & Deployment
- Docker containerization.
- Cloud deployment.
- Secure secrets management.
- CI/CD pipelines.

### Monitoring
- Structured application logging.
- Request correlation and Trace ID propagation.
- Advanced audit reporting.

---

## Author

**Albara Fahed Alharissy**

.NET Backend Developer

- GitHub: https://github.com/Albarafahed
- LinkedIn: https://www.linkedin.com/in/albara-csharp-developer/

---

## Support

If you find this project useful, consider giving it a ⭐ on GitHub.

Feedback and suggestions are welcome.

---

## License

No license file is currently included in this repository.
