---
name: reference
description: Referencias a recursos externos y configuraciones del proyecto
metadata: 
  node_type: memory
  type: reference
  originSessionId: 689e531a-68ec-4f87-acec-04efc9b9b80e
---

## Azure SQL (producción)
- Servidor: `servbomberos2026.database.windows.net`
- BD: `bd_bomberos`
- Admin: `sqladmin`
- Estado en junio 2026: BD **Disabled** por suscripción Azure Free Trial vencida

## SQL Server local (desarrollo)
- Instancia: `localhost\SQLEXPRESS`
- BD: `bd_bomberos`
- Auth: Windows Authentication
- Schema aplicado: 2026-06-23

## Comandos clave del proyecto
```bash
# Correr el API
dotnet run --project src/BomberosAPI.API

# Nueva migración
dotnet ef migrations add <Nombre> --project src/BomberosAPI.Infrastructure --startup-project src/BomberosAPI.API

# Aplicar migración
dotnet ef database update --project src/BomberosAPI.Infrastructure --startup-project src/BomberosAPI.API

# Ver secrets
dotnet user-secrets list --project src/BomberosAPI.API
```

## URLs del API local
- Swagger: `http://localhost:5054/swagger`
- Scalar: `http://localhost:5054/scalar/v1`
- OpenAPI JSON: `http://localhost:5054/openapi/v1.json`

## Archivo schema.sql
- Ubicación: raíz del proyecto `ProyectBomberos_Backend/schema.sql`
- Contiene: 25 tablas + 44 FKs + 40+ índices
- Idempotente: tiene DROP TABLE al inicio, se puede ejecutar múltiples veces
