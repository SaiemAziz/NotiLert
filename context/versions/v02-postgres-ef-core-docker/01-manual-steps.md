---
version: v02
---

# Manual Steps — v02: PostgreSQL + EF Core + Docker

Actions the developer performs by hand (not delegated to the agent): account setup, installs, dashboard configuration, credential generation, etc.

- [x] Install Docker Desktop / Docker Engine
- [x] Run `docker-compose up` manually the first time and verify container health
- [ ] Install a DB GUI (pgAdmin or DBeaver) and manually connect to confirm the schema
- [x] Create a `.env` file for DB credentials — confirm it's gitignored, never committed
- [x] Install EF Core CLI tools (`dotnet tool install --global dotnet-ef`)

## Notes
- Windows 11 Smart App Control (0x800711C7) can block unsigned assembly loads during `dotnet ef` execution if not excluded or disabled.
- PostgreSQL container verified running with port `5432` mapped to host and health check passing.
