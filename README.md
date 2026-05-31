# BomberosAPI — Arquitectura del Sistema

## Descripción General

BomberosAPI es un sistema de gestión de entrenamiento para bomberos aprendices. El backend está construido con **.NET 10** usando **Clean Architecture** con **Entity Framework Core** como ORM y **Azure SQL** como base de datos.

---

## Estructura del Repositorio

```
ProyectBomberos_ING2/
├── .git/
├── .gitignore
├── README.md
├── BomberosAPI.sln               ← Solución principal
└── src/
    ├── BomberosAPI.API/           ← Capa de presentación (HTTP)
    ├── BomberosAPI.Application/   ← Capa de lógica de negocio
    ├── BomberosAPI.Domain/        ← Capa de entidades y contratos
    └── BomberosAPI.Infrastructure/ ← Capa de acceso a datos
```

---

## Capas de la Arquitectura

### 1. BomberosAPI.Domain
**Responsabilidad:** Define las entidades del negocio y los contratos (interfaces).

- No depende de ninguna otra capa
- Contiene las clases que representan el mundo real del negocio
- No sabe nada de base de datos ni de HTTP

```
Domain/
├── Entities/
│   ├── User.cs
│   ├── TraineeFirefighter.cs
│   ├── TrainingSession.cs
│   ├── SessionParticipant.cs
│   ├── MedicalHistory.cs
│   ├── HealthStaff.cs
│   ├── Invitation.cs
│   └── EnvironmentalData.cs
└── Interfaces/
    ├── IUserRepository.cs
    └── ITrainingSessionRepository.cs
```

---

### 2. BomberosAPI.Application
**Responsabilidad:** Contiene la lógica de negocio del sistema.

- Depende solo de Domain
- Define los casos de uso (¿qué puede hacer el sistema?)
- No sabe de base de datos ni de HTTP

```
Application/
├── Services/
│   ├── UserService.cs
│   └── TrainingSessionService.cs
└── DTOs/
    ├── UserDto.cs
    └── TrainingSessionDto.cs
```

**Ejemplo de caso de uso:**
- Registrar un bombero aprendiz
- Crear una sesión de entrenamiento
- Enviar una invitación
- Registrar signos vitales

---

### 3. BomberosAPI.Infrastructure
**Responsabilidad:** Implementa el acceso a datos con Entity Framework y Azure SQL.

- Depende de Domain y Application
- Es la **única capa** que sabe que existe una base de datos
- Contiene el DbContext, las migraciones y los repositorios

```
Infrastructure/
├── Data/
│   ├── AppDbContext.cs           ← Contexto de EF Core
│   └── Migrations/               ← Historial de cambios de BD
├── Repositories/
│   ├── UserRepository.cs
│   └── TrainingSessionRepository.cs
└── Config/
    ├── UserConfig.cs             ← Configuración de tablas
    └── TrainingSessionConfig.cs
```

**Paquetes instalados:**
- `Microsoft.EntityFrameworkCore.SqlServer` — Conector con Azure SQL
- `Microsoft.EntityFrameworkCore.Tools` — Herramientas de migración

---

### 4. BomberosAPI.API
**Responsabilidad:** Expone los endpoints HTTP que consume el frontend.

- Depende de Application y Domain
- Recibe peticiones HTTP y devuelve JSON
- Contiene los controladores y la configuración del servidor

```
API/
├── Controllers/
│   ├── UsersController.cs
│   └── TrainingSessionsController.cs
├── appsettings.json              ← Configuración general
├── appsettings.Development.json  ← Configuración de desarrollo
└── Program.cs                    ← Punto de entrada del servidor
```

**Paquetes instalados:**
- `Microsoft.EntityFrameworkCore.Design` — Para ejecutar migraciones

---

## Flujo de una Petición HTTP

```
Frontend / App móvil
        │
        │  POST /api/users
        ▼
┌─────────────────┐
│  API Layer      │  ← Recibe la petición HTTP
│  UsersController│
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Application    │  ← Aplica las reglas de negocio
│  UserService    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Domain         │  ← Valida la entidad
│  User.cs        │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Infrastructure │  ← Guarda en la base de datos
│  UserRepository │
└────────┬────────┘
         │
         ▼
    Azure SQL
    bd_bomberos
```

---

## Referencias entre Proyectos

```
API          → Application, Domain
Application  → Domain
Infrastructure → Domain, Application
```

**Regla:** Una capa solo conoce a las capas que están por debajo de ella. La API no habla directo con la base de datos.

---

## Base de Datos

- **Motor:** Azure SQL (SQL Server 12)
- **Servidor:** servbomberos2026.database.windows.net
- **Base de datos:** bd_bomberos
- **ORM:** Entity Framework Core 10

### Tablas creadas:

| Tabla | Descripción |
|---|---|
| TrainingSession | Sesiones de entrenamiento |
| EnvironmentalData | Datos ambientales de cada sesión |
| Invitation | Invitaciones a sesiones |
| SessionParticipant | Participantes por sesión |
| TraineeFirefighter | Bomberos aprendices |
| MedicalHistory | Historia clínica de cada bombero |
| HealthStaff | Personal de salud |

---

## Tecnologías

| Tecnología | Versión | Uso |
|---|---|---|
| .NET | 10 | Framework principal |
| ASP.NET Core | 10 | Web API |
| Entity Framework Core | 10 | ORM |
| Azure SQL | 12 | Base de datos |
| C# | 13 | Lenguaje de programación |

---

## Configuración Local para Desarrolladores

Cada desarrollador debe crear el archivo `src/BomberosAPI.API/appsettings.Development.json` con sus propias credenciales de Azure SQL. Este archivo **no se sube al repositorio** (está en `.gitignore`).

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=servbomberos2026.database.windows.net;Database=bd_bomberos;User Id=TU_USUARIO;Password=TU_PASSWORD;Encrypt=True"
  }
}
```

Reemplaza `TU_USUARIO` y `TU_PASSWORD` con tus credenciales personales de Azure SQL (ver tabla de equipo al final de este archivo).

> En producción o servidor dedicado, la connection string se configura como variable de entorno directamente en el servidor y no se necesita este archivo.

---

## Comandos Útiles

```bash
# Compilar la solución
dotnet build

# Ejecutar el API
dotnet run --project src/BomberosAPI.API

# Crear una migración
dotnet ef migrations add NombreMigracion --project src/BomberosAPI.Infrastructure --startup-project src/BomberosAPI.API

# Aplicar migraciones a Azure SQL
dotnet ef database update --project src/BomberosAPI.Infrastructure --startup-project src/BomberosAPI.API
```


```bash
 dotnet user-secrets set "JwtSettings:SecretKey" "BomberosAPI@2024#Xk9mP2qLvR7nWjT5sYhD3cF8eA1bG6uZ" --project src/BomberosAPI.API
  dotnet user-secrets set "JwtSettings:Issuer" "BomberosAPI" --project src/BomberosAPI.API
  dotnet user-secrets set "JwtSettings:Audience" "BomberosAPIClient" --project src/BomberosAPI.API
```
---

## Equipo de Desarrollo

| Usuario | Rol |
|---|---|
| sqladmin | Administrador del servidor |
| jgutierrez | Desarrollador (db_owner) |
| mcpoveda | Desarrollador (db_owner) |
