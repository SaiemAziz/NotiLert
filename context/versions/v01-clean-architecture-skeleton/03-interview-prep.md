---
version: v01
---

# Interview Prep — v01

## Likely Questions & Short Answers

**Q:** Why did you split the solution into four separate project assemblies instead of using folders inside a single project?
**A:** Physical project boundaries enforce architectural rules at compile time. In a single-project folder setup, developers can easily take accidental dependencies (e.g. using database types inside domain entities). Separate projects make invalid dependency directions a compilation error.

**Q:** Why does `NotiLert.Api` reference `NotiLert.Infrastructure` if Clean Architecture says presentation shouldn't depend on infrastructure?
**A:** `Api` acts as the application's Composition Root—the place where dependency injection is bootstrapped at startup. It references `Infrastructure` solely to call `AddInfrastructure(configuration)`. Controllers and endpoints in `Api` only ever consume abstractions declared in `Application`.

**Q:** If `Api` references `Infrastructure`, and `Infrastructure` references `Application`, the compiler transitively gives `Api` access to `Application`. Why not break the direct project reference between `Api` and `Application`?
**A:** Relying on transitive references hides true architectural intent. The API layer's primary purpose is to invoke Application use cases (commands, queries, DTOs). Bypassing the direct link makes `Api` artificially coupled to a specific Infrastructure implementation, preventing you from swapping in alternate or test infrastructure (e.g., `InMemoryInfrastructure`) without breaking compilation of the API endpoints.

**Q:** In .NET, NuGet packages flow transitively. Why not install common packages (like DI abstractions, serialization, or logging) once in `Domain` so all layers have access without repeating package installs?
**A:** This violates the core principle of **Domain Purity** in Clean Architecture and DDD. Business rules and entities should outlive any framework, ORM, or runtime. Installing framework packages in `Domain` invites developers to leak framework and hosting concerns into domain logic. Keeping `Domain` as pure C# POCOs guarantees lightning-fast unit tests with zero mocking frameworks.

**Q:** How does `Program.cs` access `builder.Services.AddApplication()` if that method was written by us in a separate class library and is not built into Microsoft's framework?
**A:** C# extension methods use the `this` modifier on their first parameter (`this IServiceCollection services`). Because `Api.csproj` has a `<ProjectReference>` to `Application.csproj`, the compiler inspects `Application.dll` and turns the syntax `services.AddApplication()` into a static call `DependencyInjection.AddApplication(services)` behind the scenes.

**Q:** What are the trade-offs between placing DI extension methods in `namespace Microsoft.Extensions.DependencyInjection` versus your own project namespace (e.g., `namespace NotiLert.Application`)?
**A:** Using `Microsoft.Extensions.DependencyInjection` allows consumer projects like `Program.cs` to discover `AddApplication()` automatically without needing extra `using` statements (how Microsoft ships ASP.NET Core libraries). Using your own namespace (e.g., `NotiLert.Application`) requires an explicit `using NotiLert.Application;` in `Program.cs`, but makes assembly boundaries completely transparent and prevents hiding custom logic inside third-party namespaces.

**Q:** What is the fundamental difference between adding a project to a Solution (`.sln`/`.slnx`) versus adding a Project Reference (`<ProjectReference>`) in `.csproj`?
**A:** A solution file is purely a developer workspace and build-orchestration container; it tells the IDE and `dotnet build` which projects to compile together, but grants zero type visibility. A `<ProjectReference>` in `.csproj` is a compiler-level instruction that allows the dependent assembly to consume public types and APIs from the referenced project.

**Q:** Why does `NotiLert.Domain` have zero NuGet package references?
**A:** Domain purity. Enterprise business rules should be independent of external frameworks, libraries, databases, or UI concerns. Pure C# POCOs allow unit tests to execute in isolation in microseconds without mocking dependencies.

## Alternative Approaches (for "why didn't you use X instead")
- **Assembly Scanning / Reflection for DI:** Dynamically loading `Infrastructure.dll` avoids compile-time references from `Api`, but adds reflection overhead and delays dependency resolution errors to runtime.
- **Vertical Slice Architecture:** Instead of layering horizontally (`Domain`/`Application`/`Infrastructure`), slices group code by feature. Clean Architecture was selected here because NotiLert is fundamentally a notification hub integrating multiple external infrastructure channels (Postgres, RabbitMQ, Mailpit, Twilio, Discord, etc.) that share common domain abstractions.
