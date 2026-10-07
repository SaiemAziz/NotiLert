---
version: v05
title: Email Channel via Mailpit
roadmap_steps: [05]
status: not-started
started: 
completed: 
---

# v05: Email Channel via Mailpit

## Goal (inlined from roadmap — no need to reopen it)
Implement the first real delivery channel: email via local Mailpit SMTP, behind an `IEmailSender` abstraction. Establishes the provider-abstraction pattern every other channel will follow.

## Inherited Context (from previous version's handoff)
- In-memory queue + background worker exists and processes enqueued notifications.

## Sub-Steps (check off as you commit)
- [ ] 1. Add a `mailpit` service to `docker-compose.yml`
- [ ] 2. Define `IEmailSender` interface in `Application`
- [ ] 3. Implement `MailpitEmailSender` in `Infrastructure` using `System.Net.Mail` or `MailKit` against local SMTP
- [ ] 4. Wire the worker to call `IEmailSender` when processing an email-type notification
- [ ] 5. Trigger a test notification and confirm delivery

## Theory to Know Going In
- Strategy pattern / provider abstraction
- SMTP protocol basics
- `IOptions<T>` config binding for provider settings

## Validation (from roadmap, copied in full)
- [ ] Email visible in the Mailpit UI (`localhost:8025`) after triggering a notification
