# NotiLert — System Design Learning Roadmap

> A multi-channel notification platform (Email, SMS, Push, Discord, Telegram, Slack, In-App) built as a hands-on vehicle to learn and *defend* enterprise system design concepts — async processing, resilience, scaling, security, and distributed-systems trade-offs.
> **Stack:** ASP.NET Core (.NET 9/10) · EF Core · PostgreSQL · Redis · RabbitMQ/Redis Streams · React (Vite, Tailwind/Shadcn) · Docker · $0 infra

## What NotiLert Will Be Able to Do (End State)

By the time this roadmap is complete, NotiLert will be a **self-hosted, horizontally scalable notification platform** that can:

- Accept notification requests via a REST API and deliver them across **seven channels**: Email, SMS, Push, Discord, Telegram, Slack, and in-app/web — all behind one pluggable interface so adding an eighth is cheap.
- Guarantee **reliable delivery** even under partial failure — retries, circuit breakers, dead-letter queues, provider fallback chains, and an outbox pattern so no notification is ever silently lost.
- **Scale horizontally**: multiple API instances behind a load balancer, multiple background workers competing off a shared queue, with caching and distributed locking to keep them coordinated.
- Support **multi-tenant** usage: users can register, create their own workspace/tenant, generate their own API keys, and send notifications scoped only to their own data — with per-tenant rate limits and preferences.
- Let users define **priority, scheduling, and multi-step escalation workflows** (e.g., "email now, escalate to SMS if unread in 1 hour").
- Be **secured** with token-based auth, role-based access control, versioned APIs, and safely managed secrets.
- Be fully **observable** — structured logs, metrics, and distributed traces that let you follow one notification's journey end-to-end.
- Ship via a real **CI/CD pipeline** into containers, with health checks, graceful shutdown, and zero-downtime deploy patterns.
- Come with a **React dashboard** for self-service account management, live delivery status, DLQ inspection, and system metrics.
- Be entirely explainable in a system design interview — every pattern used has a documented "why this, not that" reasoning.

---

## For AI Coding Assistants / AI IDEs (Read This Section First)

If you are an AI assistant helping build this project inside an IDE, treat this file as the authoritative spec and work plan. Follow these rules:

1. **The existing codebase predates this roadmap's structure.** It does not map cleanly onto the Steps below, and that's expected. **Do not delete or discard existing code.** Instead: inventory what already exists, compare it against the closest-matching Step(s), and **gradually refactor/extend it** until it satisfies that Step's Goal and Validation criteria — then move to the next Step. Treat this as an incremental migration of a real repo, not a greenfield rewrite.
2. **Work one Step at a time, in order**, starting from Step 01, unless the user explicitly asks to jump ahead or work out of order.
3. **Do not over-engineer ahead of the current Step.** If the user is on a Phase 1 step, resist introducing Phase 4/8/10 patterns (caching, tracing, sagas, etc.) even if it looks convenient — the point of this project is to learn *why* and *when* each pattern gets introduced, not to front-load all of them.
4. **Before marking a Step done, verify it against that Step's Validation criteria** in the tables below, and say explicitly how it was verified.
5. **Respect the $0 / free-tier constraint.** Use local Docker emulators (Mailpit, local SMS/push mocks) by default; only wire up a real free-tier provider (Brevo, Resend, Twilio trial, OneSignal, Firebase) when the user explicitly opts in.
6. **When a tool/library choice is ambiguous** (e.g., RabbitMQ vs Redis Streams, Quartz vs Hangfire), prefer whatever the Step's Key Terms/description names first, but surface the alternative to the user rather than silently deciding — these choices are deliberately part of the learning goal.
7. Keep the Clean Architecture layering from Step 01 (`Domain` → `Application` → `Infrastructure`/`Api`) as new code is added or migrated, even when adapting old code that didn't follow it.

---

## How to Use This Roadmap

