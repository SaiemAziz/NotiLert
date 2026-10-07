---
version: v09
title: SMS Channel (Mock + Twilio Trial)
roadmap_steps: [09]
status: not-started
started: 
completed: 
---

# v09: SMS Channel (Mock + Twilio Trial)

## Goal (inlined from roadmap — no need to reopen it)
Add an SMS channel — first a local mock gateway for zero-cost iteration, then a real free-trial provider (Twilio) wired in behind the same abstraction.

## Inherited Context (from previous version's handoff)
- `INotificationChannel` abstraction + channel resolver exist (from Discord/Telegram version).

## Sub-Steps (check off as you commit)
- [ ] 1. Implement `ISmsSender` and a `MockSmsSender` that logs to console/file
- [ ] 2. Implement `TwilioSmsSender` behind the same interface, config-gated
- [ ] 3. Wire `SmsChannel : INotificationChannel` into the resolver
- [ ] 4. Test both the mock path and (if trial account set up) the real Twilio path

## Theory to Know Going In
- Provider abstraction reused for a genuinely new channel type
- Sandbox/trial credential handling

## Validation (from roadmap, copied in full)
- [ ] A test SMS is either logged by the mock, or actually received on a real phone via the trial account
