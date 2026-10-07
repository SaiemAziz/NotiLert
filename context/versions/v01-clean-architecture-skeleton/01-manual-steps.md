---
version: v01
---

# Manual Steps — v01: Clean Architecture Skeleton

Actions the developer performs by hand (not delegated to the agent): account setup, installs, dashboard configuration, credential generation, etc.

- [x] Install/verify .NET SDK version matches target (.NET 9/10)
- [x] Run `dotnet new sln` and `dotnet new webapi` / `classlib` commands to scaffold projects
- [ ] Initialize git repo, add `.gitignore` for .NET
- [x] Decide and document folder/naming conventions for the rest of the project
- [ ] Set up IDE/editor workspace settings (EditorConfig, formatting rules)

## Notes
- Projects placed directly under `Backend/` (`NotiLert.Domain`, `NotiLert.Application`, `NotiLert.Infrastructure`, `NotiLert.Api`) with `NotiLert.slnx`. 