- **You already have existing code and tags from before this roadmap existed — that's fine.** Nothing gets deleted. But the old work doesn't line up step-for-step with the structure below, so treat this as a **fresh restart from Step 01**, using the existing repo as raw material: keep what still fits, refactor what's close, and rebuild what doesn't — gradually, step by step, rather than all at once.
- Steps are **generic and sequential** (`Step 01`, `Step 02`, ...) — not pinned to specific git tags, so you (or your AI IDE) can map them to whatever tags/branches make sense as you go.
- Steps are kept **small on purpose** (30–90 min each) so progress feels constant and motivating. Several steps together often form what you'd call one "version."
- Each step lists: **Goal**, **Core Concept** (the interview-defensible "why"), **Key Terms**, and **Validation** (how you prove it works before moving on).
- This is a **map, not a script** — when you reach a step, bring it back up (in chat or to your AI IDE) and go deep on implementation, trade-offs, and alternatives before writing code.
- Concepts are tagged by real-world frequency: 🟢 Common (you'll use this constantly in real jobs) · 🟡 Occasional (comes up in scaled/mature systems) · 🔵 Rarely-implemented-but-interview-favorite (you're building it here specifically *because* most projects never get the chance to).

---

## Phase 0 — Foundations
**Theme:** Clean architecture skeleton, no infra dependencies yet.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **01** | Solution structure with `Domain`, `Application`, `Infrastructure`, `Api` layers | 🟢 Separation of concerns, dependency inversion | Clean Architecture, DDD-lite, Dependency Injection | Builds cleanly; `/health` endpoint responds; dependencies flow inward only |
| **02** | PostgreSQL + EF Core, first `Notification` entity + migration, Dockerized DB | 🟢 ORM fundamentals, migration-based schema evolution | EF Core Migrations, Code-First, Docker Compose | `docker-compose up` boots Postgres; can insert/query a Notification via API |
| **03** | Centralized config & secrets (appsettings, `dotnet user-secrets`, env-based overrides) | 🟢 Secrets management, environment-based config | `IOptions<T>`, user-secrets, 12-factor config | No secret ever committed; same code runs against dev/prod config via env vars only |
| **04** | In-memory producer-consumer queue (`System.Threading.Channels`) + `BackgroundService` consumer | 🟢 Producer-consumer pattern, decoupled processing | Bounded/unbounded channels, backpressure, hosted services | POST returns instantly (202); worker logs processing 1–2s later under load |

---

## Phase 1 — Core Delivery Engine & Channels
**Theme:** All seven channels, behind one abstraction, on real durable queuing infrastructure.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **05** | Email channel via Mailpit (local SMTP) behind `IEmailSender` | 🟢 Provider abstraction (Strategy pattern) | Strategy pattern, SMTP, local emulator | Email visible in Mailpit UI after a request |
| **06** | Swap in-memory channel for a real broker (RabbitMQ or Redis Streams, Dockerized) | 🟢 Durable message queues, at-least-once delivery | Message broker, ack/nack, durable queue | Kill API mid-processing → restart → message still delivered |
| **07** | Publish/Subscribe broadcast channel (e.g., a "system announcement" fanned out to all subscribers of a topic) | 🟡 Pub-Sub vs point-to-point messaging | Pub-Sub, topic/exchange, fan-out | One publish reaches N independent subscribers without duplication logic per subscriber |
| **08** | Introduce `INotificationChannel` abstraction; implement Discord + Telegram webhook senders | 🟢 Strategy + Factory pattern, Open/Closed Principle | Strategy pattern, Factory pattern, channel routing | Same request body, different `channel` field, routes correctly to each provider |
| **09** | SMS channel — start with a local mock SMS gateway (logs to console/file), then wire a real free-trial provider (Twilio trial credit) | 🟢 Provider abstraction reused for a new channel type | SMS gateway integration, sandbox/trial credentials | A test SMS is either logged by the mock or actually received on a real phone via the trial account |
| **10** | Push notification channel via OneSignal or Firebase Cloud Messaging (free tier) | 🟡 Third-party push infrastructure integration | Push notification service, device tokens | A test push notification is received on a registered test device/browser |
| **11** | Slack channel via incoming webhook (reuses the webhook pattern from Step 08) | 🟢 Reinforcing the abstraction — new channel, near-zero new code | Webhook integration, DRY via existing abstraction | Message appears in a test Slack channel |
| **12** | In-app / web notification channel — delivered into the dashboard's own live feed (lays groundwork reused later in Step 48) | 🟡 Treating your own UI as just another delivery channel | In-app notification, channel symmetry | A triggered notification appears in a simple in-app feed endpoint, independent of the dashboard UI being built yet |
| **13** | Idempotency keys & deduplication table | 🟡 Idempotency in distributed/at-least-once systems | Idempotency key, dedup table, exactly-once *effect* | Redelivering the same message twice results in exactly one real send, on any channel |

