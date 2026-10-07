---
version: v04
title: In-Memory Queue & Background Worker
roadmap_steps: [04]
status: not-started
started: 
completed: 
---

# v04: In-Memory Queue & Background Worker

## Goal (inlined from roadmap — no need to reopen it)
Introduce `System.Threading.Channels` as an in-process producer-consumer queue, with a `BackgroundService` consuming it. This is the first async-processing building block — later versions swap the in-memory channel for a real broker without changing this pattern's shape.

## Inherited Context (from previous version's handoff)
- Config/secrets pattern established.
- Notification entity + DB access exists.

## Sub-Steps (check off as you commit)
- [ ] 1. Define a `NotificationQueue` wrapper around `System.Threading.Channels.Channel<T>`, registered as a singleton
- [ ] 2. Implement a `NotificationWorker : BackgroundService` that reads from the channel in a loop
- [ ] 3. Wire the `POST /notifications` endpoint to enqueue instead of processing synchronously
- [ ] 4. Add structured logging around enqueue and dequeue/processing
- [ ] 5. Manually load-test with rapid sequential requests to confirm the API thread never blocks

## Theory to Know Going In
- Producer-consumer pattern
- Bounded vs unbounded channels, backpressure
- `IHostedService` / `BackgroundService` lifecycle

## Validation (from roadmap, copied in full)
- [ ] `POST /notifications` returns 202 Accepted instantly
- [ ] Background worker logs processing 1-2s later, decoupled from the request thread
- [ ] 100 rapid requests do not block the API thread
