---
name: project-context
description: "Estado completo del proyecto BomberosAPI — stack, arquitectura, módulos implementados y pendientes, BD local configurada"
metadata: 
  node_type: memory
  type: project
  originSessionId: 689e531a-68ec-4f87-acec-04efc9b9b80e
---

## BomberosAPI — Sistema de gestión de entrenamiento para bomberos aspirantes

**Why:** Sistema para monitorear la salud de bomberos aprendices durante sesiones de entrenamiento (signos vitales, bioimpedancia, alertas críticas).

**How to apply:** Usar este contexto para entender qué módulos están listos y cuáles son los próximos a implementar.

---

### Stack
- .NET 10 / ASP.NET Core 10
- Entity Framework Core 10 con SQL Server
- Clean Architecture (4 capas)
- Scalar + Swagger para documentación
- Azure SQL en producción, SQL Server Express local en desarrollo

---

### Estructura
```
BomberosAPI.Domain          → Entidades + contratos (IRepository)
BomberosAPI.Application     → Servicios + DTOs + Validators (FluentValidation)
BomberosAPI.Infrastructure  → DbContext + Repositories + EF Configurations
BomberosAPI.API             → Controllers HTTP
```

---

### Base de datos local (desde 2026-06-23)
- Motor: SQL Server Express (`localhost\SQLEXPRESS`)
- BD: `bd_bomberos`
- Auth: Windows Authentication (Integrated Security)
- Connection string guardada en `dotnet user-secrets` del proyecto API
- Schema aplicado manualmente con `schema.sql` + registro en `__EFMigrationsHistory`
- EF Core reconoce migración `20260623233804_InitialCreate` como aplicada
- **Desde ahora usar:** `dotnet ef migrations add <Nombre>` + `dotnet ef database update`

### Secrets configurados (dotnet user-secrets)
- `JwtSettings:SecretKey`
- `JwtSettings:Issuer` = BomberosAPI
- `JwtSettings:Audience` = BomberosAPIClient
- `ConnectionStrings:DefaultConnection` → localhost\SQLEXPRESS, bd_bomberos, Integrated Security

---

### Módulos implementados (25 tablas, Controllers + Services + Repos)

| Módulo | Branch mergeado |
|---|---|
| Auth (login, JWT, roles) | feature/14-ef-core-integration |
| Institutions + Locations | anterior |
| TraineeFirefighters | feature/23-trainee-firefighters |
| SessionParticipants + check-in | feature/24-participants-and-invitations |
| Invitations (token, accept/reject/revoke) | feature/24-participants-and-invitations |
| MedicalHistory | feature/25-medical-and-environmental |
| VitalSignsMeasurement | feature/25-medical-and-environmental |
| EnvironmentalData | feature/25-medical-and-environmental |
| HealthPersonnel | feature/25-medical-and-environmental |

---

### Módulos PENDIENTES (entidades existen, sin Controller/Service/Repo)

| Entidad | Depende de | Prioridad |
|---|---|---|
| `BioimpedanceMeasurement` | SessionParticipant, HealthPersonnel | Alta — mismo patrón que VitalSigns |
| `SymptomReport` | SessionParticipant, User | Alta — simple |
| `SessionResult` | SessionParticipant, User | Media |
| `CriticalAlert` | VitalSigns, SymptomReport, EnvironmentalData | Baja — depende de los anteriores |

---

### Patrón de implementación de módulos
Cada módulo sigue exactamente esta estructura:

1. `Domain/Repositories/I<Entidad>Repository.cs` — contrato
2. `Infrastructure/Persistence/Configurations/<Entidad>Configuration.cs` — EF fluent API
3. `Infrastructure/Repositories/<Entidad>Repository.cs` — implementación
4. `Application/Features/<Modulo>/Create<Entidad>Request.cs` — DTO entrada
5. `Application/Features/<Modulo>/Create<Entidad>Validator.cs` — FluentValidation con rangos fisiológicos/físicos
6. `Application/Features/<Modulo>/<Entidad>Dto.cs` — DTO salida
7. `Application/Features/<Modulo>/<Entidad>Service.cs` — lógica de negocio con FK validation
8. `API/Controllers/<Entidad>Controller.cs` — endpoint POST + GET

---

### Archivos importantes
- `src/BomberosAPI.API/Program.cs` — registro de DI
- `src/BomberosAPI.Infrastructure/DependencyInjection.cs` — registro de repos y servicios
- `schema.sql` (raíz del proyecto) — script completo con 25 tablas, FKs y 40+ índices
- Migraciones en `src/BomberosAPI.Infrastructure/Persistence/Migrations/`
