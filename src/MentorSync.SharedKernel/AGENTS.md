# Shared kernel guidance

- Keep this project small and domain-neutral. Add code here only when multiple modules genuinely share the same technical contract or primitive.
- Do not add module entities, business workflows, module-specific DTOs, or dependencies on module implementation projects.
- Shared abstractions must remain backward compatible or be updated atomically across every consumer.
- Preserve the custom mediator, endpoint, domain-event, result, validation, pagination, logging, and registration conventions already exposed here.
- Avoid turning helpers into service locators or generic repositories. Prefer explicit DI and direct module-owned DbContext access.
- Because changes have a broad blast radius, run the full backend build/tests and architecture tests.
