## Commit Style Guide

We follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) style for our commit messages.

**Important**: Your commit messages directly impact automated semantic versioning and release creation. See [Semantic Versioning Strategy](../../docs/GettingStarted/semantic-versioning.md) for how commits translate to version bumps.

### Commit Types and Examples

-   `feat: add new user authentication module` → MINOR version bump
-   `fix: resolve issue with data fetching` → PATCH version bump
-   `break: migrate authentication to OAuth2` → MAJOR version bump
-   `docs: update README with installation instructions` → No version change
-   `style: format code with Prettier` → No version change
-   `refactor: improve performance of data processing` → No version change
-   `test: add unit tests for user service` → No version change
-   `chore: update dependencies` → No version change
-   `ci: add GitHub Actions workflow for CI/CD` → No version change
-   `perf: optimize image loading speed` → PATCH version bump
-   `infrastructure: set up database migrations` → PATCH version bump

### Commit Message Format

```
<type>: <description>

<body (optional)>

<footer (optional)>
```

### Breaking Changes

For commits that introduce breaking changes, add a footer:

```
break: migrate authentication to OAuth2

BREAKING CHANGE: Legacy API key authentication is no longer supported.
Users must update their clients to use OAuth2 tokens.
```

### Best Practices

-   Use lowercase for the description
-   Keep description concise (50 characters or less)
-   Reference related issues in the body: `Fixes #123` or `Related to #456`
-   Be specific and descriptive about what changed
