# GodelAI — Folder Structure Reference

Quick-reference guide for navigating the GodelAI workspace.

---

## 1. Workspaces (`Workspaces/`)

VS Code workspace configuration files.

| Path | Description |
|------|-------------|
| `Workspaces/` | Contains `.code-workspace` files for opening multi-root VS Code workspaces |

---

## 2. Documents (`Documents/`)

Planning materials, requirements, templates, and reference docs created **before and during** project development. Not part of any deployable project — purely documentation.

| Path | Description |
|------|-------------|
| `Documents/ai-common-project-structure/` | Reusable template for AI-driven projects (GitHub/Copilot config, workflow docs, prompt templates) |
| `Documents/ai-common-project-structure/.github/` | Template Copilot instructions, auto-injected instruction files, and reusable `.prompt.md` files |
| `Documents/ai-common-project-structure/.github/instructions/` | Template instruction files: AI generation rules, prompt logging conventions |
| `Documents/ai-common-project-structure/.github/prompts/` | Template reusable prompt files (dockerize, generate entity/component/handler, implement, CI setup) |
| `Documents/ai-common-project-structure/docs/` | Template project docs (AI workflow reference) |
| `Documents/ai-common-project-structure/prompts/` | Prompt log template (`_prompt-log-template.md`) |
| `Documents/preparetion/` | Early-stage brainstorming: original task description, project ideas, Copilot skill drafts, plan prompts |
| `Documents/project-requarements/` | Formal project requirements: overview, SRS, backlog tasks, AI generation rules, documentation templates |
| `Documents/prompts/` | Prompt logs from the Documents-level workspace (numbered `{N}_{short-key}.md`) |
| `Documents/summaries/` | High-level summaries: initial structure generation, architecture overview, implementation plan |

---

## 3. PR Review Assistant (`Projects/ai-pr-review-assistant/`)

The main application — an AI-powered code review assistant (React + .NET 9 + PostgreSQL).

### Root files

| Path | Description |
|------|-------------|
| `docker-compose.yml` | Orchestrates frontend, backend, and PostgreSQL containers |
| `README.md` | Project overview, run instructions, changelog |
| `.env` / `.env.example` | Environment variables (secrets / template) |
| `.gitignore` | Git ignore rules |

### `.github/`

GitHub & Copilot configuration.

| Path | Description |
|------|-------------|
| `.github/copilot-instructions.md` | Master Copilot behavioral rules, conventions, domain context |
| `.github/plan-pr-review-assistant.md` | Task plan with acceptance-criteria checkboxes |
| `.github/pull_request_template.md` | PR template |
| `.github/instructions/` | Auto-injected instruction files (AI generation, workflow, requirements, prompt logging) |
| `.github/prompts/` | Reusable `.prompt.md` files for common generation tasks |
| `.github/workflows/` | GitHub Actions CI (`ci.yml`) |

### `backend/`

