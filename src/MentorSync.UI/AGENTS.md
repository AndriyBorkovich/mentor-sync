# Frontend guidance

This file applies to the React frontend. It supplements the repository-level guidance.

## Current stack and structure

- Use the versions declared in `package.json` (currently React 19, React Router 7, TypeScript 5.8, Vite 7, Tailwind CSS 4); do not rely on older version notes in documentation.
- Keep domain code under `src/features/<domain>/` using the existing `components`, `hooks`, `pages`, `services`, and `types` organization.
- Put reusable application-wide UI in `src/components`, shared API/infrastructure code in `src/shared`, and route composition in `src/routes.tsx`.
- Use function components and explicit prop types. Follow existing export and naming conventions in the nearest feature.

## State, API, and routing

- Keep truly global state in a focused Context, reusable feature behavior in hooks, and presentation-only state inside components.
- Do not call HTTP endpoints directly from page or presentational components. Add domain service functions that use `src/shared/services/api.ts`.
- Preserve the centralized Axios authentication/error behavior; do not create competing Axios instances.
- Use `VITE_API_URL` for the API base URL and never embed environment-specific origins or credentials.
- Reuse `ProtectedRoute`, `RoleBasedRoute`, and `AuthContext`; match backend authorization policies when exposing UI actions.
- Add routes through the existing `createBrowserRouter` configuration and preserve lazy/error/loading behavior around affected routes.

## Styling and behavior

- Prefer the project's Tailwind utilities and shared CSS variables/components. Existing scoped CSS is valid; do not perform unrelated styling migrations.
- Keep UI accessible: semantic elements, associated labels, keyboard operation, meaningful alt text, and visible focus states.
- Show user-facing failures through the existing notification/form patterns and avoid leaking sensitive technical details.
- Memoize only when referential stability or measured rendering cost justifies it; do not add `useMemo`/`useCallback` mechanically.

## TypeScript and React conventions

- Treat `tsconfig.app.json`, `eslint.config.ts`, and `.prettierrc.json` as executable style rules. TypeScript is strict: do not suppress errors with `any`, `@ts-ignore`, unused declarations, or unchecked side-effect imports.
- Let Prettier control whitespace and wrapping. Use the repository's configured tabs/width and avoid formatting unrelated files.
- Use `PascalCase.tsx` for components and pages, PascalCase for component/type names, camelCase for functions and variables, `use<Name>` for hooks, and `<domain>Service.ts` for API services.
- Define explicit props and API boundary types. Prefer string-literal unions for small closed UI states and `unknown` plus narrowing for caught/untrusted values rather than unsafe casts.
- Keep components focused on rendering and interaction. Move reusable stateful behavior into hooks, transport logic into services, and shared visual primitives into `src/components`.
- Follow the Rules of Hooks and keep dependency arrays complete. Effects must clean up subscriptions, timers, abortable requests, and SignalR listeners they create; do not use effects to derive values that can be computed during render.
- Do not mutate props, Context values, or React state. Use functional state updates when the next value depends on the previous value, and keep unrelated state separate.
- Use stable domain IDs as list keys; do not use array indexes when items can be inserted, deleted, reordered, or filtered.
- Prefer controlled inputs and the existing `react-hook-form` patterns for non-trivial forms. Keep validation messages close to their fields and align client validation with backend rules without treating it as a security boundary.
- Await or deliberately handle every promise. Model loading, empty, success, and error states explicitly, and prevent stale async responses from overwriting newer state.
- Preserve Fast Refresh compatibility: component modules should primarily export React components; move shared constants/helpers into separate modules when ESLint warns.
- Avoid new `console.log` debugging. Logging that remains must redact tokens and personal data; user-facing failures go through the established toast/form error patterns.
- Use semantic HTML before ARIA, type button elements correctly, associate labels with controls, and keep interactions keyboard accessible.

## Validation

Run from `src/MentorSync.UI`:

```powershell
npm ci
npm run lint
npm run build
npm test --if-present
```

Use `npm install` only when intentionally changing dependencies or the lock file. For UI behavior changes, manually exercise the affected route and authentication/role state when possible.
