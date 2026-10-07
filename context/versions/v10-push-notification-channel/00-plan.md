---
version: v10
title: Push Notification Channel
roadmap_steps: [10]
status: not-started
started: 
completed: 
---

# v10: Push Notification Channel

## Goal (inlined from roadmap — no need to reopen it)
Add a push notification channel via OneSignal or Firebase Cloud Messaging (free tier), introducing device-token-based delivery.

## Inherited Context (from previous version's handoff)
- `INotificationChannel` abstraction + resolver exist.
- SMS channel exists as the most recent example of adding a new provider.

## Sub-Steps (check off as you commit)
- [ ] 1. Implement `IPushSender`
- [ ] 2. Register a OneSignal or Firebase app in their dashboard (manual)
- [ ] 3. Implement `PushSender` calling the chosen provider's API
- [ ] 4. Add a simple device-token registration endpoint
- [ ] 5. Test a push notification on a registered test device/browser

## Theory to Know Going In
- Third-party push infrastructure integration
- Device tokens and registration flow

## Validation (from roadmap, copied in full)
- [ ] A test push notification is received on a registered test device/browser
