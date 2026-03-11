# PR Review Assistant

AI-powered code review assistant that analyzes code diffs and provides categorized, severity-rated feedback — built with 90%+ AI-generated code using GitHub Copilot.

## What This App Does

PR Review Assistant helps developers get instant feedback on code changes before submitting a pull request. Paste a code diff, select the programming language, and the AI mock service analyzes it for bugs, naming issues, performance problems, security vulnerabilities, and best practice violations. Each finding includes a severity level, confidence score, description, suggestion, and optional inline fix.

**Working features:**
- Submit a code diff for instant AI analysis (all 7 supported languages)
- Findings categorized: Bug, Naming, Performance, Security, CodeStyle, BestPractice
- Severity levels: Critical (red), Warning (orange), Info (blue) with confidence scores
- Collapsible suggested-fix panels on each finding
- Filter results by severity and/or category
- Review history persisted in PostgreSQL, browsable with pagination
- Admin panel: manage reviews (view, delete) and toggle mock AI mode
- Full Docker Compose setup — one command to start everything

## Technical Overview

### Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Frontend | React + Vite + TypeScript (strict) | React 18, Vite 6, TS 5 |
| HTTP | Axios | latest |
| Routing | React Router | v6 |
| Backend | ASP.NET Core Minimal API | .NET 9 |
| CQRS | MediatR | latest |
| Validation | FluentValidation | latest |
| ORM | EF Core + Npgsql | 9 |
| Database | PostgreSQL | 16 |
| Container | Docker + Compose | — |
| CI | GitHub Actions | — |

### Folder Structure

```
ai-pr-review-assistant/
├── frontend/                        # React + Vite + TypeScript
│   └── src/
│       ├── components/
│       │   ├── common/              # ErrorMessage, LoadingSpinner, SeverityBadge, ConfidenceBadge, ErrorBoundary
│       │   └── review/              # FindingCard, FindingsList, ReviewSummary, SuggestedFixPanel
│       ├── hooks/                   # useReviews, useReviewDetail, useSettings, useCreateReview, useDeleteReview
│       ├── layouts/                 # MainLayout, AdminLayout
│       ├── pages/                   # HomePage, ReviewPage, ReviewResultPage, HistoryPage, AdminSettingsPage
│       ├── router/                  # React Router v6 config
│       ├── services/                # Axios API client (api.ts)
│       ├── models/                  # TypeScript interfaces & types
│       └── styles/                  # Global CSS variables + design tokens
├── backend/
│   ├── src/PRReviewAssistant.API/
│   │   ├── Domain/                  # Entities (Review, Finding, AppSettings), enums
│   │   ├── Features/                # MediatR Commands + Queries + Handlers + Validators
│   │   ├── Infrastructure/          # AppDbContext, Repositories, MockAiAnalysisService
│   │   ├── Endpoints/               # Minimal API route maps
│   │   ├── Shared/                  # DTOs, ApiResponse<T>, GlobalExceptionHandler
│   │   └── Program.cs               # DI, middleware, Swagger
│   └── tests/PRReviewAssistant.Tests/
├── infra/                           # nginx.conf
├── docs/                            # ai-workflow.md, INSIGHTS.md
├── prompts/                         # AI prompt logs ({N}_{short-key}.md)
├── .github/                         # Copilot instructions, plan, CI workflow
├── docker-compose.yml
├── .env.example
└── README.md
```

### Running with Docker (recommended)

```bash
cp .env.example .env          # edit passwords as needed
docker compose up -d
```

| Service | URL |
|---------|-----|
| Frontend | http://localhost:3000 |
| Backend API + Swagger | http://localhost:8080/swagger |
| PostgreSQL | localhost:5432 |

### Running Locally (without Docker)

**Prerequisites:** .NET 9 SDK, Node.js 20+, PostgreSQL 16

**Backend:**
```bash
cd backend
dotnet restore
# set ConnectionStrings__DefaultConnection in appsettings.Development.json
dotnet ef database update --project src/PRReviewAssistant.API
dotnet run --project src/PRReviewAssistant.API
# API: http://localhost:8080 | Swagger: http://localhost:8080/swagger
```

**Frontend:**
```bash
cd frontend
npm install
# optional: set VITE_API_BASE_URL=http://localhost:8080 in .env
npm run dev
# UI: http://localhost:5173
```

**Tests:**
```bash
cd backend && dotnet test
cd frontend && npm test
```

### Environment Variables

See [.env.example](.env.example) for all required variables. Key variables:

