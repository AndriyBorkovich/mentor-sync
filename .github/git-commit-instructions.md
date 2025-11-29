## Commit Style Guide

We follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) with additional semantic-versioning flags used by the CI workflow.

Important: Your commit messages drive automatic version bumps and release creation. See `docs/GettingStarted/semantic-versioning.md` for details.

### Version Bump Rules (as configured in CI)

-   `(MAJOR)` prefix anywhere in the subject → MAJOR bump
    -   Example: `(MAJOR) feat: migrate auth to OAuth2`
-   `(MINOR)` prefix anywhere in the subject → MINOR bump
    -   Example: `(MINOR) feat: add materials recommendations`
-   No flag → defaults to PATCH when there are code changes
    -   Example: `fix: correct null check in handler`

Notes:

-   The flags `(MAJOR)` and `(MINOR)` are matched by the workflow and take precedence.
-   Without a flag, any conventional change (e.g., `fix:`) produces a PATCH bump; docs-only changes are typically ignored by CI filters.

### Conventional Types (recommended)

-   `feat:` new, backward-compatible functionality
-   `fix:` bug fix
-   `perf:`, `refactor:`, `build:`, `ci:`, `chore:`, `docs:`, `style:`, `test:` as appropriate

Combine flags with types if needed:

-   `(MAJOR) feat: init module`
-   `(MINOR) feat: add scheduling API`
-   `fix: handle token refresh race`

### Commit Message Format

```
<type>: <description>

<body (optional)>

<footer (optional)>
```

### Breaking Changes

Prefer using the `(MAJOR)` flag in the subject. Additionally, you may include the standard footer:

```
(MAJOR) feat: migrate authentication to OAuth2

BREAKING CHANGE: Legacy API key authentication is no longer supported.
Users must update their clients to use OAuth2 tokens.
```

### Best Practices

-   Use lowercase for the description
-   Keep description concise (50 characters or less)
-   Reference related issues in the body: `Fixes #123` or `Related to #456`
-   Be specific and descriptive about what changed
