---
version: v08
title: Discord + Telegram Channels
roadmap_steps: [08]
status: not-started
started: 
completed: 
---

# v08: Discord + Telegram Channels

## Goal (inlined from roadmap — no need to reopen it)
Introduce the `INotificationChannel` abstraction and implement Discord and Telegram webhook senders behind it. This is the pattern every subsequent channel (SMS, Push, Slack, In-App) reuses.

## Inherited Context (from previous version's handoff)
- `IEmailSender` abstraction exists as a working example of the provider-abstraction pattern.
- Durable broker + worker pipeline exists.

## Sub-Steps (check off as you commit)
- [ ] 1. Define `INotificationChannel` interface in `Application/Channels/`
- [ ] 2. Implement `DiscordWebhookChannel`
- [ ] 3. Implement `TelegramWebhookChannel`
- [ ] 4. Implement a channel resolver/factory that picks the right implementation from a `channel` field on the request
- [ ] 5. Integration test: same payload shape, different `channel` value, routes correctly to each

## Theory to Know Going In
- Strategy pattern + Factory pattern
- Open/Closed Principle (adding a channel without modifying existing ones)

## Validation (from roadmap, copied in full)
- [ ] Same request body, different `channel` field, routes correctly to each provider
