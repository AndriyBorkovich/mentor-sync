# MentorSync Tech Stack Analysis

## Core Technology Analysis

### Architecture Pattern

-   **Backend Architecture**: **Modular Monolith** with feature-based module organization
    -   Each domain (Users, Materials, Scheduling, Ratings, Recommendations, Notifications) is a separate module
    -   Modules located in `src/Modules/{Domain}/`
    -   Each module has isolated DbContext, Entities, Features, and Services
    -   Modules share common kernel via `MentorSync.SharedKernel`
    -   Potential for future microservices migration (each module can become independent service)

### Primary Languages & Frameworks

-   **Backend**: C# (.NET 9) with ASP.NET Core WebAPI using Minimal APIs
-   **Frontend**: TypeScript with React and Vite
-   **Infrastructure**: Bicep for IaC, .NET Aspire for orchestration
-   **Database**: PostgreSQL multi-schema (one schema per module)

### Backend Frameworks & Libraries

-   **ASP.NET Core**: Web framework
-   **Entity Framework Core**: ORM for data access
-   **ASP.NET Identity**: Authentication with JWT & OAuth2 (Google, GitHub, LinkedIn)
-   **SignalR**: Real-time communication
-   **FluentValidation**: Request validation
-   **Custom CQRS Implementation**: Commands and Queries using ICommand, IQuery interfaces
-   **Ardalis.Result**: Standardized Result<T> pattern for responses
-   **OpenTelemetry**: Distributed tracing and monitoring
-   **ML.NET**: Machine learning for recommendations
-   **Azure Communication Services**: Email notifications
-   **Azure Blob Storage**: File/avatar storage

### Frontend Framework & Libraries

-   **React**: UI library (v18+)
-   **Vite**: Build tool and dev server
-   **TypeScript**: Static typing
-   **TailwindCSS**: Utility-first CSS framework
-   **React Router**: Client-side routing
-   **Custom API Service Layer**: Type-safe API client

### State Management Approach

-   **React Context API**: Global state management
-   **React Hooks**: Local component state (useState, useCallback, useMemo, useEffect)
-   **Custom Hooks**: Domain-specific logic (e.g., useUser, useAuth)

### Infrastructure & DevOps

-   **.NET Aspire**: Local development and cloud orchestration
-   **Bicep**: Infrastructure as Code for Azure resources
-   **GitHub Actions**: CI/CD pipelines
-   **Docker**: Containerization
-   **Azure Container Apps**: Deployment target
-   **Azure PostgreSQL Database**: Managed relational database
-   **Azure Key Vault**: Secrets management
-   **OpenTelemetry Exporters**: Distributed tracing

### Other Relevant Technologies

-   **Npgsql**: PostgreSQL ADO.NET provider
-   **Health Checks**: .NET built-in health check framework
-   **Rate Limiting**: Middleware for API protection
-   **CORS**: Cross-origin resource sharing configuration
-   **Problem Details (RFC 7807)**: Standardized API error responses

## Modular Monolith Architecture

### Overview

MentorSync uses a **modular monolith** architecture for the backend, organizing the application into independent feature modules while maintaining a single deployment unit.

### Module Organization

Each module is located in `src/Modules/{Domain}/` and contains:

```
src/Modules/{Domain}/
├── MentorSync.{Domain}/                    # Main module project
│   ├── Features/                           # Feature folders with CQRS operations
│   │   ├── Feature1/
│   │   │   ├── {Feature}Command.cs
│   │   │   ├── {Feature}CommandHandler.cs
│   │   │   ├── {Feature}CommandValidator.cs
│   │   │   ├── {Feature}Endpoint.cs
│   │   │   ├── {Feature}Request.cs
│   │   │   └── {Feature}Response.cs
│   │   └── Feature2/
│   ├── Data/
│   │   ├── {Domain}DbContext.cs            # Module-specific DbContext
│   │   ├── Configurations/                 # EF Core entity configurations
│   │   └── Migrations/
│   ├── Domain/                             # Domain entities and value objects
│   │   ├── {Entity}.cs
│   │   └── Events/
│   ├── Infrastructure/                     # Services and utilities
│   │   └── Services/
│   ├── ModuleRegistration.cs               # DI container registration
│   └── MentorSync.{Domain}.csproj
└── MentorSync.{Domain}.Contracts/          # Public API contracts
    ├── Models/                             # Shared DTOs exposed to other modules
    └── Services/                           # Interfaces for inter-module communication
```