---

## Phase 2 — Resilience Patterns
**Theme:** The system survives and degrades gracefully under failure.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **14** | Retry policy with exponential backoff + jitter (Polly) around provider calls | 🟢 Transient fault handling | Polly, exponential backoff, jitter | Kill Mailpit → logs show increasing retry delays → eventual failure |
| **15** | Explicit timeout policy per provider call | 🟢 Timeout pattern, avoiding indefinite hangs | Timeout policy, cancellation tokens | A hung provider call is force-failed after N seconds instead of blocking forever |
| **16** | Circuit breaker around each provider | 🟡 Fail fast when a dependency is down | Circuit breaker (closed/open/half-open) | Force provider down → breaker opens after N failures → immediate failure, no wasted retries |
| **17** | Bulkhead isolation — cap concurrent in-flight calls per provider | 🔵 Fault isolation between dependencies | Bulkhead pattern, concurrency limiter | One slow/overloaded provider can't starve threads needed by other providers |
| **18** | Dead-letter queue for messages that exhaust retries | 🟢 Poison message isolation | DLQ, poison message | Failing message lands in DLQ table/queue instead of retrying forever |
| **19** | Provider fallback chain (e.g., primary email provider fails → auto-fallback to secondary) | 🟡 Redundancy / failover for high availability | Failover chain, provider redundancy | Disabling primary provider mid-test still results in delivery via fallback |
| **20** | Fault injection / chaos test harness (toggle to randomly fail providers or drop connections) | 🔵 Chaos engineering basics | Fault injection, chaos testing | A documented chaos-test run shows the system recovering without manual intervention |

---

## Phase 3 — Consistency & Data Patterns
**Theme:** Correctness guarantees across the DB/queue boundary, and query-layer fundamentals.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **21** | Transactional Outbox pattern — write notification + event in one DB transaction, relay publishes async | 🟡 Solves the dual-write problem | Outbox pattern, dual-write problem, eventual consistency | Kill process right after commit, before publish → on restart, message still gets published |
| **22** | Append-only event log for notification lifecycle (Created → Queued → Sent → Failed) | 🔵 Event sourcing basics | Event sourcing, append-only log, state reconstruction | Full history of a notification's state can be rebuilt purely from its event log |
| **23** | Split read/write models for notification history (denormalized read view vs normalized write model) | 🔵 CQRS (Command Query Responsibility Segregation) | CQRS, read model, write model | History endpoint queries a purpose-built read table, not the write-side joins |
| **24** | Database indexing pass + query plan analysis on hot queries | 🟢 Query optimization | `EXPLAIN ANALYZE`, indexing strategy, N+1 queries | Before/after query time comparison documented for at least 2 optimized queries |
| **25** | Simulate a Postgres read replica (second container, logical replication); route history reads to it | 🟡 Read/write splitting, replication lag | Read replica, replication lag, eventual consistency | Read-only queries hit the replica; a documented test shows visible replication lag |
| **26** | Connection pooling tuning (Npgsql pool size, `DbContext` lifetime review) | 🟡 Resource management under load | Connection pooling, pool exhaustion | Load test shows pool exhaustion at low limits, resolved after tuning |

---

