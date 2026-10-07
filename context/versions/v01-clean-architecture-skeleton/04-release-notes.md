---
version: v01
---

# Release Notes — v01: Clean Architecture Skeleton

## Highlights
- **Clean Architecture Foundation**: Scaffolding of the core 4-layer architecture (`NotiLert.Domain`, `NotiLert.Application`, `NotiLert.Infrastructure`, `NotiLert.Api`) targeting **.NET 10** using `NotiLert.slnx`.
- **Inward Dependency Direction**: Enforced architectural boundaries with zero third-party dependencies in `Domain`.
- **Modular Dependency Injection**: Implemented decentralized DI extension methods (`AddApplication()` and `AddInfrastructure()`) keeping `Program.cs` concise and decoupled.
- **Operational Health Check**: Minimal API endpoint `GET /health` responding `200 OK`.