### Core Modules

| Module              | Purpose                                   | DbContext                | Entities                                     |
| ------------------- | ----------------------------------------- | ------------------------ | -------------------------------------------- |
| **Users**           | User management, authentication, profiles | UsersDbContext           | User, UserSkill, UserAvailability            |
| **Materials**       | Learning materials, tags, attachments     | MaterialsDbContext       | LearningMaterial, MaterialAttachment, Tag    |
| **Scheduling**      | Session booking, availability management  | SchedulingDbContext      | Session, Availability, Booking               |
| **Ratings**         | Reviews and ratings system                | RatingsDbContext         | Rating, Review                               |
| **Recommendations** | ML-driven mentor and material suggestions | RecommendationsDbContext | MentorRecommendation, MaterialRecommendation |
| **Notifications**   | Email and notification delivery           | NotificationsDbContext   | Notification, NotificationLog                |

### Key Characteristics

1. **Independent DbContexts**: Each module has its own PostgreSQL schema

    - Schema naming: `{domain_lowercase}` (e.g., `users`, `materials`, `scheduling`)
    - Enforces data isolation at database level
    - Potential for independent scaling or migration to microservices

2. **Direct DbContext Access**: Handlers and services query DbContext directly

    - **No repository pattern**: Repositories add unnecessary abstraction layer
    - **LINQ queries**: All data access via EF Core LINQ directly on DbContext
    - **AsNoTracking()** for read-only queries to improve performance
    - **Select()** projections to DTOs instead of loading full entities

3. **Custom CQRS per Module**: All modules use custom ICommand/IQuery pattern

    - Commands for write operations
    - Queries for read operations
    - Handlers contain business logic
    - Result<T> pattern for all operations

4. **Minimal APIs per Module**: Each module registers its own IEndpoint implementations

    - Auto-discovery via reflection in Program.cs
    - Routes prefixed with module domain: `/api/{domain}/`
    - All endpoints require IMediator for command/query execution

5. **Shared Infrastructure**: Common across all modules

    - `MentorSync.SharedKernel`: Abstractions, base classes, utilities
    - `MentorSync.ServiceDefaults`: Service registration helpers
    - `MentorSync.API`: API host and middleware configuration

6. **Module Contracts**: Public interfaces for inter-module communication
    - Located in `MentorSync.{Domain}.Contracts` projects
    - Exposes service interfaces and DTOs
    - Consumed by other modules for cross-cutting concerns

### Benefits of Modular Monolith Approach

-   **Code Organization**: Related features grouped by domain
-   **Team Scalability**: Teams can work on modules independently
-   **Testing**: Modules can be tested in isolation
-   **Flexibility**: Easy migration path to microservices (each module can become a service)
-   **Maintainability**: Clear boundaries reduce complexity
-   **Shared Resources**: Development simplicity with single deployment
-   **Performance**: Direct function calls within monolith (no network latency)

### Inter-Module Communication

Modules communicate through:

1. **Event Publishing**: Domain events propagated through shared event bus
2. **Service Interfaces**: Injected contracts from other modules' Contracts projects
3. **Database References**: Foreign keys for related entities across modules
4. **API Calls**: Future capability when modules are extracted as microservices

## Domain Specificity Analysis

### Problem Domain

MentorSync is a **mentor-mentee matching and learning platform** that connects experienced professionals (mentors) with learners (mentees) in the IT field. It leverages machine learning to provide personalized mentor and material recommendations.

### Core Business Concepts

1. **Mentor-Mentee Relationships**: Matching based on skills, experience, and learning goals
2. **Skill Assessment**: Proficiency levels across 40+ programming languages and IT domains
3. **Availability Management**: Scheduling mentor availability slots with booking management
4. **Rating & Reviews**: Mentee feedback on mentor effectiveness
5. **Material Recommendations**: ML-driven suggestions for learning resources
6. **Industry Classification**: Categorization of mentors and mentees across 30+ IT industries
7. **User Roles**: Distinct mentor and mentee profiles with role-based access

### Core Mathematical/Business Concepts

-   **Recommendation Pipelines**: ML.NET models for mentor and material recommendations (trained models: `mentor_model.zip`, `material_model.zip`)
-   **Skill Matching Algorithms**: Proximity-based matching between mentor skills and mentee learning goals
-   **Rating Calculations**: Average ratings and review aggregation
-   **Availability Conflict Resolution**: Handling booking conflicts and cascade deletions

