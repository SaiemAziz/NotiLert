---
version: v03
---

# Manual Steps — v03: Config & Secrets Management

Actions the developer performs by hand (not delegated to the agent): account setup, installs, dashboard configuration, credential generation, etc.

- [ ] Run `dotnet user-secrets init` and manually add local DB credentials there
- [ ] Add/verify `.gitignore` covers `secrets.json` and `.env`
- [ ] Create an `appsettings.Development.json.example` template with placeholder (non-real) values
- [ ] Decide and document the environment variable naming convention (e.g. `NOTILERT_DB__CONNECTIONSTRING`)

## Notes
(anything discovered while doing these manually, worth remembering for interview prep or future versions)
