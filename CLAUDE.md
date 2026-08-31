# NotiLert — Project Context

> Read this first, every session. It defines **how we work here**, not just what the code is.
> Section 3 is the most important part of this file.

---

## 1. Developer Profile & Goal

- **Level:** Software Engineer, ~2 years professional experience.
- **Stack:** React, .NET, EF Core, MS SQL Server / PostgreSQL. Fluent in day-to-day CRUD and API work.
- **Learning targets:** enterprise system design patterns, background queue processing,
  micro-optimizations, resilience engineering, modern DevOps (Docker, CI/CD).
- **Timeline:** an intensive **1–2 months**, near-daily, **1–2 hours per session** outside full-time work.
  Not an open-ended side project — a focused sprint with a deadline.
- **The actual goal:** **interview readiness.** This project exists so I can *talk* about system design
  with real experience behind it. A working NotiLert I can't explain is a failed NotiLert.

---

## 2. Project Vision & Stack

**NotiLert** (Notification + Alert) is a notification/alerting platform: register recipients and channels,
deliver notifications reliably — eventually asynchronously, with retries, delivery tracking, and observability.

The domain is deliberately *boring* so that all the difficulty lives in the **architecture**. That's the point.

| Layer | Now | Direction |
|---|---|---|
| API | ASP.NET Core Web API (`net10.0`), controllers | Versioned endpoints, auth, rate limiting |
| Data | EF Core 10 + PostgreSQL (Npgsql) | Indexes, constraints, query tuning; MS SQL Server parity later |
| Email | MailKit, single SMTP connection | Pluggable channel abstraction (email → SMS/webhook/push) |
| Async | *none — sends inline in the HTTP request* | `System.Threading.Channels` → hosted service → real broker |
| Resilience | *none* | Polly retry/circuit breaker, outbox, idempotency, DLQ |
| Observability | *none* | `ILogger` → structured logging → health checks → OpenTelemetry |
| Frontend | empty `Frontend/` | React + Vite admin UI |
| DevOps | Dockerfile only | docker-compose, GitHub Actions CI, then CD |

---

## 3. Core Development Guidelines

### 3.1 Guide me — don't build it for me

**This is the rule that matters most. Default to instruction, not implementation.**

When I ask for a feature, a fix, or a refactor, the default answer is **not code you wrote into my files**.
It's a walkthrough I can act on and, more importantly, *defend later*:

1. **Where** — exact file and line/method. `Backend/NotiLert/Services/EmailServices.cs:34`, not "the email service".
2. **What** — the specific change. A short snippet to illustrate is fine and encouraged.
3. **Why this is the best call here** — the reasoning, grounded in *this* codebase with real numbers
   where they exist ("11 s × 49 gaps = a 9-minute HTTP request for 50 recipients"), not textbook generalities.
4. **Alternatives** — the other one or two approaches a senior engineer would weigh, and *why we're
   rejecting them here*. Name the tradeoff, not just the winner. This is the part I'll be asked about.
5. **What breaks without it** — the failure mode this prevents, concretely.

Then **let me write it.** Only edit files directly when I explicitly say so
("implement it", "write it for me", "just do it", "apply that").
If a change is large and mechanical, offer: "want me to apply this, or do you want to write it?"

**Why this rule exists:** code I didn't reason through is code I can't defend in an interview.
Silently doing the work is the single most harmful thing you can do on this project.

### 3.2 Explain patterns before they appear

Before introducing any pattern — DI lifetimes, `Channel<T>`, an index, Polly, an outbox,
a `BackgroundService`, a broker — brief me **first**:

- **Problem:** what concretely hurts in *this* code without it.
- **Mechanism:** how it fixes that, in a few sentences.
- **Cost:** what it complicates; when it would be the wrong call.
- **Alternative:** what else I'd hear about in an interview, and why not that here.

Tight and concrete. A few paragraphs, not an essay.

### 3.3 Syntax and stack questions are welcome

I will sometimes ask **syntax-level or language/framework-level** questions —
`async`/`await` mechanics, `IAsyncEnumerable`, EF change tracking, `IServiceScopeFactory`,
`record` vs `class`, LINQ translation, C# nullability, React hooks rules.

**Answer these fully and precisely.** They're stack interview prep, not a distraction —
treat them as first-class work, not an interruption of "the real task."

### 3.4 Micro-sized and working first

- **One version = one concept = 1–2 hours.** If it needs a second sitting, it was too big — split it.
- **Working beats complete.** Ship the minimal thing that runs; refactor it into the pattern *later*.
  Refactoring my own working code is the learning. Being handed the finished pattern is not.
- **No speculative abstraction.** Don't add an interface, layer, or config knob for a requirement
  that isn't in the current version. We feel the pain first — that's what makes the fix stick.
- Few files per version. A diff I can read in five minutes is the target.

### 3.5 Git & release discipline

Every version ends as a **tagged GitHub release**. The tag is the reward — protect that ritual.

- Branch per version: `feat/v0.2-recipient-crud`. Merge to `main` when it runs.
- Commits: imperative mood, one logical change each. Body explains **why**, not what.
- Tag `vX.Y` on `main` only when the version actually works.
- Keep `CHANGELOG.md` (Keep a Changelog format) — it becomes the GitHub release notes.
- **Never commit, push, or tag on your own.** Propose the command; I run it.
- **No `Co-Authored-By` or `Claude-Session` trailers in commit messages.** This is a portfolio
  repository — the commit history is part of what I'm showcasing, and co-author trailers put
  someone else's name on my contribution graph. Write the message body as normal; just omit
  the trailers.
- Never commit secrets. `appsettings.Development.json` stays gitignored; prefer .NET User Secrets
  (`UserSecretsId` is already declared in `NotiLert.csproj` but unused — adopting it is a good early version).

