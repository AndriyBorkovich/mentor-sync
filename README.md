# 🚀 MentorSync

[![Backend - build & test](https://github.com/AndriyBorkovich/mentor-sync/actions/workflows/backend-build-and-test.yml/badge.svg)](https://github.com/AndriyBorkovich/mentor-sync/actions/workflows/backend-build-and-test.yml)
[![UI - build & test](https://github.com/AndriyBorkovich/mentor-sync/actions/workflows/ui-build-and-test.yml/badge.svg)](https://github.com/AndriyBorkovich/mentor-sync/actions/workflows/ui-build-and-test.yml)
[![Semantic Versioning](https://github.com/AndriyBorkovich/mentor-sync/actions/workflows/semantic-versioning.yml/badge.svg)](https://github.com/AndriyBorkovich/mentor-sync/actions/workflows/semantic-versioning.yml)
[![Architecture Tests](https://img.shields.io/badge/architecture-tested-brightgreen)](tests/MentorSync.ArchitectureTests)

**MentorSync** is a comprehensive mentorship platform connecting mentors and mentees for effective learning, guidance, and professional development.

## 📋 Quick Navigation

-   🎯 [Features](#-features)
-   🏗️ [Architecture](#-architecture)
-   🛠️ [Tech Stack](#-tech-stack)
-   ⚡ [Quick Start](#-quick-start)
-   📚 [Documentation](#-documentation)
-   🤝 [Contributing](#-contributing)
-   📝 [Versioning](#-versioning)

## 🎯 Features

-   **Mentor-Mentee Matching** – Intelligent pairing based on skills, expertise, and interests
-   **Session Scheduling** – Interactive calendar for booking and managing mentorship sessions
-   **Real-time Messaging** – In-app chat for direct communication between mentors and mentees
-   **Learning Materials** – Curated library of resources and guidance documents shared by mentors
-   **Availability Management** – Mentors can set their availability and preferred mentoring hours
-   **Ratings & Reviews** – Transparent feedback system to build trust and quality assurance
-   **Smart Recommendations** – AI-driven suggestions for optimal mentor-mentee pairings

## 🏗️ Architecture

MentorSync follows a **Modular Monolith** architecture with independent feature modules:

### Core Modules

| Module              | Purpose                                          | Database Schema   |
| ------------------- | ------------------------------------------------ | ----------------- |
| **Users**           | Authentication, user profiles, skills management | `users`           |
| **Materials**       | Learning resources and knowledge base            | `materials`       |
| **Scheduling**      | Session booking and availability management      | `scheduling`      |
| **Ratings**         | Reviews and mentor ratings                       | `ratings`         |
| **Recommendations** | ML-driven pairing and suggestions                | `recommendations` |
| **Notifications**   | Email and in-app notifications                   | `notifications`   |

### Key Architectural Features

✅ **Module Independence** – Each module has isolated DbContext, entities, and features
✅ **Contracts-Only Dependencies** – Modules communicate through `.Contracts` projects only
✅ **Clear Boundaries** – Architecture tests validate module isolation
✅ **Scalability Path** – Modules can be extracted to microservices
✅ **CQRS Pattern** – Custom command/query handlers for explicit operation flow

## 🛠️ Tech Stack

### Backend

-   **Runtime**: .NET 9 with C# 13
-   **API Framework**: ASP.NET Core Minimal APIs
-   **Architecture**: Modular Monolith with CQRS + VSA
-   **ORM**: Entity Framework Core + PostgreSQL (NpSQL)
-   **Validation**: FluentValidation
-   **Authentication**: JWT with refresh tokens
-   **Service Orchestration**: .NET Aspire

### Frontend

-   **Framework**: React 18+ with TypeScript
-   **Build Tool**: Vite
-   **Styling**: TailwindCSS
-   **State Management**: React Context API + Custom Hooks
-   **HTTP Client**: Axios
-   **Notifications**: react-toastify
-   **Forms**: react-hook-form

### Infrastructure

-   **Database**: PostgreSQL (multi-schema per module)
-   **Hosting**: Azure Container Apps
-   **IaC**: Bicep templates
-   **CI/CD**: GitHub Actions
-   **Versioning**: Semantic Versioning with automated releases

## ⚡ Quick Start

### Prerequisites

-   .NET 9 SDK
-   Node.js 20+
-   Docker
-   Git

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/AndriyBorkovich/mentor-sync.git
    cd mentor-sync
    ```

2. **Navigate to Aspire host:**

    ```bash
    cd aspire/MentorSync.AppHost
    ```

3. **Run the application:**

    ```bash
    dotnet run
    ```

    This will start:

    - Backend API (http://localhost:5001)
    - Frontend UI (http://localhost:5173)
    - PostgreSQL database
    - All supporting services

4. **Access the application:**
    - **Frontend**: http://localhost:5173
    - **API Swagger**: http://localhost:5001/swagger
    - **Aspire Dashboard**: http://localhost:15177

## 📚 Documentation

Complete documentation is organized by topic:

### Getting Started

-   **[Build Instructions](docs/GettingStarted/build-instructions.md)** – Detailed setup guide
-   **[Semantic Versioning](docs/GettingStarted/semantic-versioning.md)** – Release and version strategy

### Architecture & Design

-   **[Architecture Overview](docs/Architecture/overview.md)** – System design and patterns
-   **[Architecture Testing](docs/Architecture/testing.md)** – Quality assurance approach
-   **[Modular Design](docs/Guides/DomainDeepDives/module-boundaries.md)** – Module structure

### Development Guides

-   **[React Components](docs/Guides/CodingStandards/react-pages.md)** – Frontend best practices
-   **[C# Commands & Queries](docs/Guides/CodingStandards/csharp-commands.md)** – Backend patterns
-   **[API Client & Services](docs/Guides/CodingStandards/api-client.md)** – HTTP integration
-   **[Error Handling](docs/Guides/CodingStandards/error-handling.md)** – Exception strategies

### Module Deep Dives

-   [Users Module](docs/Modules/Users%20module.md)
-   [Materials Module](docs/Modules/Materials%20module.md)
-   [Scheduling Module](<docs/Modules/Scheduling%20(booking)%20module.md>)
-   [Ratings Module](docs/Modules/Ratings%20module.md)
-   [Recommendations Module](docs/Modules/Recommendations%20module.md)
-   [Notifications Module](docs/Modules/Notifications%20module.md)

📖 **[Browse All Documentation](docs/README.md)**

## 🤝 Contributing

### Commit Convention

All commits follow [Conventional Commits](https://www.conventionalcommits.org/):

```bash
feat: add material recommendations        # MINOR version bump
fix: resolve session timeout              # PATCH version bump
break: migrate to OAuth2                  # MAJOR version bump
docs: update API documentation            # No version change
```

See **[Commit Guide](.github/git-commit-instructions.md)** for details.

### Development Workflow

1. **Create a feature branch** from `master`
2. **Make commits** with [conventional format](.github/git-commit-instructions.md)
3. **Push to GitHub** and create a pull request
4. **Run tests locally**:

    ```bash
    # Backend tests
    dotnet test tests/MentorSync.ArchitectureTests/

    # Frontend tests
    cd src/MentorSync.UI && npm test
    ```

5. **Merge to master** when approved
6. **Automatic release** – Semantic versioning creates release automatically

### Code Standards

-   **Architecture Tests**: Validate module boundaries and dependencies
-   **C# Guidelines**: See [Coding Standards](docs/Guides/CodingStandards/csharp-commands.md)
-   **React Guidelines**: See [Component Standards](docs/Guides/CodingStandards/react-pages.md)
-   **Linting**: ESLint + .editorconfig

## 📝 Versioning

MentorSync uses **[Semantic Versioning 2.0.0](https://semver.org/)**:

```
MAJOR.MINOR.PATCH
v1.2.3
```

-   **MAJOR**: Breaking changes (v1.0.0 → v2.0.0)
-   **MINOR**: New features (v1.0.0 → v1.1.0)
-   **PATCH**: Bug fixes (v1.0.0 → v1.0.1)

**Automatic Releases**: Versions are calculated from commit messages and released automatically via [GitHub Actions](.github/workflows/semantic-versioning.yml).

📋 **[View Release History](https://github.com/AndriyBorkovich/mentor-sync/releases)**

## 🔧 Project Structure

```
MentorSync/
├── src/
│   ├── Modules/              # Feature modules (Users, Materials, etc.)
│   ├── MentorSync.API/       # API entry point
│   ├── MentorSync.UI/        # React frontend
│   └── MentorSync.SharedKernel/
├── aspire/                   # .NET Aspire orchestration
├── tests/                    # Test projects
├── docs/                     # Comprehensive documentation
├── .github/
│   ├── workflows/            # GitHub Actions (CI/CD, releases)
│   └── git-commit-instructions.md
└── Directory.Build.props     # Global project configuration
```

## 🚀 CI/CD Pipeline

| Workflow                 | Trigger              | Purpose                                   |
| ------------------------ | -------------------- | ----------------------------------------- |
| **Backend Build & Test** | Push to any branch   | Compile and test .NET code                |
| **UI Build & Test**      | Push to any branch   | Build and test React app                  |
| **Semantic Versioning**  | Push to `master`     | Auto-calculate version and create release |
| **Azure Deploy**         | Manual / Release tag | Deploy to Azure Container Apps            |

## 📞 Support & Contact

-   **Issues**: [GitHub Issues](https://github.com/AndriyBorkovich/mentor-sync/issues)
-   **Discussions**: [GitHub Discussions](https://github.com/AndriyBorkovich/mentor-sync/discussions)
-   **Author**: [Andrii Borkovych](https://github.com/AndriyBorkovich)

## 📄 License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.

---
