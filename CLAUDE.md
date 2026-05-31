# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Backend API for a firefighter training management system. Built with **.NET 10 / ASP.NET Core**, **Entity Framework Core 10**, and **Azure SQL**. Follows Clean Architecture with four distinct layers.

## Common Commands

```bash
# Run the API
dotnet run --project src/BomberosAPI.API

# Build the solution
dotnet build

# Add a new EF Core migration
dotnet ef migrations add <MigrationName> --project src/BomberosAPI.Infrastructure --startup-project src/BomberosAPI.API

# Apply pending migrations
dotnet ef database update --project src/BomberosAPI.Infrastructure --startup-project src/BomberosAPI.API
```

There is no test project yet.

## Local Configuration

`appsettings.Development.json` is gitignored and must be created manually:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=__SERVER__;Database=__DATABASE__;User Id=__USER__;Password=__PASSWORD__;Encrypt=True"
  },
  "JwtSettings": {
    "SecretKey": "__JWT_SECRET_KEY__",
    "Issuer": "__JWT_ISSUER__",
    "Audience": "__JWT_AUDIENCE__",
    "ExpirationMinutes": 60
  }
}
```

Database: Azure SQL at `servbomberos2026.database.windows.net`, database `bd_bomberos`.

## Architecture

Clean Architecture with strict layer dependency rules:

```
BomberosAPI.Domain          — Entities, enums, repository interfaces (no external deps)
BomberosAPI.Application     — Use cases, services, DTOs, FluentValidation validators
BomberosAPI.Infrastructure  — EF Core DbContext, repository implementations, JWT service
BomberosAPI.API             — Controllers, middleware, DI wiring, appsettings
```

Dependency rule: `API` → `Application` + `Domain`; `Infrastructure` → `Application` + `Domain`; `Application` → `Domain` only.

### Request Flow

`Controller` → `IService` (Application) → `IRepository` (Domain interface) → `Repository` (Infrastructure) → `AppDbContext` → Azure SQL

### Key Abstractions

- Repository interfaces live in `Domain/Repositories/`; implementations in `Infrastructure/Repositories/`
- Services (application logic) live in `Application/Features/<Feature>/`
- DI registration: `Infrastructure/DependencyInjection.cs` + `API/Common/Extensions/`

### Authentication

- Login via `POST /api/auth/login` → returns JWT
- `GET /api/auth/me` → returns current user from token
- Passwords hashed with BCrypt (`BCrypt.Net-Next`)
- Tokens generated in `Infrastructure/Services/JwtTokenService.cs`
- Auth setup in `API/Common/Extensions/AuthServiceExtensions.cs`

### Adding a New Feature

Follow the existing `Auth` feature as the reference pattern:

1. **Domain**: Add entity in `Domain/Entities/`, add repository interface in `Domain/Repositories/`
2. **Application**: Create `Application/Features/<Feature>/` with request/result DTOs, validator, and service
3. **Infrastructure**: Implement the repository in `Infrastructure/Repositories/`, register it in `DependencyInjection.cs`
4. **API**: Add controller in `API/Controllers/`, register any API-layer services in `Program.cs`
5. **Database**: Add `DbSet<T>` to `AppDbContext`, configure via Fluent API in `Infrastructure/Persistence/Configurations/`, then run `dotnet ef migrations add`

### Key Packages

| Package | Purpose |
|---|---|
| `FluentValidation.AspNetCore` | Request validation |
| `BCrypt.Net-Next` | Password hashing |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | JWT auth |
| `Swashbuckle.AspNetCore` / `Scalar.AspNetCore` | API docs (Swagger + Scalar) |
| `Microsoft.EntityFrameworkCore.SqlServer` | EF Core SQL Server provider |

API docs available at `/swagger` and `/scalar` when running in Development.
