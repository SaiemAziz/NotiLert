---
version: v02
---

# Release Notes — v02: PostgreSQL + EF Core + Docker

## Highlights
- **Notification Domain Entity**: Implemented the core `Notification` entity with encapsulated domain rules, delivery status lifecycle (`Pending`, `Sent`, `Failed`), and channel taxonomy (`Email`, `Sms`, `Push`, `Slack`, `Discord`, `Telegram`, `InAppWeb`).
- **PostgreSQL Persistence with EF Core**: Added `NotiLertDbContext` with Npgsql provider, custom Fluent API configuration, string-based enum value conversions, and composite indexes.
- **Automated Schema Migrations**: Created initial migration `InitialCreate` and applied it to PostgreSQL.
- **Docker Compose Dev Parity**: Configured containerized PostgreSQL 17 Alpine database service with health checks, persistent volumes, and environment file configuration (`.env`).
- **CRUD Endpoints**: Added minimal API endpoints for creating notifications (`POST /api/notifications`) and querying notifications (`GET /api/notifications`, `GET /api/notifications/{id}`) with full round-trip verification.
