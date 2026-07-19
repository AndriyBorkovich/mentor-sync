# Notifications module guidance

- This module owns email delivery, the email outbox/background job, SignalR notifications/chat, MongoDB persistence, and notification contracts.
- Treat send operations as at-least-once: handlers and processors must be idempotent or safely deduplicate retries. Persist outbox state before acknowledging work.
- Do not hold scoped DbContext/Mongo services across background-job iterations; create scopes per unit of work and propagate cancellation.
- Never log message bodies, tokens, connection strings, or sensitive recipient data. Keep provider details behind existing interfaces.
- Authorize SignalR hubs/chat endpoints and validate that the current user belongs to the requested room; do not trust client-provided sender IDs.
- Keep commands/events used by other modules in `MentorSync.Notifications.Contracts`; provider implementations remain internal to this module.
- Test retry/failure transitions, duplicate processing, cancellation, recipient authorization, and unavailable-provider behavior.
