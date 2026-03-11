# PR Review Assistant — Project Overview & Architecture

---

## 1. Problem Statement

Developers spend significant time waiting for code reviews, and senior engineers spend hours reviewing pull requests that contain common, repetitive issues (naming, security, performance). This creates bottlenecks, delays delivery, and reduces time senior devs can spend on architecture and mentoring.

**PR Review Assistant** provides instant, AI-driven code review feedback before a human reviewer is involved — catching common issues early and freeing senior developer time.

---

## 2. Target Audience

| Audience | Value |
|----------|-------|
| Developers at Godel | Get instant feedback before submitting PRs |
| Tech leads / Senior devs | Fewer trivial review comments to write |
| Team managers | Faster PR cycle times, better code quality |
| Onboarding engineers | Learn team conventions faster via AI feedback |

---

## 3. Business Value

- **Time savings:** Reduces initial review round from hours to seconds
- **Quality:** Catches bugs, naming issues, security flags before human review
- **Consistency:** Standardized review criteria across all teams
- **Learning tool:** Junior developers get instant, actionable feedback
- **Company-applicable:** Directly useful for Godel's daily engineering workflows

---

## 4. Business Positioning (README-Ready)

### What It Is
An AI-powered code review assistant that analyzes code diffs and provides categorized, severity-rated feedback — bugs, naming issues, performance hints, security flags, and best practice suggestions.

### Why It Matters
Code reviews are a bottleneck. Waiting for a senior developer to review a PR can take hours or days. PR Review Assistant gives developers instant feedback on common issues, so human reviewers can focus on architecture and business logic.

### Key Benefits
- Paste a diff or provide a GitHub PR URL — get feedback in seconds
- Findings categorized by type: Bug, Naming, Performance, Security, Code Style, Best Practice
- Severity levels: Critical, Warning, Info
- Full review history with admin management
- Clean, modern UI with multiple layouts

### Differentiation
Unlike generic AI chat, PR Review Assistant is purpose-built for code review with structured, categorized output that mirrors how senior developers actually give feedback.

### Example Use Case
A junior developer finishes a feature branch. Before creating a PR, they paste their diff into PR Review Assistant. The tool flags a potential null reference (Critical), suggests better variable names (Info), and identifies a missing input validation (Warning). The developer fixes these issues before the PR even reaches a reviewer.

---

## 5. Scope

### In-Scope (MVP)

- Paste code diff as plain text for AI analysis
- Optional: Enter GitHub PR URL to fetch diff
- AI-powered analysis returning categorized findings
- Finding categories: Bug, Naming, Performance, Security, Code Style, Best Practice
- Severity levels: Critical, Warning, Info
- Review results page with filters (by category, by severity)
- Review history (list, detail view, delete)
- Two layouts: MainLayout + AdminLayout
- At least 5 pages with routing
- Persistent storage (PostgreSQL)
- Mock AI service (works without external API key)

### Out-of-Scope

- User authentication / authorization
- Real-time collaboration
- GitHub webhook integration
- Multi-language AI analysis configuration
- Deployment to cloud
- Mobile-specific UI optimization

### Time Constraints

- 2 days, max 2 hours per day (~4 hours total)
- 90%+ code AI-generated
- Focus on working solution over polish

---

## 6. High-Level Architecture

```
┌─────────────────────────────────────────────────────────┐
│                React (Vite + TypeScript)                 │
│                                                         │
│  ┌─────────────┐  ┌──────────────┐  ┌───────────────┐  │
│  │ MainLayout   │  │ ReviewPage   │  │ AdminLayout   │  │
│  │  - Home      │  │  - Input     │  │  - History    │  │
│  │  - About     │  │  - Result    │  │  - Settings   │  │
│  └─────────────┘  └──────────────┘  └───────────────┘  │
│                                                         │
│               React Router v6 (nested)                  │
└────────────────────────┬────────────────────────────────┘
                         │ HTTP (REST, JSON)
                         │ /api/v1/...
┌────────────────────────┴────────────────────────────────┐
│              .NET 9 Minimal API                          │
│                                                         │
│  ┌──────────┐  ┌─────────────┐  ┌───────────────────┐  │
│  │ Endpoints │  │  MediatR    │  │ AI Analysis Layer │  │
│  │ (thin)    │  │  Handlers   │  │ (Mock / OpenAI)   │  │
│  └──────────┘  └─────────────┘  └───────────────────┘  │
│                                                         │
│  ┌─────────────────┐  ┌──────────────────────────────┐  │
│  │ FluentValidation│  │ EF Core + Repository Pattern │  │
│  └─────────────────┘  └──────────────────────────────┘  │
│                              │                          │
└──────────────────────────────┼──────────────────────────┘
                               │
                    ┌──────────┴──────────┐
                    │   PostgreSQL 16     │
                    │   (Dockerized)      │
                    └─────────────────────┘
```

