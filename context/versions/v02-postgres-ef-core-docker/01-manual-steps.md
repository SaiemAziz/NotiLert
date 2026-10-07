---
version: v02
---

# Manual Steps — v02: PostgreSQL + EF Core + Docker

Actions the developer performs by hand (not delegated to the agent): account setup, installs, dashboard configuration, credential generation, etc.

- [ ] Install Docker Desktop / Docker Engine
- [ ] Run `docker-compose up` manually the first time and verify container health
- [ ] Install a DB GUI (pgAdmin or DBeaver) and manually connect to confirm the schema
- [ ] Create a `.env` file for DB credentials — confirm it's gitignored, never committed
- [ ] Install EF Core CLI tools (`dotnet tool install --global dotnet-ef`)

## Notes
(anything discovered while doing these manually, worth remembering for interview prep or future versions)
