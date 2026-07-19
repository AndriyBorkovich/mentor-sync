# Ratings module guidance

- This module owns mentor reviews, material reviews, aggregates, and the `ratings` PostgreSQL schema.
- Keep mentor and material review flows distinct while sharing only genuinely identical domain behavior.
- Enforce the current rating range, one-review/uniqueness rules, reviewer ownership, eligibility, and admin moderation rules in server-side logic.
- Never accept reviewer identity from the request when it can be derived from authenticated claims.
- Use `MentorSync.Users.Contracts` and `MentorSync.Materials.Contracts` for referenced resource data; expose rating data needed elsewhere through `MentorSync.Ratings.Contracts`.
- When changing review writes, verify create/update/delete/check/list behavior plus aggregate values and authorization.
