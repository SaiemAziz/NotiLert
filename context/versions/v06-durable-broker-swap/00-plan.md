---
version: v06
title: Durable Broker Swap (RabbitMQ/Redis Streams)
roadmap_steps: [06]
status: not-started
started: 
completed: 
---

# v06: Durable Broker Swap (RabbitMQ/Redis Streams)

## Goal (inlined from roadmap — no need to reopen it)
Replace the in-memory `System.Threading.Channels` queue with a real, durable message broker (RabbitMQ or Redis Streams). This is the step that actually makes the system survive a crash/restart without losing messages.

## Inherited Context (from previous version's handoff)
- `NotificationQueue` abstraction exists (in-memory) feeding `NotificationWorker`.
- `IEmailSender` + `MailpitEmailSender` exist.

## Sub-Steps (check off as you commit)
- [ ] 1. Add the chosen broker service to `docker-compose.yml` (RabbitMQ recommended for this step's AMQP concepts; Redis Streams is the documented alternative)
- [ ] 2. Install the client library (`RabbitMQ.Client` or `StackExchange.Redis`)
- [ ] 3. Implement a publisher replacing the in-memory channel's write side
- [ ] 4. Implement a consumer replacing `NotificationWorker`'s read side, with explicit ack/nack
- [ ] 5. Test durability: kill the API process mid-processing, restart, confirm the message still gets delivered

## Theory to Know Going In
- Durable message queues vs in-memory queues
- At-least-once delivery semantics
- Ack/nack and message acknowledgment patterns

## Validation (from roadmap, copied in full)
- [ ] Killing the API mid-processing and restarting still results in delivery (durability proof)
- [ ] Broker's management UI (RabbitMQ) shows queue depth accurately