## Phase 4 — Traffic & Scale Management
**Theme:** The classic "system design interview" territory — load balancing, sharding, caching.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **27** | Rate limiter (token bucket) per user/tenant/channel | 🟢 Rate limiting algorithms | Token bucket, sliding window, `System.Threading.RateLimiting` | Bursting past quota returns 429 with clear reset-time header |
| **28** | Reverse proxy / lightweight API Gateway in front of the API (YARP or Nginx) | 🟢 Reverse proxy, request routing | Reverse proxy, API Gateway pattern | All traffic flows through the proxy; gateway adds a request ID header |
| **29** | Run 2–3 API instances behind the gateway with load balancing (round-robin / least-connections) | 🟢 Load balancing | Load balancer, round-robin, least-connections, health-aware routing | Repeated requests visibly hit different instances (logged instance ID); one instance can be killed with zero downtime |
| **30** | Scale background workers to N replicas competing off the same queue | 🟢 Horizontal scaling, consumer competition | Consumer group scaling, work distribution | Throughput measurably increases from 1 → 3 workers under the same load test |
| **31** | Redis cache-aside layer for templates/preferences | 🟢 Caching strategy | Cache-aside, TTL, cache invalidation | Cache hit ratio visible in metrics; stale cache correctly invalidated on update |
| **32** | Distributed lock (Redlock-style) to prevent duplicate scheduled-job execution across replicas | 🟡 Coordination across scaled instances | Distributed lock, split-brain risk | 2 worker instances running the same scheduled job → it executes exactly once |
| **33** | Partition the notifications table (by tenant or by date range) | 🔵 Database sharding/partitioning | Table partitioning, shard key selection | Query planner shows partition pruning; old partitions can be dropped/archived cheaply |
| **34** | Consistent hashing to route a tenant consistently to the same shard/worker | 🔵 Consistent hashing | Consistent hashing ring, hot-key avoidance | Adding/removing a shard node only remaps a small fraction of tenants (demonstrated with a small script) |
| **35** | Load test + capacity plan (k6 or NBomber) across the scaled setup | 🟢 Load testing methodology | Throughput vs latency, bottleneck analysis | A written report: max sustainable RPS, p95/p99 latency, and the bottleneck identified |

---

## Phase 5 — Multi-Tenancy & Governance
**Theme:** Behaving like a real multi-customer SaaS platform.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **36** | Multi-tenant data isolation (shared schema + `TenantId`, enforced at query layer) | 🟡 Multi-tenancy architecture | Shared-schema multi-tenancy, tenant isolation | Tenant A's API key can never retrieve Tenant B's data, verified by test |
| **37** | Priority levels (critical/normal/low) affecting queue processing order | 🟢 Priority scheduling | Priority queue | High-priority item processed before an earlier-queued low-priority one |
| **38** | Scheduled/delayed delivery (Quartz.NET or Hangfire) | 🟢 Job scheduling, delayed messaging | Cron scheduling, delayed delivery pattern | A notification scheduled for +2 minutes fires within an acceptable tolerance window |
| **39** | Notification preferences per channel (opt-in/out across all 7 channels, quiet hours) | 🟢 Preference-driven routing | Preference center, quiet-hours logic | User who opted out of Slack never receives a Slack message even if requested |
| **40** | Template engine (Handlebars/Scriban) decoupling content from delivery logic, per channel | 🟢 Separation of content and transport | Template engine, content negotiation | Same event renders channel-appropriate content (e.g., short SMS vs rich email) without code changes |
| **41** | Feature flags for gradually enabling new channels/behaviors per tenant | 🟡 Feature flagging | Feature flag, gradual rollout | A flagged feature (e.g., a new channel) can be toggled per-tenant at runtime with no redeploy |

---

## Phase 6 — Security & Self-Service Accounts
**Theme:** The part every "production-grade" claim has to survive scrutiny on — including users managing their own accounts.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **42** | API authentication (API keys for service-to-service, JWT for dashboard users) | 🟢 AuthN fundamentals | API key auth, JWT, bearer tokens | Unauthenticated requests rejected with 401; valid token/key succeeds |
| **43** | Authorization — tenant-scoped and role-scoped access control | 🟢 AuthZ, RBAC | Role-based access control, claims-based auth | A "viewer" role can read but not trigger sends; enforced server-side, tested |
| **44** | Tenant self-registration + self-service API key generation from the dashboard | 🟢 Self-service SaaS onboarding | Signup flow, tenant provisioning, key management UI | A brand-new user can sign up, get their own tenant/workspace created automatically, and generate their own API key without any manual DB intervention |
| **45** | API versioning strategy (URL or header-based) | 🟢 API versioning | Versioning strategy, backward compatibility | `/v1/` and `/v2/` endpoints coexist without breaking existing clients |
| **46** | Secrets rotation & least-privilege review (provider API keys, DB credentials) | 🟡 Secrets lifecycle management | Secret rotation, least privilege | A provider key can be rotated with zero downtime and no code change |

