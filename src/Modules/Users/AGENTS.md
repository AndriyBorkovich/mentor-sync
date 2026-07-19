# Users module guidance

- This module owns ASP.NET Identity users, authentication, JWT/refresh tokens, external login, roles, mentor/mentee profiles, account state, avatar handling, and the `users` PostgreSQL schema.
- Treat authentication and authorization changes as security-sensitive. Preserve issuer/audience/signing-key validation, refresh-token rotation/revocation, active-user checks, role policies, and generic login failure messages.
- Derive the acting user's identity from claims for self-service operations; never authorize ownership solely from route or request IDs.
- Passwords and tokens must never be logged or returned outside their established response fields. Use Identity APIs for password hashing, reset, confirmation, roles, and lockout behavior.
- Keep types required by other modules in `MentorSync.Users.Contracts`; do not expose Identity entities or the Users DbContext.
- Validate changes across anonymous, authenticated, inactive, wrong-role, owner, and admin cases as applicable.
