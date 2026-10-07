---
version: v07
title: Pub/Sub Broadcast Channel
roadmap_steps: [07]
status: not-started
started: 
completed: 
---

# v07: Pub/Sub Broadcast Channel

## Goal (inlined from roadmap — no need to reopen it)
Implement a publish/subscribe broadcast pattern (e.g. a 'system announcement' fanned out to all subscribers of a topic), contrasted against the point-to-point queue from the previous version.

## Inherited Context (from previous version's handoff)
- Durable broker exists and is used for point-to-point notification delivery.

## Sub-Steps (check off as you commit)
- [ ] 1. Define a topic/exchange for broadcast-type messages (fanout exchange if RabbitMQ)
- [ ] 2. Implement a publisher for the broadcast notification type
- [ ] 3. Implement 2+ independent consumer subscriptions to prove fan-out
- [ ] 4. Test that one publish reaches every subscriber without manual duplication logic

## Theory to Know Going In
- Publish/Subscribe pattern
- Fan-out vs point-to-point messaging
- Topic/exchange vs queue semantics

## Validation (from roadmap, copied in full)
- [ ] One publish reaches N independent subscribers without per-subscriber dedup logic
