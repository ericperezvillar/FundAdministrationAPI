# FundAdminAPI (Clean Architecture, .NET 8)

Enterprise-ready Fund Administration Web API implementing Clean Architecture, Repository Pattern, DTOs, AutoMapper, FluentValidation, JWT auth (hardcoded users for simplicity), Serilog, Swagger, API versioning, health checks, and EF Core (SQLite) Code-First.

## Structure
- **FundAdmin.Domain** — Entities & Enums (no dependencies)
- **FundAdmin.Application** — DTOs, Interfaces, Services, Mappings, Validators
- **FundAdmin.Infrastructure** — EF Core, Repositories, Seed Data
- **FundAdmin.API** — Controllers, Middleware, Program

## Run
```bash
dotnet restore
dotnet build
dotnet run --project src/FundAdmin.API/FundAdmin.API.csproj
```

API at: `https://localhost:54513` (or `http://localhost:54514`).

## Auth (local demo)
- Request token: `POST /api/v1/auth/login` with body `{ "username": "admin", "password": "admin123" }`
- Use `Bearer <token>` for protected endpoints (GET/POST/PUT/DELETE).

## Docs
- Swagger: `/swagger`
- Health: `/health`

## Notes
- DB: SQLite file `fundadmin.db` generated at runtime (code-first).
- Logging: Serilog writes console + `Logs/log-*.txt`.
