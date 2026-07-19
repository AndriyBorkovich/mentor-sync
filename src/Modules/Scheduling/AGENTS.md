# Scheduling module guidance

- This module owns mentor availability, bookings, booking status transitions, and the `scheduling` PostgreSQL schema.
- Store and compare instants consistently; make timezone assumptions explicit at API boundaries. Do not mix local and UTC values implicitly.
- Enforce mentor ownership of availability and mentee/mentor access to bookings from authenticated claims.
- Booking creation and status changes must protect against overlapping slots, double booking, invalid transitions, and concurrent updates. Keep the check and write atomic where possible.
- Send notifications through `MentorSync.Notifications.Contracts` and query user information only through `MentorSync.Users.Contracts`; do not reference those implementations.
- Test boundary times, overlap cases, cancellation/status transitions, authorization roles, and concurrency-sensitive paths.