.NET 9 Minimal API (C#, MediatR, FluentValidation, EF Core).

| Path | Description |
|------|-------------|
| `backend/PRReviewAssistant.sln` | Solution file |
| `backend/Dockerfile` | Multi-stage backend Docker build |
| `backend/src/PRReviewAssistant.API/` | Main API project |
| `backend/src/.../Domain/Entities/` | Domain entities: `Review`, `Finding`, `AppSettings` |
| `backend/src/.../Domain/Enums/` | Enums: `FindingCategory`, `FindingSeverity` |
| `backend/src/.../Domain/Interfaces/` | Service & repository interfaces (`IAiAnalysisService`, `IReviewRepository`, `ISettingsRepository`) |
| `backend/src/.../Endpoints/` | Minimal API endpoint maps (`ReviewEndpoints`, `SettingsEndpoints`) |
| `backend/src/.../Features/Reviews/Commands/` | MediatR commands: create review, delete review |
| `backend/src/.../Features/Reviews/Queries/` | MediatR queries: list reviews, get review by ID |
| `backend/src/.../Features/Reviews/Validators/` | FluentValidation validators for review requests |
| `backend/src/.../Features/Settings/Commands/` | MediatR commands: update settings |
| `backend/src/.../Features/Settings/Queries/` | MediatR queries: get settings |
| `backend/src/.../Features/Settings/Validators/` | FluentValidation validators for settings requests |
| `backend/src/.../Infrastructure/Data/` | `AppDbContext`, EF entity configurations, migrations |
| `backend/src/.../Infrastructure/Repositories/` | Repository implementations (`ReviewRepository`, `SettingsRepository`) |
| `backend/src/.../Infrastructure/Services/` | AI analysis service implementations (`MockAiAnalysisService`) |
| `backend/src/.../Shared/DTOs/` | Request/response records, `ApiResponse<T>`, `PaginatedList<T>` |
| `backend/src/.../Shared/Middleware/` | `GlobalExceptionHandler` middleware |
| `backend/src/.../Program.cs` | App entry point (DI, CORS, Swagger, middleware, PostgreSQL) |
| `backend/tests/PRReviewAssistant.Tests/` | xUnit tests (features, infrastructure/services) |

### `frontend/`

React 18 + Vite + TypeScript (strict mode).

| Path | Description |
|------|-------------|
| `frontend/Dockerfile` | Multi-stage frontend Docker build (Node → Nginx) |
| `frontend/package.json` | NPM dependencies and scripts |
| `frontend/vite.config.ts` | Vite build configuration |
| `frontend/eslint.config.js` | ESLint rules |
| `frontend/jest.config.cjs` | Jest test configuration |
| `frontend/nginx.conf` | Nginx config for serving the SPA in production |
| `frontend/src/main.tsx` | App entry point |
| `frontend/src/App.tsx` | Root component with router |
| `frontend/src/models/` | TypeScript interfaces and types (domain models, DTOs) |
| `frontend/src/services/` | Typed Axios API client with error normalization (`api.ts`) |
| `frontend/src/router/` | React Router v6 config (nested routes + layouts) |
| `frontend/src/layouts/` | Page layouts: `MainLayout`, `AdminLayout` (with CSS modules) |
| `frontend/src/pages/` | Page components: Home, Review, ReviewResult, History, AdminSettings |
| `frontend/src/components/common/` | Shared UI: `ErrorBoundary`, `ErrorMessage`, `LoadingSpinner`, `SeverityBadge`, `ConfidenceBadge` |
| `frontend/src/components/review/` | Review feature UI: `FindingCard`, `FindingsList`, `ReviewSummary`, `SuggestedFixPanel` |
| `frontend/src/components/admin/` | Admin-specific components (placeholder) |
| `frontend/src/hooks/` | Custom hooks: `useCreateReview`, `useReviews`, `useReviewDetail`, `useDeleteReview`, `useSettings` |
| `frontend/src/styles/` | Global CSS (`global.css`) |
| `frontend/src/__mocks__/` | Jest mocks for testing |

### `docs/`

Project documentation.

| Path | Description |
|------|-------------|
| `docs/ai-workflow.md` | AI-assisted development workflow reference |
| `docs/INSIGHTS.md` | Observations and lessons learned during development |

### `infra/`

Infrastructure configuration.

| Path | Description |
|------|-------------|
| `infra/nginx.conf` | Nginx reverse-proxy config for Docker Compose |

### `project-requarements/`

Project requirements (copy kept alongside the codebase).

| Path | Description |
|------|-------------|
| `project-requarements/1-project-overview.md` | High-level project overview |
| `project-requarements/2-functional-requirements-srs.md` | Functional requirements (SRS) |
| `project-requarements/3-backlog-tasks.md` | Backlog with task IDs and priorities |
| `project-requarements/4-ai-generation-rules.md` | Rules for AI code generation |
| `project-requarements/5-documentation-templates.md` | Templates for project documentation |

### `prompts/`

Prompt logs — one file per AI interaction that produced code.

| Path | Description |
|------|-------------|
| `prompts/_prompt-log-template.md` | Template for logging prompts |
| `prompts/{N}_{short-key}.md` | Individual prompt logs (47 entries covering scaffold → final validation) |
| `prompts/all-prompts-*.txt/.md` | Aggregated prompt exports |
