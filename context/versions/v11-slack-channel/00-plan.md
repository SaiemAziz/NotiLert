---
version: v11
title: Slack Channel
roadmap_steps: [11]
status: not-started
started: 
completed: 
---

# v11: Slack Channel

## Goal (inlined from roadmap — no need to reopen it)
Add a Slack channel via incoming webhook, reusing the webhook pattern already built for Discord — a deliberately small step to reinforce how cheap new channels become once the abstraction exists.

## Inherited Context (from previous version's handoff)
- `INotificationChannel` abstraction + resolver exist.
- `DiscordWebhookChannel` exists as a near-identical webhook pattern to reuse.

## Sub-Steps (check off as you commit)
- [ ] 1. Implement `SlackWebhookChannel`, reusing/extracting a shared webhook base if it makes sense
- [ ] 2. Wire into the channel resolver
- [ ] 3. Test a message appears in a test Slack channel

## Theory to Know Going In
- Reinforcing the Strategy pattern — new channel, near-zero new architecture
- DRY via a shared webhook base where appropriate

## Validation (from roadmap, copied in full)
- [ ] Message appears correctly in a test Slack channel