---

## 7. Layered Architecture Breakdown

### Presentation Layer (Frontend)
- React (Vite + TypeScript, strict mode)
- React Router v6 with nested routes and layout outlets
- Functional components only, hooks for state
- Axios HTTP client with typed API service
- Error boundaries on every route
- CSS Modules or Tailwind for styling

### Presentation Layer (API Endpoints)
- .NET 9 Minimal API
- Thin endpoint maps → `Send()` to MediatR
- No business logic in endpoints
- Response envelope: `{ data: T | null, error: ProblemDetails | null }`
- API versioning: `/api/v1/`

### Application Layer
- MediatR Commands and Queries with Handlers
- FluentValidation for all input validation
- DTOs as `record` types
- `CancellationToken` on all async methods

### Domain Layer
- Entity models (Review, Finding, AppSettings)
- Enums (FindingCategory, FindingSeverity)
- No framework dependencies

### Infrastructure Layer
- EF Core with PostgreSQL
- Repository pattern (no raw EF queries outside repositories)
- AI analysis service (IAiAnalysisService)
  - MockAiAnalysisService (default, no external dependency)
  - Optional: OpenAiAnalysisService
- Optional: GitHub API service for fetching PR diffs

---

## 8. Data Flow

```
User pastes diff → [ReviewPage] → POST /api/v1/reviews
    → FluentValidation (validates input)
    → CreateReviewCommand → Handler
        → IAiAnalysisService.AnalyzeAsync(codeDiff, language)
        → Returns List<Finding>
        → Repository.SaveAsync(review + findings)
        → Returns ReviewDetailResponse
    → Frontend displays categorized findings
```

---

## 9. Technology Stack

### Frontend

| Technology | Version | Purpose | Justification |
|-----------|---------|---------|---------------|
| React | 18+ | UI framework | Copilot-friendly, component-based |
| TypeScript | 5+ | Type safety | Strict mode, no `any` |
| Vite | 5+ | Build tool | Fast dev server, modern defaults |
| React Router | v6 | Routing + layouts | Nested routes with `<Outlet />` |
| Axios | latest | HTTP client | Typed, interceptor-friendly |
| Jest + RTL | latest | Testing | Standard React testing |
| ESLint + Prettier | latest | Code quality | Consistent formatting |

### Backend

| Technology | Version | Purpose | Justification |
|-----------|---------|---------|---------------|
| .NET | 9 | Runtime | Latest LTS, Minimal API |
| ASP.NET Core | 9 | Web framework | Minimal API pattern |
| MediatR | latest | CQRS | Clean command/query separation |
| FluentValidation | latest | Input validation | Declarative, testable |
| EF Core | 9 | ORM | PostgreSQL provider |
| xUnit | latest | Testing | Standard .NET testing |
| Serilog | latest | Logging | Structured logging |
| Swagger / OpenAPI | latest | API docs | Auto-generated |

### Infrastructure

| Technology | Purpose |
|-----------|---------|
| PostgreSQL 16 | Primary database (Dockerized) |
| Docker + Compose | Containerized development |
| GitHub Actions | CI pipeline |

### AI Integration

| Approach | Description |
|----------|-------------|
| Mock service (default) | Returns realistic structured findings without external API |
| OpenAI API (optional) | Real AI analysis when API key is configured |
| GitHub API (optional) | Fetch PR diff by URL |

---

## 10. Repository Folder Structure