---

## Phase 7 — Frontend & Real-Time UX
**Theme:** Making the system demoable, self-service, and interactive.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **47** | React dashboard skeleton (Vite + Tailwind/Shadcn) — signup/login, trigger test notifications | 🟢 Consuming an async, authenticated API from a frontend | React Query, optimistic UI, protected routes | A new user can sign up, log in, and trigger a notification from the UI |
| **48** | Live status updates via WebSockets or Server-Sent Events, reusing the in-app channel from Step 12 | 🟡 Real-time push vs polling | WebSockets, SSE, long polling trade-offs | Status badge updates (Queued → Sent/Failed) without a page refresh |
| **49** | Paginated notification history view (cursor or offset pagination) | 🟢 Pagination strategies | Cursor pagination, offset pagination, infinite scroll | History of 10,000+ rows loads incrementally without fetching everything at once |
| **50** | DLQ inspector + manual "replay" action in the dashboard | 🟡 Operational tooling for failure recovery | Manual replay, operational dashboards | A DLQ'd message can be replayed from the UI and successfully redelivered |

---

## Phase 8 — Observability & Ops
**Theme:** Knowing what your system is doing, at all times, without guessing.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **51** | Structured logging with correlation IDs (Serilog) | 🟢 Structured logging | Serilog, correlation ID | One notification's full path can be `grep`'d out of logs by correlation ID |
| **52** | Metrics (queue depth, retry count, failure rate) exported Prometheus-style | 🟢 Metrics & monitoring | Prometheus metrics, counters/gauges/histograms | A local dashboard (Grafana or simple UI) shows live queue depth and failure rate |
| **53** | Distributed tracing across API → queue → worker → provider (OpenTelemetry) | 🟡 Distributed tracing | OpenTelemetry, spans, trace context propagation | A single trace ID shows every hop a notification took, with timing per hop |
| **54** | Health checks (liveness/readiness) wired into the load balancer | 🟢 Health checks | Liveness probe, readiness probe | Load balancer stops routing to an instance that fails its readiness check |
| **55** | Graceful shutdown (drain in-flight work before process exit) | 🟡 Graceful shutdown | `IHostApplicationLifetime`, drain timeout | Sending SIGTERM mid-batch lets in-flight messages finish before the process exits |

---

## Phase 9 — Deployment & Infra Patterns
**Theme:** Shipping like a real team ships.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **56** | CI pipeline (GitHub Actions): build, test, lint on every push | 🟢 Continuous Integration | GitHub Actions, test gate | A failing test blocks merge automatically |
| **57** | CD pipeline: Docker image build + publish to GHCR on tag | 🟢 Continuous Delivery | Docker image publishing, semantic versioning | Pushing a tag produces a pulled-and-runnable image with zero manual steps |
| **58** | Full-stack Docker Compose (API + workers + Postgres + Redis + broker + gateway) as one command | 🟢 Containerized local parity | Docker Compose orchestration | `docker-compose up` brings up the entire system from a clean machine |
| **59** | Blue-green (or canary) deploy simulation using two Compose stacks behind the gateway | 🔵 Zero-downtime deployment strategies | Blue-green deployment, canary release | Switching the gateway's upstream from "blue" to "green" causes zero dropped requests |

---

## Phase 10 — Advanced Distributed Patterns (Capstone)
**Theme:** The concepts most engineers only ever *talk about* — you'll have actually built them.

