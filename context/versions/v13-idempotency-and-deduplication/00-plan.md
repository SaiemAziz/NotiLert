---
version: v13
title: Idempotency & Deduplication
roadmap_steps: [13]
status: not-started
started: 
completed: 
---

# v13: Idempotency & Deduplication

## Goal (inlined from roadmap — no need to reopen it)
Add idempotency keys and a deduplication mechanism so redelivered messages (from broker retries or consumer crashes) never result in a duplicate real send, across all channels built so far.

## Inherited Context (from previous version's handoff)
- All 7 channels exist (Email, Discord, Telegram, SMS, Push, Slack, In-App) behind `INotificationChannel`.
- Durable broker delivers at-least-once (can redeliver).

## Sub-Steps (check off as you commit)
- [ ] 1. Add an `IdempotencyKey` column/table for tracking processed message IDs
- [ ] 2. Implement a dedup check in the worker before any channel send is attempted
- [ ] 3. Wire the check into the shared send path so it applies to all channels uniformly
- [ ] 4. Test by manually forcing a redelivery and confirming only one real send occurs

## Theory to Know Going In
- Idempotency in distributed, at-least-once delivery systems
- Exactly-once *effect* vs exactly-once *delivery* (the distinction matters)

## Validation (from roadmap, copied in full)
- [ ] Redelivering the same message twice results in exactly one real send, on any channel
