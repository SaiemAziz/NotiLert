---
version: v01
title: Clean Architecture Skeleton
roadmap_steps: [01]
status: in-progress
started: 2026-10-07
completed: 
---

# v01: Clean Architecture Skeleton

## Goal (inlined from roadmap — no need to reopen it)
Set up the solution with `Domain`, `Application`, `Infrastructure`, and `Api` layers (Clean/Onion Architecture). This is the foundation every later step builds on top of — get the dependency direction right now so nothing needs restructuring later.

## Inherited Context (from previous version's handoff)
- None (this is the first version).

## Sub-Steps (check off as you commit)
- [x] 1. Create the solution and four projects: `NotiLert.Domain`, `NotiLert.Application`, `NotiLert.Infrastructure`, `NotiLert.Api`
- [x] 2. Wire project references: `Api` -> `Application` -> `Domain`; `Infrastructure` -> `Application`
- [ ] 3. Register dependency injection in `Program.cs` (empty service registrations for now, just the wiring pattern)
- [ ] 4. Add a `/health` minimal API endpoint returning 200 OK
- [ ] 5. Confirm the solution builds and no layer references outward incorrectly

## Theory to Know Going In
- Clean Architecture / Onion Architecture — why dependencies point inward
- Dependency Inversion Principle
- DDD-lite layering (Domain has zero external dependencies)

## Validation (from roadmap, copied in full)
- [ ] Solution builds with no errors
- [ ] `/health` endpoint responds 200
- [ ] Dependency graph confirmed to flow inward only (no Domain -> Infrastructure reference, etc.)