| Step | Goal | Core Concept | Key Terms | Validation |
|---|---|---|---|---|
| **60** | Leader election among worker replicas for singleton responsibilities (e.g., the scheduler) | 🔵 Leader election | Leader election, coordination service | Kill the current leader → a follower is promoted within a bounded time, singleton job doesn't run twice or zero times |
| **61** | Saga / orchestrated multi-step workflow ("email now → wait 24h if unread → escalate to SMS/Slack") | 🟡 Saga pattern, long-running processes | Saga pattern, orchestration vs choreography, state machine, compensating action | A started workflow waits, checks read-status, and escalates exactly per rules — demonstrated end-to-end |
| **62** | Written CAP-theorem trade-off doc for NotiLert (what you chose to sacrifice and why, per component) | 🟡 Applying CAP theorem to real design decisions | CAP theorem, consistency vs availability trade-offs | A short doc in `/docs/decisions/` explicitly states NotiLert's CAP stance per subsystem (queue, DB, cache) |

---

## Concept Coverage Matrix (For Interview Prep)

| Category | Concepts Covered | Frequency |
|---|---|---|
| **Channels** | Email, SMS, Push, Discord, Telegram, Slack, In-App — all behind one Strategy-pattern interface | 🟢 (core), 🟡 (each new provider integration) |
| **Async & Queuing** | Producer-consumer, channels, message brokers, pub-sub, outbox pattern, priority queues, delayed delivery | 🟢🟢🟢🟡🟡🟢🟢 |
| **Resilience** | Retry + backoff, timeout, circuit breaker, bulkhead, DLQ, fallback chains, chaos testing | 🟢🟢🟡🔵🟢🟡🔵 |
| **Consistency & Data** | Outbox pattern, event sourcing, CQRS, indexing/query optimization, read replicas, connection pooling | 🟡🔵🔵🟢🟡🟡 |
| **Scale & Traffic** | Rate limiting, reverse proxy/API gateway, load balancing, horizontal scaling, caching, distributed locking, sharding/partitioning, consistent hashing, load testing | 🟢🟢🟢🟢🟢🟡🔵🔵🟢 |
| **Multi-Tenancy & Governance** | Tenant isolation, priority scheduling, feature flags, template/preference systems | 🟡🟢🟡🟢 |
| **Security & Accounts** | AuthN/AuthZ, RBAC, self-service tenant onboarding, API versioning, secrets management/rotation | 🟢🟢🟢🟢🟡 |
| **Frontend/UX** | Real-time push (WebSockets/SSE), pagination, operational tooling | 🟡🟢🟡 |
| **Observability** | Structured logging, metrics, distributed tracing, health checks, graceful shutdown | 🟢🟢🟡🟢🟡 |
| **Deployment** | CI/CD, containerization, blue-green/canary deploys | 🟢🟢🔵 |
| **Advanced Distributed Systems** | Leader election, saga pattern, CAP theorem trade-offs | 🔵🟡🟡 |

**Legend:** 🟢 Common in everyday backend work · 🟡 Shows up once systems mature/scale · 🔵 Rarely implemented hands-on, but a favorite in senior/staff-level interviews.

---

## Suggested Working Rhythm

1. Pick the next Step (check where your existing codebase already lands — steps don't have to be done one-per-tag; batch a few small ones into one tag if that suits your pace).
2. Re-read its Goal + Core Concept here for context.
3. Come back and discuss implementation approach, trade-offs, and alternatives in detail before writing code.
4. Build, test against the Validation criteria, commit, and tag it however fits your own scheme (e.g. `v1.4`, `feature/step-23-cqrs`).
5. Write a 3–5 sentence "design decision" note in the repo's `/docs/decisions/` (lightweight ADR) — this becomes your interview talking points, and doubles as a personal changelog of *why*, not just *what*.

**Working with old code and tags without losing anything:** existing git tags are safe to explore anytime without disturbing current work — `git checkout <tag>` (detached HEAD) to look around, or `git worktree add ../notilert-<tag> <tag>` to run an old version in its own folder side-by-side with your current work, ideal for demoing multiple stages of the project at once (e.g., in a portfolio walkthrough).

## Mapping Your Existing Progress

Since you already have work done under your own version tags that predates this roadmap, do a one-time pass with your AI IDE: inventory what exists, match each piece to the closest Step number(s) above, note it in a `PROGRESS.md`, and continue forward from there — refactoring toward the Step structure gradually rather than rewriting everything at once. Nothing existing gets deleted in the process.
