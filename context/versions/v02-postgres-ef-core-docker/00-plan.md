---
version: v02
title: PostgreSQL + EF Core + Docker
roadmap_steps: [02]
status: not-started
started: 
completed: 
---

# v02: PostgreSQL + EF Core + Docker

## Goal (inlined from roadmap — no need to reopen it)
Add the first `Notification` entity, EF Core migrations, and a Dockerized Postgres instance via docker-compose. Establishes the DB access pattern and containerized local dev parity used for the rest of the project.

## Inherited Context (from previous version's handoff)
- Clean Architecture layers exist (`Domain`/`Application`/`Infrastructure`/`Api`).

## Sub-Steps (check off as you commit)
- [ ] 1. Define `Notification` entity in `Domain`
- [ ] 2. Add `NotiLertDbContext` in `Infrastructure` with EF Core + Npgsql
- [ ] 3. Write `docker-compose.yml` with a `postgres` service
- [ ] 4. Create the initial EF Core migration
- [ ] 5. Apply the migration against the Dockerized DB
- [ ] 6. Add a minimal endpoint to insert/query a Notification, confirm round-trip

## Theory to Know Going In
- ORM fundamentals (EF Core, Code-First)
- Migration-based schema evolution
- Containerized dev parity (why Docker for local Postgres instead of a local install)

## Validation (from roadmap, copied in full)
- [ ] `docker-compose up` boots Postgres successfully
- [ ] `dotnet ef database update` applies the migration cleanly
- [ ] Can insert and query a Notification via the API