```
PRReviewAssistant/
├── frontend/                           # React + Vite + TypeScript
│   ├── src/
│   │   ├── components/
│   │   │   ├── common/                 # ErrorMessage, LoadingSpinner, SeverityBadge
│   │   │   ├── review/                 # FindingCard, FindingsList, ReviewSummary
│   │   │   └── admin/                  # ReviewTable, SettingsForm
│   │   ├── hooks/                      # useReviews, useReviewDetail, useSettings
│   │   ├── layouts/
│   │   │   ├── MainLayout.tsx
│   │   │   └── AdminLayout.tsx
│   │   ├── pages/
│   │   │   ├── HomePage.tsx
│   │   │   ├── ReviewPage.tsx
│   │   │   ├── ReviewResultPage.tsx
│   │   │   ├── HistoryPage.tsx
│   │   │   └── AdminSettingsPage.tsx
│   │   ├── services/
│   │   │   └── api.ts                  # Typed Axios wrapper
│   │   ├── models/                     # TypeScript interfaces & types
│   │   ├── router/
│   │   │   └── index.tsx               # React Router config
│   │   ├── App.tsx
│   │   └── main.tsx
│   ├── package.json
│   ├── tsconfig.json
│   ├── vite.config.ts
│   └── Dockerfile
│
├── backend/
│   ├── src/
│   │   └── PRReviewAssistant.API/
│   │       ├── Endpoints/              # Minimal API endpoint maps
│   │       ├── Features/
│   │       │   ├── Reviews/
│   │       │   │   ├── Commands/       # CreateReview, DeleteReview
│   │       │   │   └── Queries/        # GetReviews, GetReviewById
│   │       │   └── Settings/
│   │       │       ├── Commands/       # UpdateSettings
│   │       │       └── Queries/        # GetSettings
│   │       ├── Domain/
│   │       │   ├── Entities/           # Review, Finding, AppSettings
│   │       │   └── Enums/              # FindingCategory, FindingSeverity
│   │       ├── Infrastructure/
│   │       │   ├── Data/               # AppDbContext, Migrations
│   │       │   ├── Repositories/       # ReviewRepository, SettingsRepository
│   │       │   └── Services/           # MockAiAnalysisService, OpenAiAnalysisService
│   │       ├── Shared/
│   │       │   ├── DTOs/               # Request/Response records
│   │       │   └── Middleware/         # GlobalExceptionHandler
│   │       └── Program.cs
│   ├── tests/
│   │   └── PRReviewAssistant.Tests/
│   └── PRReviewAssistant.sln
│
├── infra/
│   └── nginx.conf
│
├── .github/
│   ├── copilot-instructions.md
│   ├── pull_request_template.md
│   ├── prompts/
│   │   ├── implement.prompt.md
│   │   ├── dockerize.prompt.md
│   │   ├── setup-ci.prompt.md
│   │   ├── generate-entity.prompt.md
│   │   ├── generate-component.prompt.md
│   │   └── generate-handler.prompt.md
│   └── workflows/
│       └── ci.yml
│
├── docs/
│   ├── INSIGHTS.md
│   └── ai-workflow.md
│
├── prompts/
│   ├── _prompt-log-template.md
│   └── {N}_{short-key}.md  (one file per prompt)
│
├── docker-compose.yml
├── .env.example
├── .gitignore
└── README.md
```

---

## 11. Layouts

### MainLayout
- **Header:** App logo/name + navigation links (Home, New Review)
- **Content:** React Router `<Outlet />`
- **Footer:** App version, "Powered by AI" note

### AdminLayout
- **Sidebar:** Navigation links (History, Settings)
- **Top bar:** "Admin" badge + back-to-main link
- **Content:** React Router `<Outlet />`

---

## 12. Pages Summary

| Page | Route | Layout | Description |
|------|-------|--------|-------------|
| Home | `/` | MainLayout | App overview, quick "Start Review" CTA |
| New Review | `/review` | MainLayout | Paste diff or enter PR URL, select language, submit |
| Review Result | `/review/:id` | MainLayout | Display AI findings grouped by category, filterable |
| History | `/admin/history` | AdminLayout | List of past reviews, click to view, delete |
| Admin Settings | `/admin/settings` | AdminLayout | Configure mock mode, view stats |

---

## 13. Success Criteria

- [ ] Application builds and runs (frontend + backend + database via Docker Compose)
- [ ] At least 90% of code is AI-generated
- [ ] 5 pages with routing implemented
- [ ] 2 distinct layouts (MainLayout + AdminLayout)
- [ ] Code review analysis works end-to-end (submit diff → get findings)
- [ ] Review history persisted and browsable
- [ ] README.md with project overview
- [ ] Prompt logs in `prompts/` folder following `{N}_{short-key}.md` naming convention
- [ ] INSIGHTS.md with observations
- [ ] Follows conventions from `.github/copilot-instructions.md`

---

## 14. Killer Feature: AI Confidence Score + "Fix Suggestion" Preview

**What:** Each finding includes an AI confidence percentage and an inline code suggestion showing the exact fix.

**Why it's powerful:**
- Transforms passive feedback into actionable fixes
- Visually impressive for demo — shows before/after inline
- Differentiates from generic "AI review" tools

**Implementation (~2-3 hrs):**
- Add `confidence: number` (0-100) and `suggestedFix: string | null` fields to Finding
- Mock service generates realistic suggestions with confidence scores
- Frontend: render a collapsible "Suggested Fix" panel with syntax highlighting per finding
- No architecture changes needed — just extends existing Finding model

**Minimal architecture impact:** Adds 2 fields to an existing entity, one UI component, zero new endpoints.
