# Materials module guidance

- This module owns learning materials, tags, attachments, blob-backed file operations, and the `materials` PostgreSQL schema.
- Preserve author/owner and admin authorization for create, update, delete, and attachment operations; do not trust owner IDs from client payloads.
- Validate uploads before storage: size, allowed content/type, filename handling, and material ownership. Generate server-controlled blob names and never expose storage credentials.
- Coordinate blob and database mutations so partial failures do not leave avoidable orphaned blobs or records; make cleanup retryable.
- Keep cross-module models/services in `MentorSync.Materials.Contracts`. Refer to users and ratings by contracts and stable IDs only.
- For listing/search, project only response fields, use `AsNoTracking`, and preserve pagination/filter semantics.
