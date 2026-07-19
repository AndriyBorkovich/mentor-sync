# MentorSync Documentation

Welcome to the MentorSync project documentation. This folder is organized for easy navigation and discovery.

## 📁 Directory Structure

### 📚 **Architecture/**

Deep architectural documentation and design patterns

-   `overview.md` - Modular monolith architecture, module boundaries, and database design
-   `testing.md` - Comprehensive guide to ArchUnitNET architecture testing
-   `domains.json` - Architectural domains with required patterns and constraints

### 🎯 **Guides/**

Comprehensive guides and coding standards

#### **DomainDeepDives/**

In-depth technical guides for each architectural domain:

-   `frontend-ui.md` - React component architecture and patterns
-   `auth-flow.md` - Authentication and authorization flow
-   `state-management.md` - React state management hierarchy
-   `backend-cqrs.md` - Custom CQRS pattern implementation
-   `data-access.md` - API client and data layer patterns
-   `error-handling.md` - Global exception handling and RFC 7807
-   `performance-optimization.md` - Memoization, lazy loading, caching strategies

#### **CodingStandards/**

Per-category coding conventions and best practices:

-   `react-pages.md` - Page component patterns
-   `custom-hooks.md` - Custom React hooks
-   `feature-components.md` - Feature-specific components
-   `ui-components.md` - Reusable UI components
-   `react-contexts.md` - React Context patterns
-   `csharp-commands.md` - CQRS command pattern
-   `csharp-queries.md` - CQRS query pattern
-   `csharp-handlers.md` - Command/query handler patterns
-   `layout-components.md` - Layout component patterns
-   `frontend-services.md` - Frontend service layer
-   `csharp-endpoints.md` - Minimal API endpoint patterns

### 💻 **TechStack/**

Technology stack analysis

-   `overview.md` - Complete technology stack, dependencies, and versions

### 🏗️ **CodebaseStructure/**

Codebase organization and file categorization

-   `file-categorization.json` - 22 file categories with 130+ files cataloged

### 🚀 **GettingStarted/**

Build and development instructions

-   `build-instructions.md` - Setup, building, and development workflow

### 📦 **Modules/**

Feature module documentation

-   `Users module.md` - User account management and profiles
-   `Materials module.md` - Learning materials CRUD and metadata
-   `Ratings module.md` - Mentor and article review system
-   `Recommendations module.md` - ML.NET-based recommendation engine
-   `Scheduling (booking) module.md` - Session booking and availability
-   `Notifications module.md` - Push and email notifications

### ⚙️ **Config/**

Configuration and deployment guides

-   `Azure deployment.md` - Azure Container Apps deployment
-   `Migrations.md` - EF Core migrations and database setup
-   `Troubleshooting.md` - Common issues and solutions
-   `Users credentials.md` - User credentials and test accounts

### 📋 **Other Files**

-   `Functionality.md` - Feature requirements and user functionality
-   `MentorSync app.md` - Application overview
-   `Prompts.md` - AI assistant prompts and instructions

## 🎯 Quick Start

**New to MentorSync?** Start here:

1. Read `Guides/DomainDeepDives/` for your domain (frontend or backend)
2. Check `Guides/CodingStandards/` for specific patterns
3. Read the root `AGENTS.md` and the closest subtree-specific `AGENTS.md` for agent guidance

**Setting up locally?**

1. Follow `GettingStarted/build-instructions.md`
2. Check `Config/` for environment setup
3. Review `Config/Migrations.md` for database initialization

**Working on architecture:**

-   Reference `Architecture/overview.md` for module boundaries
-   Run architecture tests: `dotnet test tests/MentorSync.ArchitectureTests/`
-   Read `Architecture/testing.md` for constraint validation

## 🔄 Cross-References

Documentation files reference each other for easy navigation:

-   Root and subtree-specific `AGENTS.md` files link to the relevant documentation
-   Build instructions link to all guide categories
-   Architecture documents reference module documentation

## 📝 Contributing

When updating documentation:

1. Keep file names descriptive and lowercase with hyphens
2. Update cross-references in other files
3. Maintain the directory structure organization
4. Update this README if adding new categories

---

**Last Updated:** November 2025