| Variable | Description | Default |
|----------|-------------|---------|
| `POSTGRES_DB` | Database name | `pr_review_assistant` |
| `POSTGRES_USER` | DB user | `pguser` |
| `POSTGRES_PASSWORD` | DB password | `changeme` |
| `ConnectionStrings__DefaultConnection` | EF Core connection string | — |
| `USE_MOCK_AI` | Use mock AI (no API key required) | `true` |
| `OPENAI_API_KEY` | OpenAI key (when `USE_MOCK_AI=false`) | — |
| `VITE_API_BASE_URL` | Backend URL for local dev | empty (Docker proxy) |

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/v1/reviews` | Submit code diff for AI review |
| GET | `/api/v1/reviews?page=1&pageSize=20` | List reviews (paginated) |
| GET | `/api/v1/reviews/{id}` | Get review with all findings |
| DELETE | `/api/v1/reviews/{id}` | Delete a review (cascade) |
| GET | `/api/v1/settings` | Get application settings |
| PUT | `/api/v1/settings` | Update application settings |

All responses are wrapped: `{ data: T | null, error: ProblemDetails | null }`

## AI Generation Stats

| Metric | Value |
|--------|-------|
| Total prompt logs | 47 |
| Primary tool | GitHub Copilot (VS Code — Claude Sonnet 4.6) |
| Target AI-generated code | ≥ 90% |
| Manual edits | Marked with `// MANUAL EDIT: <reason>` |

## Documentation

- [Prompt Logs](prompts/) — One file per prompt: `{N}_{short-key}.md`
- [Insights](docs/INSIGHTS.md) — Observations on AI-assisted development
- [AI Workflow](docs/ai-workflow.md) — Development standard and conventions

## Changelog

- [2026-03-04] [INF-05] Generate `.gitignore`
- [2026-03-04] [BE-01] Scaffold .NET 9 solution
- [2026-03-04] [FE-01/02/03] Scaffold React + Vite + TypeScript frontend
- [2026-03-04] [BE-02/03] Domain entities (Review, Finding, AppSettings) and enums
- [2026-03-04] [BE-04] DTOs as C# records + ApiResponse<T>
- [2026-03-04] [BE-05] Repository and service interfaces
- [2026-03-04] [BE-06] AppDbContext + EF Fluent API configurations
- [2026-03-04] [BE-08] ReviewRepository + SettingsRepository implementations
- [2026-03-04] [BE-09] MockAiAnalysisService
- [2026-03-04] [BE-07] EF Core InitialCreate migration
- [2026-03-04] [BE-10/11] FluentValidation validators
- [2026-03-04] [BE-12] CreateReview Command + Handler
- [2026-03-04] [BE-13] GetReviews Query + Handler (paginated)
- [2026-03-04] [BE-14] GetReviewById Query + Handler
- [2026-03-04] [BE-15] DeleteReview Command + Handler
- [2026-03-04] [BE-16] Settings Queries + Commands + Handlers
- [2026-03-04] [BE-18] Minimal API endpoint maps
- [2026-03-04] [BE-19] GlobalExceptionHandler middleware
- [2026-03-04] [BE-17] Program.cs final DI + middleware + routing
- [2026-03-04] [FE-04] TypeScript models/interfaces
- [2026-03-04] [FE-05] Axios API service layer
- [2026-03-04] [FE-06] MainLayout
- [2026-03-04] [FE-07] AdminLayout
- [2026-03-04] [FE-08] React Router v6 configuration
- [2026-03-04] [FE-09] Common components (ErrorMessage, LoadingSpinner, SeverityBadge, ConfidenceBadge)
- [2026-03-04] [FE-10] Custom hooks (useReviews, useReviewDetail, useSettings, useCreateReview, useDeleteReview)
- [2026-03-04] [FE-11] Review feature components (FindingCard, FindingsList, ReviewSummary, SuggestedFixPanel)
- [2026-03-04] [FE-12] HomePage
- [2026-03-04] [FE-13] ReviewPage
- [2026-03-04] [FE-14] ReviewResultPage
- [2026-03-04] [FE-15] HistoryPage
- [2026-03-04] [FE-16] AdminSettingsPage
- [2026-03-04] [FE-17] Global styling: CSS design tokens + CSS module refactor
- [2026-03-04] [FE-18] Error boundaries per route
- [2026-03-04] [INF-03] docker-compose.yml (frontend + backend + db)
- [2026-03-04] [INF-01] Frontend Dockerfile (multi-stage node→nginx)
- [2026-03-04] [INF-02] Backend Dockerfile (multi-stage SDK→runtime)
- [2026-03-04] [INF-04] GitHub Actions CI workflow
- [2026-03-04] [BE-21] Backend unit tests (handlers, validators, MockAiAnalysisService)
- [2026-03-04] [FE-tests] Frontend component tests (FindingCard, ReviewSummary, ErrorBoundary, hooks)
- [2026-03-10] [DOC-01] README.md comprehensive documentation
- [2026-03-10] [DOC-03] INSIGHTS.md lessons learned
- [2026-03-10] [Final] Validation & cleanup — all services healthy, all endpoints verified, health check fix
