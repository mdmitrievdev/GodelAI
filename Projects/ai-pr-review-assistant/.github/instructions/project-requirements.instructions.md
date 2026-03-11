---
applyTo: "**"
---

# Project Requirements — PR Review Assistant

> Source: `project-requarements/` folder (5 files). Full details in those files; key content surfaced here for Copilot.

---

## Functional Requirements

### 3.1 Code Review Input

| ID | Requirement | Priority |
|----|------------|----------|
| FR-RI-01 | User shall paste a code diff as plain text into a multiline input | P0 |
| FR-RI-02 | User shall optionally enter a GitHub PR URL | P2 |
| FR-RI-03 | System shall validate that code diff is not empty before submission | P0 |
| FR-RI-04 | User shall select the programming language from a dropdown | P0 |
| FR-RI-05 | System shall display a loading indicator during AI analysis | P0 |
| FR-RI-06 | Supported languages: C#, TypeScript, JavaScript, Python, Java, Go, Other | P0 |

### 3.2 AI Code Analysis

| ID | Requirement | Priority |
|----|------------|----------|
| FR-AI-01 | System shall send code diff + language to IAiAnalysisService | P0 |
| FR-AI-02 | Service shall return a list of findings | P0 |
| FR-AI-03 | Each finding shall include: category, severity, title, description, lineReference (nullable), suggestion, confidence (0–100), suggestedFix (nullable) | P0 |
| FR-AI-04 | Finding categories: Bug, Naming, Performance, Security, CodeStyle, BestPractice | P0 |
| FR-AI-05 | Finding severities: Critical, Warning, Info | P0 |
| FR-AI-06 | System shall handle AI service errors and return ProblemDetails | P0 |
| FR-AI-07 | Mock AI service shall return realistic, varied findings based on input length and language | P0 |
| FR-AI-08 | If GitHub PR URL is provided, system shall fetch diff via GitHub API before analysis | P2 |

### 3.3 Review Results Display

| ID | Requirement | Priority |
|----|------------|----------|
| FR-RD-01 | System shall display findings grouped by category | P0 |
| FR-RD-02 | Each finding shall be color-coded by severity (red=Critical, orange=Warning, blue=Info) | P0 |
| FR-RD-03 | User shall filter findings by severity | P0 |
| FR-RD-04 | User shall filter findings by category | P0 |
| FR-RD-05 | System shall display summary: total findings, count per severity, average confidence | P0 |
| FR-RD-06 | System shall display the original diff in a code block alongside findings | P1 |
| FR-RD-07 | Each finding with a suggestedFix shall show a collapsible "Suggested Fix" panel | P1 |
| FR-RD-08 | Confidence score shall be displayed as a percentage badge on each finding | P1 |

### 3.4 Review History

| ID | Requirement | Priority |
|----|------------|----------|
| FR-RH-01 | System shall persist each review: id, codeDiff, language, prUrl, createdAt, findings | P0 |
| FR-RH-02 | User shall view a paginated list of past reviews | P0 |
| FR-RH-03 | Each history item shows: date, language, snippet preview, finding counts by severity | P0 |
| FR-RH-04 | User shall click a review to navigate to its full result page | P0 |
| FR-RH-05 | Reviews sorted by date (newest first) by default | P0 |

### 3.5 Admin Features

| ID | Requirement | Priority |
|----|------------|----------|
| FR-AD-01 | Admin view shall list all reviews with management options | P0 |
| FR-AD-02 | Admin shall delete individual reviews | P0 |
| FR-AD-03 | Admin settings page shall allow toggling mock AI mode on/off | P1 |
| FR-AD-04 | Admin settings page shall display application stats (total reviews, total findings) | P1 |

### 3.6 Navigation & Routing

| ID | Requirement | Priority |
|----|------------|----------|
| FR-NR-01 | Application shall use client-side routing (React Router v6) | P0 |
| FR-NR-02 | Route `/` → HomePage (MainLayout) | P0 |
| FR-NR-03 | Route `/review` → ReviewPage (MainLayout) | P0 |
| FR-NR-04 | Route `/review/:id` → ReviewResultPage (MainLayout) | P0 |
| FR-NR-05 | Route `/admin/history` → HistoryPage (AdminLayout) | P0 |
| FR-NR-06 | Route `/admin/settings` → AdminSettingsPage (AdminLayout) | P1 |
| FR-NR-07 | Unknown routes → redirect to `/` | P0 |
| FR-NR-08 | Active navigation link shall be visually highlighted | P1 |

---

## Non-Functional Requirements

### 4.1 Performance
| ID | Requirement |
|----|------------|
| NFR-P-01 | Frontend initial load: under 3 seconds |
| NFR-P-02 | API responses (non-AI): under 500ms |
| NFR-P-03 | Mock AI analysis: under 2 seconds |
| NFR-P-04 | History page pagination: 20 items per page |

