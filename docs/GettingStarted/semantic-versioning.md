# Semantic Versioning: Step‑by‑Step Dev Workflow

This guide explains exactly how to work in MentorSync with automated versioning, releases, and changelog updates powered by GitHub Actions.

## TL;DR

1. Create a branch → implement changes → commit using Conventional Commits.
2. For MINOR bump add `(MINOR)`; for MAJOR bump add `(MAJOR)` in the subject.
3. Open a PR to `master` → the PR bot comments the “next version”.
4. Merge PR → workflow tags the repo, creates a Release, and updates `CHANGELOG.md`.

## One‑Time Setup (maintainers)

-   Ensure a baseline tag exists (e.g., `v1.0.0`). If missing:
    ```bash
    git tag v1.0.0
    git push origin v1.0.0
    ```
-   Branch protection on `master` (recommended): require PRs and passing checks.

## Daily Development Flow

1. Branch

    - From `master`: `git checkout -b feat/scheduling-slots`

2. Commit with clear messages

    - Follow Conventional Commits and our bump flags.
    - Examples:
        - Patch: `fix: correct null check in schedule overlap`
        - Minor: `(MINOR) feat: add scheduling API endpoints`
        - Major: `(MAJOR) feat: rename /api/materials -> /api/content`

3. Open a PR to `master`

    - The workflow `.github/workflows/on-pr-future-version.yml` posts a comment like:
      “The next version will be v1.3.0”
    - If the version isn’t what you expect, adjust your commit messages (add/remove `(MINOR)`/`(MAJOR)`) and push again.

4. Merge the PR when approved

    - On merge, `.github/workflows/semantic-versioning.yml`:
        - Calculates the next SemVer
        - Tags the repo (prefix `v`)
        - Builds release notes/changelog
        - Creates a GitHub Release
        - Updates `CHANGELOG.md` (skips gracefully if empty)

5. Verify
    - Check Releases and Tags on GitHub
    - `CHANGELOG.md` gets a new section for the version

## Commit Rules That Drive Bumps

-   We use Conventional Commits plus explicit flags:
    -   MAJOR bump: add `(MAJOR)` in the subject
    -   MINOR bump: add `(MINOR)` in the subject
    -   PATCH bump: default when neither MAJOR nor MINOR is present and relevant code changed

Examples:

```bash
# PATCH
git commit -m "fix: prevent NRE in rating calculation"

# MINOR
git commit -m "(MINOR) feat: add mentor availability search"

# MAJOR
git commit -m "(MAJOR) feat: change auth scheme to JWT-only"
# (Optional) Put migration details in the body for reviewers/users
```

Notes:

-   The workflows search commit subjects (and body) for the `(MAJOR)` / `(MINOR)` flags.
-   We intentionally ignore docs-only changes: pushes that only modify `**/*.md`, `docs/**`, `CHANGELOG.md`, or `.github/dependabot*` won’t run the release workflow.

## Preview The Next Version (on PR)

-   On PR open, a comment shows the computed next version using the PR’s merge ref.
-   If inaccurate, normalize your commits and push again (squash/amend as needed).

## After Merge: What Gets Created

-   Tag: `vX.Y.Z` (or `vX.Y.Z-rcN` if pre-release suffix is enabled in the workflow)
-   GitHub Release with auto-generated notes
-   `CHANGELOG.md` entry prepended with the new version and changes

Tip: If you want plain tags without `-rcN`, remove the `version_format` line from `semantic-versioning.yml`.

## When A Release Won’t Trigger

-   Your push went to a branch other than `master`
-   Only docs changed (paths are ignored by design)
-   No commits since the last tag affected included paths

## Troubleshooting

-   PR has no “next version” comment

    -   Ensure the PR targets `master` and is in “opened” state.
    -   Re-open the PR or re-run the workflow from Actions if needed.

-   Release didn’t appear

    -   Check Actions logs for `semantic-versioning.yml` on the merge commit
    -   Confirm your commits used `(MINOR)`/`(MAJOR)` or that patch-worthy changes exist

-   Changelog step “nothing to commit”
    -   That’s OK—our script exits successfully when no content is produced.

## Cheat Sheet

```bash
# New feature (minor)
git commit -m "(MINOR) feat: add rating summary endpoint"

# Breaking change (major)
git commit -m "(MAJOR) feat: unify materials and resources domain"

# Bug fix (patch)
git commit -m "fix: correct pagination off-by-one"
```

## References

-   Workflow (releases): `.github/workflows/semantic-versioning.yml`
-   Workflow (PR preview): `.github/workflows/on-pr-future-version.yml`
-   Commit guide: `/.github/git-commit-instructions.md`
-   Semantic Versioning: https://semver.org/
-   Conventional Commits: https://www.conventionalcommits.org/
