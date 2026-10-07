---
version: v03
title: Config & Secrets Management
roadmap_steps: [03]
status: not-started
started: 
completed: 
---

# v03: Config & Secrets Management

## Goal (inlined from roadmap — no need to reopen it)
Centralize configuration and secrets via appsettings, `dotnet user-secrets`, and environment-variable overrides. This is the pattern every later provider integration (email, SMS, push, etc.) will plug credentials into.

## Inherited Context (from previous version's handoff)
- DbContext + Postgres connection exists; currently connection string may be hardcoded/inline — this version formalizes it.

## Sub-Steps (check off as you commit)
- [ ] 1. Define strongly-typed options classes (e.g. `DatabaseOptions`) bound via `IOptions<T>`
- [ ] 2. Restructure `appsettings.json` / `appsettings.Development.json` to hold non-secret config only
- [ ] 3. Run `dotnet user-secrets init` and move local secrets there
- [ ] 4. Add environment-variable override support (standard ASP.NET Core config precedence)
- [ ] 5. Document the config precedence order in a short comment or doc

## Theory to Know Going In
- 12-factor app config principles
- `IOptions<T>` / `IOptionsMonitor<T>` pattern
- Secret management basics (why secrets never belong in source control)

## Validation (from roadmap, copied in full)
- [ ] No secret exists in any committed file
- [ ] The same codebase runs correctly switching only environment variables (no code change needed)