### 4.2 Usability
| ID | Requirement |
|----|------------|
| NFR-U-01 | Desktop-first responsive design |
| NFR-U-02 | Navigation with visible active state |
| NFR-U-03 | Clear, actionable error messages (no raw exceptions) |
| NFR-U-04 | Loading states for all async operations |

### 4.3 Reliability
| ID | Requirement |
|----|------------|
| NFR-R-01 | Application shall handle API failures with user-friendly error display |
| NFR-R-02 | Application shall not crash on invalid or malicious input |
| NFR-R-03 | Error boundaries on every top-level route |

### 4.4 Maintainability
| ID | Requirement |
|----|------------|
| NFR-M-01 | Layered architecture with clear separation of concerns |
| NFR-M-02 | MediatR for all business logic (no logic in endpoints) |
| NFR-M-03 | Repository pattern for data access |
| NFR-M-04 | Dependency Injection throughout |
| NFR-M-06 | Files under 150 lines where possible |

### 4.5 Security
| ID | Requirement |
|----|------------|
| NFR-S-01 | No secrets in source code |
| NFR-S-02 | ProblemDetails responses never expose stack traces |
| NFR-S-03 | Input validation on all API endpoints |
| NFR-S-04 | CORS configured for frontend origin only |

### 4.6 AI Generation
| ID | Requirement |
|----|------------|
| NFR-AI-01 | At least 90% of code AI-generated |
| NFR-AI-02 | All prompts documented in `prompts/` folder using `{N}_{short-key}.md` per-file naming convention |
| NFR-AI-03 | Manual edits marked with `// MANUAL EDIT: <reason>` |

---

## Backlog Task IDs

Use these IDs in commit messages, prompt logs, and plan references.

### Backend
| ID | Task | Priority | Phase |
|----|------|----------|-------|
| BE-01 | .NET 9 solution + Minimal API scaffold | P0 | 1 |
| BE-02 | Domain entities (Review, Finding) + enums | P0 | 2 |
| BE-03 | AppSettings entity | P1 | 2 |
| BE-04 | DTOs as C# records (requests + responses) | P0 | 2 |
| BE-05 | AppDbContext + PostgreSQL config + entity configs | P0 | 2 |
| BE-06 | IReviewRepository + ReviewRepository | P0 | 2 |
| BE-07 | ISettingsRepository + SettingsRepository | P1 | 2 |
| BE-08 | IAiAnalysisService interface | P0 | 2 |
| BE-09 | MockAiAnalysisService | P0 | 2 |
| BE-10 | CreateReview Command + Handler | P0 | 2 |
| BE-11 | GetReviews Query + Handler (paginated) | P0 | 2 |
| BE-12 | GetReviewById Query + Handler | P0 | 2 |
| BE-13 | DeleteReview Command + Handler | P0 | 2 |
| BE-14 | GetSettings + UpdateSettings | P1 | 2 |
| BE-15 | FluentValidation validators | P0 | 2 |
| BE-16 | Minimal API endpoint maps | P0 | 2 |
| BE-17 | Program.cs (DI, CORS, Swagger, PostgreSQL, MediatR, middleware) | P0 | 1 |
| BE-18 | GlobalExceptionHandler middleware | P0 | 2 |
| BE-19 | EF Core migrations | P0 | 2 |
| BE-20 | ApiResponse\<T\> wrapper | P0 | 2 |
| BE-21 | OpenAiAnalysisService (optional) | P2 | 4 |
| BE-22 | GitHubApiService for PR diff fetch (optional) | P2 | 4 |

### Frontend
| ID | Task | Priority | Phase |
|----|------|----------|-------|
| FE-01 | React + Vite + TypeScript scaffold | P0 | 1 |
| FE-02 | Dependencies (react-router-dom, axios) | P0 | 1 |
| FE-03 | ESLint + Prettier + strict TypeScript config | P0 | 1 |
| FE-04 | TypeScript models/interfaces | P0 | 3 |
| FE-05 | API service layer (typed Axios + error normalization) | P0 | 3 |
| FE-06 | MainLayout | P0 | 3 |
| FE-07 | AdminLayout | P0 | 3 |
| FE-08 | Router configuration (nested routes) | P0 | 3 |
| FE-09 | Common components: ErrorMessage, LoadingSpinner, SeverityBadge, ConfidenceBadge | P0 | 3 |
| FE-10 | HomePage | P0 | 3 |
| FE-11 | ReviewPage (diff input, language select, submit) | P0 | 3 |
| FE-12 | Review components: FindingCard, FindingsList, ReviewSummary, SuggestedFixPanel | P0 | 3 |
| FE-13 | ReviewResultPage (findings with filters) | P0 | 3 |
| FE-14 | HistoryPage (paginated list) | P0 | 3 |
| FE-15 | AdminSettingsPage | P1 | 3 |
| FE-16 | Custom hooks: useReviews, useReviewDetail, useSettings | P0 | 3 |
| FE-17 | Styling (CSS Modules or Tailwind) | P1 | 4 |
| FE-18 | Error boundaries on route level | P1 | 4 |

