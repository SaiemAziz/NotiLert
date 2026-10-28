---
version: v02
status: complete
---

# Outcome — v02

## What Was Actually Built
- **Domain Modeling**:
  - `Notification` entity in `NotiLert.Domain/Entities/Notification.cs` with encapsulated state, factory creation method (`Create`), and domain methods (`MarkAsSent`, `MarkAsFailed`).
  - `NotificationChannel` enum (`Email`, `Sms`, `Push`, `Slack`, `Discord`, `Telegram`, `InAppWeb`).
  - `NotificationStatus` enum (`Pending`, `Sent`, `Failed`).
- **Data Persistence Layer (EF Core + Npgsql)**:
  - `NotiLertDbContext` in `NotiLert.Infrastructure/Persistence/NotiLertDbContext.cs` mapping `DbSet<Notification>`.
  - Fluent API configuration in `NotificationConfiguration.cs` with string enum conversions, length constraints, and database indexes on `Status` and `CreatedAtUtc`.
  - Service registration in `NotiLert.Infrastructure/DependencyInjection.cs` via `services.AddDbContext<NotiLertDbContext>()`.
  - Initial EF Core migration (`20261028202435_InitialCreate`) generated and executed.
- **Containerized Infrastructure**:
  - `docker-compose.yml` specifying PostgreSQL 17 Alpine (`notilert-postgres`), health check (`pg_isready`), persistent volume (`postgres_data`), and port forwarding (`5432:5432`).
  - `.env.example` committed to git and local `.env` gitignored for database credentials.
- **API Endpoints & Verification**:
  - `POST /api/notifications` to create a notification.
  - `GET /api/notifications` to query notifications ordered by creation time descending.
  - `GET /api/notifications/{id:guid}` to retrieve a single notification by identifier.
  - `NotiLert.Api.http` updated with repeatable test queries.
  - Verified full round-trip persistence and retrieval against the PostgreSQL database.

## Deviations From Plan
- Recreated Docker container with explicit host port mapping (`0.0.0.0:5432->5432/tcp`) to allow host-level EF Core CLI tools to access PostgreSQL.
- Addressed Windows 11 Smart App Control (0x800711C7) kernel policy blocks on unsigned dynamic assembly loading during CLI execution.
- Added DTO classes (`CreateNotificationRequest`, `NotificationResponse`) in `NotiLert.Application` to decouple HTTP request/response payloads from domain entities.

## Theory & Code Concepts Applied
- **ORM & Code-First Schema Evolution**: Maintained database schema in synchronization with domain entity design using EF Core migrations and `__EFMigrationsHistory`.
- **Domain Encapsulation with ORMs**: Used a parameterless private constructor (`private Notification() { }`) and private setters so EF Core can materialize objects without exposing mutation to external code.
- **Fluent API Entity Configuration**: Kept entity classes clean of database attributes (`[Table]`, `[Column]`) by isolating database mapping into `IEntityTypeConfiguration<T>`.
- **Enum String Conversions**: Used `.HasConversion<string>()` to store readable and immutable representation in the database, avoiding ordinal shift bugs.
- **Dev-Environment Parity with Docker**: Guaranteed reproducible database environments across developers without requiring host-level Postgres installations.

## Alternatives Considered
- *Integer-based Enum Storage*: Storing enum as `int` saves minimal disk space but causes silent data corruption if enum members are reordered or inserted; rejected in favor of string conversion.
- *Data Annotations on Domain Entities*: Putting `[Required]`, `[MaxLength]` attributes directly in `Domain` leaks ORM concerns; rejected in favor of Fluent API in `Infrastructure`.
- *Database-First / Raw SQL Migrations*: Using Flyway, DbUp, or raw SQL scripts; EF Core migrations were preferred for tight C# integration and type safety.

## Context Handoff (read by the NEXT version only — keep this short)
- PostgreSQL container: service `postgres` in `docker-compose.yml` running on port `5432` with db `notilert`.
- DbContext: `NotiLertDbContext` registered in DI via `AddInfrastructure(IConfiguration)` with connection string `"Database"`.
- Entities available: `Notification` in `NotiLert.Domain.Entities`, enums `NotificationChannel` and `NotificationStatus` in `NotiLert.Domain.Enums`.
- DTOs available: `CreateNotificationRequest` and `NotificationResponse` in `NotiLert.Application.Notifications.DTOs`.
- API endpoints active: `/health`, `POST /api/notifications`, `GET /api/notifications`, `GET /api/notifications/{id:guid}`.
