# Semantic Versioning Strategy

This document outlines how MentorSync uses semantic versioning for automated releases and version management.

## Overview

MentorSync follows **[Semantic Versioning](https://semver.org/)** (SemVer) 2.0.0 specification:

```
MAJOR.MINOR.PATCH
v1.2.3
```

-   **MAJOR** (X.0.0): Breaking changes that require user action
-   **MINOR** (X.Y.0): New features (backward compatible)
-   **PATCH** (X.Y.Z): Bug fixes and non-breaking improvements

## Commit Convention & Version Bumping

All commits to `master` follow [Conventional Commits](https://www.conventionalcommits.org/) format. The GitHub Actions workflow automatically calculates and creates releases based on commit message patterns.

### Commit Types and Version Impact

| Commit Type         | Pattern               | Version Impact    | Example                                   |
| ------------------- | --------------------- | ----------------- | ----------------------------------------- |
| **Breaking Change** | `break:`, `breaking:` | **MAJOR** ↑       | `break: migrate authentication to OAuth2` |
| **Feature**         | `feat:`, `feature:`   | **MINOR** ↑       | `feat: add material recommendations`      |
| **Bug Fix**         | `fix:`                | **PATCH** ↑       | `fix: resolve session timeout issue`      |
| **Documentation**   | `docs:`               | No version bump   | `docs: update API endpoints`              |
| **Chore**           | `chore:`              | No version bump   | `chore: update dependencies`              |
| **Refactor**        | `refactor:`           | No version bump\* | `refactor: improve database queries`      |
| **Performance**     | `perf:`               | **PATCH** ↑\*     | `perf: optimize image loading`            |
| **Style**           | `style:`              | No version bump   | `style: format code`                      |
| **Testing**         | `test:`               | No version bump   | `test: add unit tests`                    |
| **CI/CD**           | `ci:`                 | No version bump   | `ci: update workflow`                     |
| **Infrastructure**  | `infrastructure:`     | **PATCH** ↑\*     | `infrastructure: upgrade database`        |

**\* Performance and infrastructure changes bump PATCH version if they impact users or deployment.**

## Workflow: `.github/workflows/semantic-versioning.yml`

### When It Runs

-   **Trigger**: Push to `master` branch
-   **Ignores**: Markdown files, docs directory, dependabot updates
-   **Frequency**: On every commit that matches the trigger

### What It Does

1. **Analyzes** all commits since the last version tag
2. **Determines** the next version based on commit types
3. **Creates** a GitHub Release with:
    - Version tag (e.g., `v1.2.3`)
    - Automated release notes
    - Reference to commit convention

### Outputs

The workflow provides:

-   `new_version`: The calculated semantic version (e.g., `1.2.3`)
-   `release_created`: Boolean indicating if release was created
-   Automatically tagged commit with release information

## Examples

### Example 1: Feature Release

```bash
git commit -m "feat: add mentor availability calendar"
# Result: v1.3.0 (MINOR bump)
```

### Example 2: Bug Fix Release

```bash
git commit -m "fix: resolve session timeout issue"
# Result: v1.2.4 (PATCH bump)
```

### Example 3: Breaking Change Release

```bash
git commit -m "break: migrate authentication to OAuth2

BREAKING CHANGE: Legacy API key authentication is no longer supported"
# Result: v2.0.0 (MAJOR bump)
```

### Example 4: Multiple Commits (One Release)

```bash
git commit -m "feat: add notification preferences"
git commit -m "fix: resolve email delivery bug"
git commit -m "docs: update notification docs"
# Result: v1.4.0 (MINOR from feat, PATCH from fix, docs ignored)
# HIGHEST version bump wins
```

## Release Process

### For Contributors

1. **Commit** using [Conventional Commits](/.github/git-commit-instructions.md) format
2. **Push** to a feature branch
3. **Create PR** with meaningful description
4. **Merge** to `master` when approved
5. **Automatic Release** - GitHub Actions creates the release automatically

### For Release Managers

No manual steps required! The workflow is fully automated. Just ensure:

-   Commits follow the convention
-   PR descriptions are clear
-   Tests pass before merging

### Monitoring Releases

-   **View Releases**: [GitHub Releases](https://github.com/AndriyBorkovich/mentor-sync/releases)
-   **View Tags**: [GitHub Tags](https://github.com/AndriyBorkovich/mentor-sync/tags)
-   **Check Workflow**: [GitHub Actions](https://github.com/AndriyBorkovich/mentor-sync/actions/workflows/semantic-versioning.yml)

## Best Practices

### ✅ DO

-   **Use conventional commit format** for all commits to `master`
-   **Be specific** with commit messages (e.g., "fix: resolve email validation regex")
-   **Use breaking change footer** for MAJOR version bumps
-   **Reference issues** in commit body: `Fixes #123`
-   **Write clear PR descriptions** explaining what changed and why

### ❌ DON'T

-   Don't commit directly to `master` - always use PRs
-   Don't use generic messages like "update code" or "fixes"
-   Don't mix multiple concerns in one commit
-   Don't forget the commit type prefix

## Examples of Good Commits

```bash
# Feature with issue reference
git commit -m "feat: add session recovery mechanism

Implements automatic session recovery for interrupted connections.
Fixes #188"

# Bug fix with explanation
git commit -m "fix: resolve race condition in material caching

Modified cache invalidation to use locks during concurrent updates."

# Breaking change with migration guide
git commit -m "break: require JWT token in all API requests

BREAKING CHANGE: API now requires Authorization header with valid JWT.
Migration: Update client to use refreshed tokens."

# Infrastructure update
git commit -m "infrastructure: upgrade PostgreSQL to 16

Includes schema optimization and performance improvements."
```

## Version History Format

Each release includes:

-   **Tag**: `vX.Y.Z` (e.g., `v1.2.3`)
-   **Release Name**: `Release vX.Y.Z`
-   **Notes**: Automated description with:
    -   Version number and date
    -   List of commit types included
    -   Link to commit convention
    -   Semantic versioning explanation

## Troubleshooting

### Release Not Created

**Problem**: Commits were pushed but no release created.

**Causes**:

-   Commit message doesn't follow convention
-   Push was to a branch other than `master`
-   Commits only have type prefixes that don't bump versions (e.g., `docs:`, `style:`)

**Solution**: Check workflow logs in [GitHub Actions](https://github.com/AndriyBorkovich/mentor-sync/actions)

### Wrong Version Calculated

**Problem**: Version bumped incorrectly.

**Causes**:

-   Incorrect commit type prefix
-   Typo in commit message pattern

**Solution**: Create a new commit with correct type to bump version accordingly

### Manual Version Override

If manual intervention is needed:

1. Create annotated tag manually: `git tag -a v1.5.0 -m "Release v1.5.0"`
2. Push tag: `git push origin v1.5.0`
3. Create release on GitHub with notes

## Integration with CI/CD

Future integrations (planned):

-   [ ] Auto-bump version in `package.json` / `csproj` files
-   [ ] Generate changelog from commits
-   [ ] Create release artifacts
-   [ ] Trigger deployment on release
-   [ ] Notify teams on new releases

## References

-   [Semantic Versioning Spec](https://semver.org/)
-   [Conventional Commits](https://www.conventionalcommits.org/)
-   [MentorSync Commit Guide](/.github/git-commit-instructions.md)
-   [GitHub Releases Documentation](https://docs.github.com/en/repositories/releasing-projects-on-github/managing-releases-in-a-repository)
