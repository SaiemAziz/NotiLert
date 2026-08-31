# Changelog

All notable changes to NotiLert are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project uses incremental micro-versioning — each version is one
self-contained concept, sized to a single 1–2 hour session.

## [v0.3] — 2026-08-31

### Added
- `LoggingMiddleware` — logs method, path, status code and elapsed time for every request.
  - Registered **outermost**, ahead of `ExceptionsMiddleware`, so requests that throw downstream
    are still recorded with their final status rather than vanishing as the exception unwinds.
  - `try/finally` guarantees a log line even on the rethrow path.
  - Timing via `Stopwatch.GetTimestamp()` / `GetElapsedTime()` — no per-request heap allocation.
  - Log level derived from status: 5xx `Error`, 4xx `Warning`, otherwise `Information`.
  - Skips `/openapi` and `/swagger` to keep the development console readable.

### Added (project)
- `CLAUDE.md` — working agreement and project context: developer goals, incremental
  micro-versioning strategy, staged roadmap, and the current-state / known-debt register.

## [v0.2] — 2026-08-31

### Added
- `ExceptionsMiddleware` — catches unhandled exceptions, logs them with request method and path,
  and returns RFC 9457 `application/problem+json` instead of a leaked stack trace.
  Stack traces appear in `Detail` only under the Development environment.
- `INotificationService` / `NotificationService` — orchestration layer owning recipient selection.

### Changed
- **Service layer split into orchestration and transport.** `IEmailService` is now a pure transport:
  `SendEmailAsync(IReadOnlyList<string> to, string subject, string body)`. It no longer depends on
  `AppDbContext`, which keeps it reusable for sending to arbitrary addresses and unit-testable
  without a database.
- `NotificationController` depends on `INotificationService` alone; the EF query no longer lives
  in the controller.

### Removed
- Unused `AppDbContext` field and constructor parameter from `NotificationController`, plus
  `MailKit.Net.Smtp`, `MimeKit` and `NotiLert.Data` usings left over from v0.1.

## [v0.1] — 2026-08-26

### Added
- ASP.NET Core (`net10.0`) Web API with EF Core + PostgreSQL and MailKit.
- `EmailRecipient` entity and initial migrations.
- `POST /api/Notification/send-broadcast` — sends a plain-text message to all active recipients.
- `IEmailService` / `EmailService` extracted from the controller; one SMTP connection reused
  across all recipients, with a configurable delay between sends.

[v0.3]: https://github.com/SaiemAziz/NotiLert/releases/tag/v0.3
[v0.2]: https://github.com/SaiemAziz/NotiLert/releases/tag/v0.2
[v0.1]: https://github.com/SaiemAziz/NotiLert/releases/tag/v0.1