### Infrastructure & Docs
| ID | Task | Priority | Phase |
|----|------|----------|-------|
| INF-01 | docker-compose.yml (frontend + backend + PostgreSQL) | P0 | 5 |
| INF-02 | Frontend Dockerfile (multi-stage: node → nginx) | P1 | 5 |
| INF-03 | Backend Dockerfile (multi-stage: SDK → runtime) | P1 | 5 |
| INF-04 | .env.example | P0 | 5 |
| INF-05 | .gitignore | P0 | 1 |
| INF-06 | GitHub Actions CI workflow | P2 | 5 |
| DOC-01 | README.md | P0 | 5 |
| DOC-02 | prompts/ folder + _prompt-log-template.md | P0 | 1 |
| DOC-03 | Log prompts during development (ongoing) | P0 | All |
| DOC-04 | INSIGHTS.md | P0 | 5 |
| DOC-05 | .github/ folder setup + adaptation | P0 | 1 |

---

## Detailed Folder Structure

```
PRReviewAssistant/
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   │   ├── common/           # ErrorMessage, LoadingSpinner, SeverityBadge, ConfidenceBadge
│   │   │   ├── review/           # FindingCard, FindingsList, ReviewSummary, SuggestedFixPanel
│   │   │   └── admin/            # ReviewTable, SettingsForm
│   │   ├── hooks/                # useReviews, useReviewDetail, useSettings
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
│   │   │   └── api.ts            # Typed Axios wrapper with error normalization
│   │   ├── models/               # TypeScript interfaces & types
│   │   ├── router/
│   │   │   └── index.tsx         # React Router config
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
│   │       ├── Endpoints/        # Minimal API endpoint maps (ReviewEndpoints, SettingsEndpoints)
│   │       ├── Features/
│   │       │   ├── Reviews/
│   │       │   │   ├── Commands/ # CreateReviewCommand, DeleteReviewCommand
│   │       │   │   └── Queries/  # GetReviewsQuery, GetReviewByIdQuery
│   │       │   └── Settings/
│   │       │       ├── Commands/ # UpdateSettingsCommand
│   │       │       └── Queries/  # GetSettingsQuery
│   │       ├── Domain/
│   │       │   ├── Entities/     # Review, Finding, AppSettings
│   │       │   └── Enums/        # FindingCategory, FindingSeverity
│   │       ├── Infrastructure/
│   │       │   ├── Data/         # AppDbContext, Migrations
│   │       │   ├── Repositories/ # ReviewRepository, SettingsRepository
│   │       │   └── Services/     # MockAiAnalysisService, OpenAiAnalysisService
│   │       ├── Shared/
│   │       │   ├── DTOs/         # Request/Response records, ApiResponse<T>
│   │       │   └── Middleware/   # GlobalExceptionHandler
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
│   ├── instructions/             # Auto-injected into all Copilot chats
│   ├── prompts/                  # Reusable .prompt.md files
│   └── workflows/
│       └── ci.yml
│
├── docs/
│   ├── ai-workflow.md
│   └── INSIGHTS.md               # Created at project end
│
├── prompts/                      # Prompt logs: {N}_{short-key}.md
├── project-requarements/         # Full SRS, backlog, rules (5 files)
├── docker-compose.yml
├── .env.example
├── .gitignore
└── README.md
```

---

## Technology Stack (Exact Versions)

### Frontend
| Technology | Version | Purpose |
|-----------|---------|---------|
| React | 18+ | UI framework |
| TypeScript | 5+ (strict) | Type safety |
| Vite | 5+ | Build tool |
| React Router | v6 | Nested routes + layouts |
| Axios | latest | HTTP client |
| Jest + React Testing Library | latest | Testing |
| ESLint + Prettier | latest | Code quality |

### Backend
| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET | 9 | Runtime |
| ASP.NET Core Minimal API | 9 | Web framework |
| MediatR | latest | CQRS pattern |
| FluentValidation | latest | Input validation |
| EF Core | 9 | ORM |
| xUnit | latest | Testing |
| Serilog | latest | Structured logging |
| Swagger / OpenAPI | latest | API documentation |

### Infrastructure
| Technology | Purpose |
|-----------|---------|
| PostgreSQL 16 | Primary database (Dockerized) |
| Docker + Compose | Containerized development |
| GitHub Actions | CI pipeline |

---

## Success Criteria

- [ ] Application builds and runs via Docker Compose (frontend + backend + database)
- [ ] At least 90% of code is AI-generated
- [ ] 5 pages with routing implemented
- [ ] 2 distinct layouts (MainLayout + AdminLayout)
- [ ] Code review analysis works end-to-end (submit diff → get findings)
- [ ] Review history persisted and browsable
- [ ] README.md with project overview
- [ ] Prompt logs in `prompts/` folder following `{N}_{short-key}.md` naming
- [ ] INSIGHTS.md with observations
- [ ] Follows conventions from `.github/copilot-instructions.md`
