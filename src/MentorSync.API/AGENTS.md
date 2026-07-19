# API host guidance

- Keep this project a thin composition root. Domain behavior belongs in its owning module.
- Register modules and cross-cutting services through the existing extension methods; preserve intentional middleware order in `Program.cs`.
- Map business routes through module `IEndpoint` implementations. Do not add controllers or duplicate module endpoints in the host.
- Apply authentication, authorization policies, antiforgery, CORS, rate limiting, exception handling, structured logging, OpenTelemetry, Swagger, health, and liveness behavior consistently with the existing extensions.
- Return the established problem/result shapes and preserve trace IDs. Never expose exception details, tokens, or connection data.
- When module registration changes, verify startup plus `dotnet test tests/MentorSync.ArchitectureTests/MentorSync.ArchitectureTests.csproj`.