### User Interactions Supported

-   **Mentor Profile Management**: Bio, experience, skills, programming languages, availability slots, rates
-   **Mentee Profile Management**: Learning goals, current skills, industry interests
-   **Search & Discovery**: Search mentors by filters (skills, experience, rating, availability)
-   **Booking Management**: Schedule sessions, view history, cancel bookings
-   **Material Management**: Upload, categorize, and recommend learning materials
-   **Notifications**: Email notifications for bookings, confirmations, messages
-   **Rating & Reviews**: Post-session feedback and mentor evaluation

### Primary Data Types & Structures

-   **User**: AppUser with identity claims, roles, profile image (avatar), active status
-   **MentorProfile**: Bio, skills, experience, availability, rates, industry focus
-   **MenteeProfile**: Learning goals, current skills, industry focus
-   **Availability**: Time slots with duration, mentee restrictions, booking references
-   **Booking**: Session scheduling linking mentee, mentor, availability slot
-   **Rating**: Review score, comments, reviewer identity
-   **Material**: Learning resources (markdown content, attachments), categorization, author
-   **Recommendation**: ML-generated suggestions with confidence scores

### Data Organization

-   **Multi-schema PostgreSQL**: Separate schemas for Users, Scheduling, Materials, Ratings, Recommendations
-   **MongoDB**: Potential secondary storage for unstructured recommendation data
-   **Azure Blob Storage**: Avatar images and material attachments organized by user/material ID

## Application Boundaries

### Features Clearly Within Scope

-   User registration, authentication (JWT, OAuth2), profile management
-   Mentor availability management (create, edit, delete slots)
-   Booking system with conflict detection and cascade handling
-   Skill-based mentor discovery and search
-   Material upload, categorization, and recommendation
-   Rating and review system
-   Real-time notifications (email via Azure Communication Services)
-   User activation/deactivation management
-   Avatar upload/deletion with cloud storage

### Features Architecturally Inconsistent

-   Features requiring real-time messaging beyond email (would require WebSocket expansion beyond SignalR usage)
-   Payment processing (not designed in current architecture - mentioned in docs but not implemented)
-   Video conferencing integration (no streaming infrastructure)
-   Advanced analytics dashboards (no BI tool integration)
-   Multi-tenant support (single-tenant architecture assumed)

### Specialized Libraries & Domain Constraints

-   **ML.NET Pipelines**: Requires trained model files, constrained to recommendation domain
-   **Custom CQRS**: All features must follow Command/Query separation pattern
-   **Result<T> Pattern**: All operations return standardized Result objects
-   **Role-Based Authorization**: Features must respect Mentor/Mentee/Admin roles
-   **Multi-schema Database Design**: Features must isolate data by domain schema
-   **Minimal APIs**: All endpoints must use ASP.NET Minimal API conventions, not traditional controller-based routing

## Architectural Constraints

### Must-Follow Patterns

1. **CQRS via Custom Handlers**: Commands implement `ICommandHandler<TCommand, TResponse>`, Queries implement `IQueryHandler<TQuery, TResponse>`
2. **Result-Based Returns**: All operations wrap responses in `Result<T>` from Ardalis.Result
3. **Endpoint Mapping**: All routes via `IEndpoint` interface with `MapEndpoint(IEndpointRouteBuilder)`
4. **Dependency Injection**: Constructor-based DI with records for commands/queries
5. **File-Scoped Namespaces**: All new code uses file-scoped namespace declarations
6. **OpenTelemetry Tracing**: All services must be instrumented for distributed tracing
7. **Health Checks**: Services must expose `/health` and `/alive` endpoints
8. **Error Handling**: Global exception handler with trace IDs and structured problem details
9. **Rate Limiting**: API endpoints protected by configurable rate limiting middleware

### Constraints Preventing Architectural Conflicts

-   **No Direct Database Access**: All data access through EF Core and DbContext
-   **No Service-to-Service Calls Beyond Service Discovery**: Use .NET Aspire service discovery for inter-service communication
-   **No Hardcoded Configuration**: All settings via app configuration and Key Vault
-   **No Synchronous Long-Running Operations**: Use background jobs for heavy computation
-   **No Role Elevation Without Audit**: Authorization middleware tracks all access attempts