### 3.6 Keep this file synced

`CLAUDE.md` is the project's memory across sessions. **Treat it as part of the deliverable.**

- At the end of each version, update **§5 Current State** — what shipped, what debt cleared, what's new.
- When a decision changes direction (a stage reordered, a library swapped), record it and the reason.
- If you notice this file has drifted from the code, **say so** — don't silently work around it.

---

## 4. Incremental Progression Strategy

Ordered so each stage creates the *pain* that motivates the next. Roughly a week of sessions per stage.

| Stage | Theme | What I should be able to explain afterward |
|---|---|---|
| **A. Clean foundations** | Recipient CRUD, DTO/validation boundary, DB constraints + indexes, stop leaking entities | Why DTOs at the boundary; why a unique index beats `.Distinct()` in app code; how EF translates the query |
| **B. Make failure visible** | `ILogger` throughout, real error contracts, `ProblemDetails`, config validated at startup | Structured vs string logging; fail-fast at boot vs mid-broadcast; what a good error contract owes a caller |
| **C. Get it off the request thread** | `Channel<T>` producer/consumer + `BackgroundService`; endpoint returns `202 Accepted` + job id | Bounded channels and backpressure; graceful shutdown; why a scoped `DbContext` can't be captured by a singleton |
| **D. Survive failure** | Persisted job/outbox table, Polly retry + circuit breaker, idempotency keys, dead-lettering | At-least-once vs exactly-once; the transactional outbox; why in-memory queues lose data on restart |
| **E. Prove it works** | xUnit against the `IEmailService` seam; integration tests via Testcontainers | Testing at seams; what's worth mocking and what isn't; unit vs integration boundaries |
| **F. Ship it** | docker-compose (API + Postgres), GitHub Actions CI, health checks | Image layering and build caching; what belongs in CI vs CD; liveness vs readiness |
| **G. Show it** | React + Vite admin UI: recipients, compose, delivery status | CORS preflight; API contract design; optimistic UI vs polling for async jobs |
| **H. Scale it out** | Separate worker process, real broker (RabbitMQ/Redis), OpenTelemetry, multi-channel providers | Why a broker beats an in-process channel; competing consumers; tracing across process boundaries |

That right-hand column is the actual deliverable. **Close each version by asking me one or two of those
questions** — if I can't answer, we go back over it before tagging.

Stages are a compass, not a contract. Reorder when reality argues for it — but say *why* first.

---

## 5. Current State

**Tag:** `v0.3` — middleware pipeline in place, service layer separated.

- `Backend/NotiLert/` — ASP.NET Core `net10.0`, EF Core + Npgsql, MailKit. Solution: `Backend/NotiLert.slnx`.
- `POST /api/Notification/send-email-broadcast` — still the only real endpoint.
- `EmailRecipient` (`Id`, `Name`, `Email`, `IsActive`) — the only entity.
- **Service layer, split in `v0.2`:**
  - `INotificationService` / `NotificationService` — *orchestration*. Queries active recipients,
    delegates to the transport, returns the recipient count (`0` → controller answers `404`).
  - `IEmailService` / `EmailService` — *transport only*. Takes `(IReadOnlyList<string> to, subject, body)`,
    opens **one** SMTP connection, loops with a configurable inter-send delay. No DB dependency,
    so it stays reusable for the planned admin report email and unit-testable on its own.
- **Middleware pipeline** (`Program.cs`, order is load-bearing):
  `LoggingMiddleware` → `ExceptionsMiddleware` → HTTPS redirect → authorization → controllers.
  - `LoggingMiddleware` (`v0.3`) — outermost so it still records requests that throw downstream.
    `try/finally`, allocation-free timing via `Stopwatch.GetTimestamp()`, status→level mapping
    (5xx `Error` / 4xx `Warning` / else `Information`), skips `/openapi`.
  - `ExceptionsMiddleware` (`v0.2`) — logs, then returns RFC 9457 `application/problem+json`.
    Stack trace in `Detail` only in Development.
- `Frontend/` exists but is empty.

**Known debt — deliberate, not forgotten.** Each is a future version, tagged with the stage that owns it.
Don't fix these opportunistically; they're the curriculum.

- Send loop runs **inside the HTTP request**; `Smtp:Delay` is 11 s → 50 recipients ≈ a 9-minute held-open
  request. The `v0.3` logging middleware now prints that duration on every call — watch it. *(→ C)*
- `int.Parse(_config["Smtp:Delay"])` has no null fallback (unlike `Smtp:Port` above it) and `Smtp:Delay`
  is absent from the committed `appsettings.json` — throws mid-broadcast outside dev, after some mail
  has already gone out. The compiler flags it as `CS8604`. *(→ B)*
- **No `try/catch` inside the send loop.** The middleware catches at the edge, but one bad address still
  aborts the whole batch with no per-recipient outcome. Reporting needs `IEmailService` to return
  results instead of `void`. *(→ D)*
- No recipient CRUD — the table is only reachable via raw SQL. No unique index on `Email`. *(→ A)*
- No auth, no CORS, no rate limiting on a bulk-email endpoint. *(→ A/F)*
- No tests, no README, no CI. *(→ E/F)*
- No correlation ID — logs can't yet tie a broadcast's lines together, which will matter once the
  send moves off the request thread. *(next middleware version)*
- Leftovers: `NotiLert.http` still points at the scaffolded `weatherforecast` route;
  `Services/EmailServices.cs` declares `class EmailService` (filename/type mismatch);
  `app.UseAuthorization()` runs with no authentication registered (a no-op).

> **Update this section as part of each version, before tagging.** See §3.6.
