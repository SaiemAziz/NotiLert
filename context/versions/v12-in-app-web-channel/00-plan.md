---
version: v12
title: In-App / Web Notification Channel
roadmap_steps: [12]
status: not-started
started: 
completed: 
---

# v12: In-App / Web Notification Channel

## Goal (inlined from roadmap — no need to reopen it)
Treat the dashboard's own live feed as just another channel — notifications get stored and exposed via a feed endpoint. This groundwork gets reused later when the real-time dashboard UI is built.

## Inherited Context (from previous version's handoff)
- `INotificationChannel` abstraction + resolver exist.
- 4 external channels already implemented (Email, Discord, Telegram, SMS/Push/Slack as applicable).

## Sub-Steps (check off as you commit)
- [ ] 1. Implement `InAppChannel` storing the notification into a feed table/store
- [ ] 2. Add a `GET /feed` endpoint to retrieve a user's in-app notifications
- [ ] 3. Test that a triggered notification appears via the feed endpoint

## Theory to Know Going In
- Treating your own UI as a delivery channel — channel symmetry

## Validation (from roadmap, copied in full)
- [ ] A triggered notification appears in the in-app feed endpoint, independent of any dashboard UI (which doesn't exist yet)
