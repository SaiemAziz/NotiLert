---
version: v02
---

# Interview Prep — v02

## Likely Questions & Short Answers

**Q:** Why use EF Core Fluent API configurations (`IEntityTypeConfiguration<T>`) instead of Data Annotations directly on the entity class?
**A:** In Clean Architecture, Domain entities should remain pure POCOs free of persistence or ORM dependencies. Data Annotations like `[Table]`, `[Column]`, or `[ForeignKey]` leak infrastructure concerns into the Domain layer and have limited expressiveness. Fluent API keeps configuration isolated in the `Infrastructure` layer and allows complex mappings (e.g. value conversions, composite keys, indexes, and shadow properties) that attributes cannot express.

**Q:** How does EF Core materialize entities if the entity has a private parameterless constructor and private setters?
**A:** EF Core does not rely on public constructors or public property setters to instantiate and hydrate entities. It uses reflection and compiled IL delegates to read from and write to private backing fields and private constructors. This allows domain models to enforce encapsulation and domain invariants during runtime while still allowing the ORM to populate entities from the database.

**Q:** Why store enum values as strings (`HasConversion<string>()`) in relational databases instead of integer ordinals?
**A:** Integer enums are fragile in production systems. If a developer inserts, removes, or reorders enum members without explicit integer assignments, existing database rows will silently point to the wrong enum values. Storing enums as strings makes database records self-documenting, human-readable in database GUI tools, and resilient against enum ordering changes in code.

**Q:** What is the purpose of the `__EFMigrationsHistory` table in PostgreSQL?
**A:** EF Core uses `__EFMigrationsHistory` to track which migration scripts have already been applied to the database. Before executing any pending migrations, EF Core queries this table, compares it with the migration classes compiled into the assembly, and executes only the unapplied migrations in sequential order inside an atomic transaction.

**Q:** Why run PostgreSQL in a Docker container via `docker-compose.yml` instead of installing PostgreSQL natively on your operating system?
**A:** Docker provides dev-environment parity. Every team member and CI pipeline runs the identical database version, configuration, and extensions regardless of host OS. It avoids host machine contamination, eliminates port conflicts and background service bloat, and allows the entire database environment to be torn down or spun up reproducibly with a single command (`docker compose down -v` / `docker compose up -d`).

**Q:** When executing `dotnet ef database update`, you encountered `System.IO.FileLoadException (0x800711C7): An Application Control policy has blocked this file`. What causes this and how is it resolved?
**A:** This is caused by Windows 11 Smart App Control (SAC) or Windows Defender Application Control (WDAC). The `dotnet-ef` CLI tool is an external global executable that uses dynamic reflection (`Assembly.Load`) to load project DLLs across process boundaries. Because locally compiled binaries are unsigned and lack cloud reputation in Microsoft's database, the Windows kernel Code Integrity driver flags them as untrusted and blocks execution. The resolution is to add an exclusion for the development workspace folder in Windows Security or adjust Smart App Control during local development.

**Q:** How does the EF Core CLI tool discover and instantiate `NotiLertDbContext` when running commands like `dotnet ef migrations add` or `dotnet ef database update`?
**A:** EF Core tools execute the startup project's entry point (`Program.cs`) without running the web server pipeline, building the `IHost` / `WebApplication` instance to retrieve registered services from the `IServiceProvider`. If `AddDbContext<NotiLertDbContext>` is registered in dependency injection, the tooling extracts the options (including connection string and database provider) from DI. Alternatively, developers can provide an `IDesignTimeDbContextFactory<T>` implementation to bypass application startup completely.

## Alternative Approaches (for "why didn't you use X instead")
- **Dapper / Raw SQL instead of EF Core**: Dapper provides higher raw throughput for read-heavy operations, but requires manual schema management, query writing, and change tracking. EF Core was selected for type safety, automated migration management, and change tracking for writes.
- **Database-First instead of Code-First**: Database-First generates C# code by reverse-engineering an existing DB schema (scaffolding). Code-First aligns with Domain-Driven Design, where business entities drive schema evolution, and all changes are version-controlled in git.
- **Embedded In-Memory Database (EF In-Memory / SQLite)**: While faster for quick tests, EF In-Memory does not support relational constraints, transactions, or PostgreSQL-specific features, creating false confidence. Dockerized PostgreSQL ensures total parity between local development and production.
