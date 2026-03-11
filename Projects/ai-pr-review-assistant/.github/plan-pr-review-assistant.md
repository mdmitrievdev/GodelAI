# Implementation Plan — PR Review Assistant

> **Generated:** 2026-03-04
> **Project:** AI-ED-FTG16 — PR Review Assistant
> **Stack:** React 18 + Vite + TypeScript · .NET 9 Minimal API · PostgreSQL 16 · Docker Compose
> **Total Effort:** ~4 hours (2 sessions × 2 hours)

---

## 1. Readiness Assessment — Issues & Fixes

All documentation in `.github/`, `docs/`, `project-requarements/`, and `prompts/` is complete and ready for implementation. The following minor issues were found:

| # | File | Issue | Exact Fix |
|---|------|-------|-----------|
| 1 | `docs/ai-workflow.md` L294 | Duplicate `## Section 8` heading — "Code Generation Order" (L220) and "Definition of Done" (L294) | Rename L294 to `## Section 11 — Definition of Done` |
| 2 | `project-requarements/1-project-overview.md` | Lists only 3 `.github/prompts/` files in the folder structure | Add the 3 missing files: `generate-entity.prompt.md`, `generate-component.prompt.md`, `generate-handler.prompt.md` to the tree |
| 3 | `project-requarements/1-project-overview.md` | Shows `docker-compose.yml` inside `infra/` folder | Move to repo root in the tree: `├── docker-compose.yml` and remove the `infra/` section or repurpose it for `nginx.conf` only |
| 4 | `docs/ai-workflow.md` | Section numbering skips from 10 to 8 (duplicate) | Renumber: Section 8 → Code Gen Order, Section 9 → Review Checklist, Section 10 → Manual vs AI, Section 11 → Definition of Done |
| 5 | `.github/prompts/*.prompt.md` | No `applyTo` frontmatter in prompt files | Low priority — prompts work without it. Optionally add `applyTo: "**"` |
| 6 | Repo root | `.gitignore` missing | Created by Prompt 6 (first implementation step) |
| 7 | Repo root | `.env.example` missing | Created by Prompt 42 (Docker phase) |
| 8 | `project-requarements/` | Folder name misspelled ("requarements") | **Keep as-is** for consistency with all existing references |

**Decision — Prompt logging:** Per-file in `prompts/` (active Copilot instructions win over SRS)
**Decision — docker-compose.yml:** Repo root (standard convention)
**Decision — CSS approach:** CSS Modules (lighter than Tailwind for MVP)

**Verdict:** Fix issues 1–4 before starting implementation (< 5 minutes). Issues 5–8 are handled during implementation or are non-blocking.

---

## 2. Implementation Milestones

### Day 1 (~2 hours)

| Milestone | Time | Prompts | Tasks | Exit Criteria |
|-----------|------|---------|-------|---------------|
| 1 — Foundation | 0:00–0:20 | 6–9 | INF-05, BE-01, FE-01/02/03, BE-20 | `dotnet build` + `npm run build` pass; `/health` responds |
| 2 — Backend Contracts | 0:20–0:45 | 10–13 | BE-02, BE-03, BE-04, BE-05 | All entities, DTOs, interfaces compile |
| 3 — Backend Infrastructure | 0:45–1:05 | 14–17 | BE-06, BE-07, BE-08, BE-09 | DB migration runs; mock AI returns findings |
| 4 — Backend Logic | 1:05–1:45 | 18–26 | BE-10–BE-19, BE-17 | All 6 API endpoints respond correctly |
| 5 — Frontend Foundation | 1:45–2:00 | 27–28 | FE-04, FE-05 | TypeScript models + API service compile |

### Day 2 (~2 hours)

| Milestone | Time | Prompts | Tasks | Exit Criteria |
|-----------|------|---------|-------|---------------|
| 6 — Layouts & Routing | 0:00–0:20 | 29–32 | FE-06, FE-07, FE-08, FE-09 | All routes render; layouts complete |
| 7 — Hooks & Components | 0:20–0:40 | 33–34 | FE-10, FE-11 | Live data flows end-to-end |
| 8 — Pages | 0:40–1:20 | 35–39 | FE-12–FE-16 | Full user journey: submit → view → history |
| 9 — Polish | 1:20–1:40 | 40–41 | FE-17, FE-18 | Consistent design; crashes handled |
| 10 — Infra & Docs | 1:40–2:00 | 42–50 | INF-01–04, BE-21, DOC-01, DOC-03 | Docker green; CI defined; README complete |

---

## 3. Improved Backlog — Full Task Descriptions with Acceptance Criteria

### Phase 1: Foundation (Prompts 6–9, ~20 min)

---

#### TASK INF-05: Generate `.gitignore`

- **Priority:** P0 | **Estimate:** 2 min | **Depends on:** — | **Prompt:** 6
- **Intent:** Create a comprehensive `.gitignore` covering .NET, Node.js, IDE, OS, and Docker artifacts
- **Acceptance Criteria:**
  - [x] Covers `bin/`, `obj/`, `node_modules/`, `dist/`, `.env`, `*.user`, `.vs/`, `.idea/`
  - [x] Includes Docker ignore patterns (`docker-compose.override.yml`)
  - [x] File at repo root
- **Output:** `.gitignore`

---

#### TASK BE-01: Scaffold .NET 9 solution

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** INF-05 | **Prompt:** 7
- **Intent:** Create the backend solution structure with API project, test project, and NuGet packages
- **Acceptance Criteria:**
  - [x] `backend/PRReviewAssistant.sln` exists
  - [x] `backend/src/PRReviewAssistant.API/PRReviewAssistant.API.csproj` targets `net9.0`
  - [x] `backend/tests/PRReviewAssistant.Tests/PRReviewAssistant.Tests.csproj` targets `net9.0`
  - [x] NuGet packages installed: MediatR, FluentValidation.AspNetCore, Npgsql.EntityFrameworkCore.PostgreSQL, Swashbuckle.AspNetCore, NUlid, Microsoft.EntityFrameworkCore.Design
  - [x] Test packages: Moq, FluentAssertions, Microsoft.EntityFrameworkCore.InMemory
  - [x] `dotnet build` passes with 0 errors
- **Output:** Solution structure, `.csproj` files

---

#### TASK FE-01/02/03: Scaffold React + Vite + TypeScript

- **Priority:** P0 | **Estimate:** 5 min | **Depends on:** INF-05 | **Prompt:** 8
- **Intent:** Create frontend project with strict TypeScript, ESLint, Prettier
- **Acceptance Criteria:**
  - [x] `frontend/` created with Vite React TS template
  - [x] `tsconfig.json` has `strict: true`, `noUncheckedIndexedAccess: true`
  - [x] Dependencies: `react-router-dom`, `axios`
  - [x] Dev dependencies: testing-library packages, jest, eslint, prettier
  - [x] `npm run build` passes
- **Output:** `frontend/` scaffold

---

#### TASK BE-20: Program.cs skeleton

- **Priority:** P0 | **Estimate:** 5 min | **Depends on:** BE-01 | **Prompt:** 9
- **Intent:** Create minimal `Program.cs` with CORS, Swagger, health endpoint
- **Acceptance Criteria:**
  - [x] App starts and `/health` returns `200 OK`
  - [x] Swagger UI at `/swagger`
  - [x] CORS configured for `http://localhost:5173`
  - [x] No business logic — just infrastructure
- **Output:** `backend/src/PRReviewAssistant.API/Program.cs`

---

### Phase 2: Backend Contracts (Prompts 10–13, ~25 min)

---

#### TASK BE-02/03: Domain entities + enums

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-01 | **Prompt:** 10
- **Intent:** Generate `Review`, `Finding` entities and `FindingCategory`, `FindingSeverity` enums
- **Acceptance Criteria:**
  - [x] `Review` has: `Id` (string/ULID), `CodeDiff`, `Language`, `PrUrl?`, `CreatedAt` (UTC), `Findings` (nav prop)
  - [x] `Finding` has all fields per SRS including `Confidence` (0–100) and `SuggestedFix?`
  - [x] `FindingCategory` enum: Bug, Naming, Performance, Security, CodeStyle, BestPractice
  - [x] `FindingSeverity` enum: Critical, Warning, Info
  - [x] No EF attributes — Fluent API only
  - [x] `dotnet build` passes
- **Output:** `Domain/Entities/Review.cs`, `Domain/Entities/Finding.cs`, `Domain/Enums/FindingCategory.cs`, `Domain/Enums/FindingSeverity.cs`

---

#### TASK BE-02 (AppSettings): AppSettings entity

- **Priority:** P0 | **Estimate:** 5 min | **Depends on:** BE-01 | **Prompt:** 11
- **Intent:** Singleton `AppSettings` entity for mock AI toggle
- **Acceptance Criteria:**
  - [x] Fields: `Id` (string), `UseMockAi` (bool, default true), `AiModel` (string, default "mock")
  - [x] No framework dependencies
- **Output:** `Domain/Entities/AppSettings.cs`

---

#### TASK BE-04: DTOs + ApiResponse\<T\>

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** BE-02, BE-03 | **Prompt:** 12
- **Intent:** All request/response records plus the API wrapper
- **Acceptance Criteria:**
  - [x] `CreateReviewRequest`, `UpdateSettingsRequest` (request records)
  - [x] `ReviewDetailResponse`, `ReviewListItem`, `FindingDto`, `ReviewSummaryDto`, `AppSettingsResponse` (response records)
  - [x] `PaginatedList<T>` record with `Items`, `TotalCount`, `Page`, `PageSize`
  - [x] `ApiResponse<T>` with `Data` and `Error` (ProblemDetails?) properties
  - [x] All use `record` syntax, `IReadOnlyList<T>` for collections
  - [x] String IDs, nullable where specified
- **Output:** `Shared/DTOs/` directory with all DTO files

---

#### TASK BE-05: Interfaces

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-02, BE-04 | **Prompt:** 13
- **Intent:** Repository and service interfaces
- **Acceptance Criteria:**
  - [x] `IReviewRepository`: `CreateAsync`, `GetByIdAsync`, `GetAllAsync` (paginated), `DeleteAsync` — all with `CancellationToken`
  - [x] `ISettingsRepository`: `GetAsync`, `UpdateAsync` — all with `CancellationToken`
  - [x] `IAiAnalysisService`: `AnalyzeAsync(string codeDiff, string language, CancellationToken ct)` returning `Task<IReadOnlyList<Finding>>`
  - [x] XML doc comments on all methods
- **Output:** `Domain/Interfaces/IReviewRepository.cs`, `ISettingsRepository.cs`, `IAiAnalysisService.cs`

---

### Phase 3: Backend Infrastructure (Prompts 14–17, ~20 min)

---

#### TASK BE-06: AppDbContext + entity configurations

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** BE-02 | **Prompt:** 14
- **Intent:** EF Core context with Fluent API configurations for all entities
- **Acceptance Criteria:**
  - [x] `AppDbContext` inherits `DbContext`, has `DbSet<Review>`, `DbSet<Finding>`, `DbSet<AppSettings>`
  - [x] Fluent API: primary keys, max lengths, required fields, FK relationships, cascade delete on `Review→Findings`
  - [x] `AppSettings` seeded with default values
  - [x] Connection string from configuration
- **Output:** `Infrastructure/Data/AppDbContext.cs`, `Infrastructure/Data/Configurations/ReviewConfiguration.cs`, `FindingConfiguration.cs`, `AppSettingsConfiguration.cs`

---

#### TASK BE-08: Repository implementations

- **Priority:** P0 | **Estimate:** 20 min | **Depends on:** BE-05, BE-06 | **Prompt:** 15
- **Intent:** Implement `IReviewRepository` and `ISettingsRepository`
- **Acceptance Criteria:**
  - [x] `ReviewRepository` injects `AppDbContext`
  - [x] `GetAllAsync` supports pagination with `Skip`/`Take`, returns `(IReadOnlyList<ReviewListItem>, int totalCount)`
  - [x] `GetByIdAsync` includes `Finding` navigation property
  - [x] `DeleteAsync` cascades to findings
  - [x] `AsNoTracking()` for read queries
  - [x] `SettingsRepository` ensures singleton — creates default if missing
- **Output:** `Infrastructure/Repositories/ReviewRepository.cs`, `SettingsRepository.cs`

---

#### TASK BE-09: MockAiAnalysisService

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** BE-05 | **Prompt:** 16
- **Intent:** Realistic mock that returns varied findings without external API
- **Acceptance Criteria:**
  - [x] Implements `IAiAnalysisService`
  - [x] Returns 3–8 findings per analysis
  - [x] Findings vary by language and diff length
  - [x] Each finding has realistic `Title`, `Description`, `Suggestion`, `LineReference`, `Confidence` (40–98), `SuggestedFix`
  - [x] All 6 categories and 3 severity levels represented across calls
  - [x] Simulates 200–800ms delay (configurable)
  - [x] Generates ULID IDs
- **Output:** `Infrastructure/Services/MockAiAnalysisService.cs`

---

#### TASK BE-07: EF Core migration

- **Priority:** P0 | **Estimate:** 5 min | **Depends on:** BE-06 | **Prompt:** 17
- **Intent:** Create `InitialCreate` migration
- **Acceptance Criteria:**
  - [x] Migration applies cleanly to a fresh PostgreSQL database
  - [x] `AppSettings` seed data included
  - [x] Tables: `Reviews`, `Findings`, `AppSettings`
- **Output:** `Infrastructure/Data/Migrations/` directory

---

### Phase 4: Backend Logic (Prompts 18–26, ~40 min)

---

#### TASK BE-10/11: FluentValidation validators

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-04 | **Prompt:** 18
- **Intent:** Input validators for `CreateReviewRequest` and `UpdateSettingsRequest`
- **Acceptance Criteria:**
  - [x] `CreateReviewRequestValidator`: `CodeDiff` not empty, max 50000 chars; `Language` not empty, must be in supported list; `PrUrl` optional but valid URL format if provided
  - [x] `UpdateSettingsRequestValidator`: `AiModel` not empty, max 100 chars
  - [x] `WithMessage()` on every rule
  - [x] No business logic
- **Output:** `Features/Reviews/Validators/CreateReviewRequestValidator.cs`, `Features/Settings/Validators/UpdateSettingsRequestValidator.cs`

---

#### TASK BE-12: CreateReview Command + Handler

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** BE-05, BE-08, BE-09 | **Prompt:** 19
- **Intent:** Submit diff for AI analysis, persist review + findings
- **Acceptance Criteria:**
  - [x] Command implements `IRequest<ReviewDetailResponse>`
  - [x] Handler: validates → calls `IAiAnalysisService.AnalyzeAsync` → creates `Review` with ULID → saves via repository → maps to `ReviewDetailResponse`
  - [x] POST `/api/v1/reviews` with valid diff returns `201` with response containing ≥3 findings
  - [x] `CancellationToken` passed through all calls
- **Output:** `Features/Reviews/Commands/CreateReviewCommand.cs`, `CreateReviewCommandHandler.cs`

---

#### TASK BE-13: GetReviews Query + Handler

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-05, BE-08 | **Prompt:** 20
- **Intent:** Paginated list of reviews
- **Acceptance Criteria:**
  - [x] Query: `int Page = 1, int PageSize = 20`
  - [x] Returns `PaginatedList<ReviewListItem>` with `TotalCount`
  - [x] Reviews sorted by `CreatedAt` descending
  - [x] `CodeSnippet` truncated to 80 chars
- **Output:** `Features/Reviews/Queries/GetReviewsQuery.cs`, `GetReviewsQueryHandler.cs`

---

#### TASK BE-14: GetReviewById Query + Handler

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-05, BE-08 | **Prompt:** 21
- **Intent:** Single review with all findings
- **Acceptance Criteria:**
  - [x] Returns `ReviewDetailResponse` with findings and summary
  - [x] Invalid ID → returns null (endpoint converts to 404 ProblemDetails)
  - [x] Summary: `TotalFindings`, `CriticalCount`, `WarningCount`, `InfoCount`, `AverageConfidence`
- **Output:** `Features/Reviews/Queries/GetReviewByIdQuery.cs`, `GetReviewByIdQueryHandler.cs`

---

#### TASK BE-15: DeleteReview Command + Handler

- **Priority:** P0 | **Estimate:** 5 min | **Depends on:** BE-05, BE-08 | **Prompt:** 22
- **Intent:** Delete review and cascade-delete findings
- **Acceptance Criteria:**
  - [x] Invalid ID → returns `false`
  - [x] Valid ID → deletes review + findings, returns `true`
  - [x] `CancellationToken` passed
- **Output:** `Features/Reviews/Commands/DeleteReviewCommand.cs`, `DeleteReviewCommandHandler.cs`

---

#### TASK BE-16: Settings Handlers

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-05, BE-08 | **Prompt:** 23
- **Intent:** Get and update application settings
- **Acceptance Criteria:**
  - [x] `GetSettingsQuery` → returns `AppSettingsResponse`
  - [x] `UpdateSettingsCommand` → updates settings, returns `AppSettingsResponse`
  - [x] Settings singleton ensured (creates default if missing)
- **Output:** `Features/Settings/Queries/GetSettingsQuery.cs`, `GetSettingsQueryHandler.cs`, `Features/Settings/Commands/UpdateSettingsCommand.cs`, `UpdateSettingsCommandHandler.cs`

---

#### TASK BE-18: Minimal API endpoint maps

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-12–16 | **Prompt:** 24
- **Intent:** Map all 6 API routes to MediatR `Send()` calls
- **Acceptance Criteria:**
  - [x] `ReviewEndpoints`: POST, GET (list), GET (by id), DELETE
  - [x] `SettingsEndpoints`: GET, PUT
  - [x] All routes prefixed `/api/v1/`
  - [x] Responses wrapped in `ApiResponse<T>`
  - [x] 404 returned with ProblemDetails when entity not found
  - [x] No business logic in endpoints
- **Output:** `Endpoints/ReviewEndpoints.cs`, `Endpoints/SettingsEndpoints.cs`

---

#### TASK BE-19: GlobalExceptionHandler

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-17 | **Prompt:** 25
- **Intent:** Catch unhandled exceptions, return ProblemDetails
- **Acceptance Criteria:**
  - [x] `ValidationException` → 400 with field errors
  - [x] `KeyNotFoundException` → 404
  - [x] All other → 500 with generic message (no stack trace)
  - [x] Logs error with `ILogger<T>`
  - [x] Implements `IExceptionHandler` or middleware
- **Output:** `Shared/Middleware/GlobalExceptionHandler.cs`

---

#### TASK BE-17: Program.cs final

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** All BE tasks | **Prompt:** 26
- **Intent:** Complete DI registration, middleware pipeline, endpoint mapping
- **Acceptance Criteria:**
  - [x] Registers: MediatR, FluentValidation, EF Core, repositories, `IAiAnalysisService` → `MockAiAnalysisService`
  - [x] Middleware: exception handler, CORS, Swagger (dev only)
  - [x] Maps: `ReviewEndpoints`, `SettingsEndpoints`, `/health`
  - [x] `dotnet build` passes; `dotnet run` starts successfully; all endpoints respond
- **Output:** Updated `Program.cs`

---

### Phase 5: Frontend Foundation (Prompts 27–28, ~15 min)

---

#### TASK FE-04: TypeScript models

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE-04 | **Prompt:** 27
- **Intent:** Mirror all backend DTOs as TypeScript interfaces
- **Acceptance Criteria:**
  - [x] All interfaces match copilot-instructions.md TypeScript Interfaces section exactly
  - [x] Types: `FindingCategory`, `FindingSeverity` as string unions
  - [x] `ApiResponse<T>` interface: `{ data: T | null; error: ProblemDetails | null }`
  - [x] `ProblemDetails` interface
  - [x] No `any` types; strict mode compatible
- **Output:** `frontend/src/models/index.ts` (or split per entity)

---

#### TASK FE-05: Axios API service

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** FE-04 | **Prompt:** 28
- **Intent:** Typed API client with error normalization
- **Acceptance Criteria:**
  - [x] Base URL configurable via `VITE_API_URL` env var
  - [x] Methods: `createReview`, `getReviews`, `getReviewById`, `deleteReview`, `getSettings`, `updateSettings`
  - [x] All return typed responses
  - [x] Error interceptor normalizes to `{ message: string; code?: string }`
  - [x] No `any` types
- **Output:** `frontend/src/services/api.ts`

---

### Phase 6: Frontend Layouts & Routing (Prompts 29–32, ~20 min)

---

#### TASK FE-06: MainLayout

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** FE-01 | **Prompt:** 29
- **Intent:** Header + nav (Home, New Review) + `<Outlet />` + footer
- **Acceptance Criteria:**
  - [x] Header with app name/logo and navigation links
  - [x] Active link visually highlighted
  - [x] Footer with version info
  - [x] Uses `<Outlet />` for child routes
  - [x] CSS Module for styling
- **Output:** `frontend/src/layouts/MainLayout.tsx`, `MainLayout.module.css`

---

#### TASK FE-07: AdminLayout

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** FE-01 | **Prompt:** 30
- **Intent:** Sidebar (History, Settings) + top bar (Admin badge + back link) + `<Outlet />`
- **Acceptance Criteria:**
  - [x] Sidebar with nav links
  - [x] Top bar with "Admin" badge and "Back to Main" link
  - [x] `<Outlet />` for child routes
  - [x] CSS Module for styling
- **Output:** `frontend/src/layouts/AdminLayout.tsx`, `AdminLayout.module.css`

---

#### TASK FE-08: Router configuration

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** FE-06, FE-07 | **Prompt:** 31
- **Intent:** Nested routes with layout wrappers
- **Acceptance Criteria:**
  - [x] `/` → `HomePage` (MainLayout)
  - [x] `/review` → `ReviewPage` (MainLayout)
  - [x] `/review/:id` → `ReviewResultPage` (MainLayout)
  - [x] `/admin/history` → `HistoryPage` (AdminLayout)
  - [x] `/admin/settings` → `AdminSettingsPage` (AdminLayout)
  - [x] Unknown routes → redirect to `/`
  - [x] Pages can be placeholder stubs initially
- **Output:** `frontend/src/router/index.tsx`, updated `App.tsx`

---

#### TASK FE-09: Common components

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** FE-04 | **Prompt:** 32
- **Intent:** Shared UI primitives
- **Acceptance Criteria:**
  - [x] `ErrorMessage`: accepts `message: string`, renders styled error alert
  - [x] `LoadingSpinner`: visual spinner with optional `message` prop
  - [x] `SeverityBadge`: renders colored badge (red/orange/blue) based on `FindingSeverity`
  - [x] `ConfidenceBadge`: renders percentage badge with color gradient
  - [x] Props interfaces defined above components
  - [x] Named exports
- **Output:** `frontend/src/components/common/ErrorMessage.tsx`, `LoadingSpinner.tsx`, `SeverityBadge.tsx`, `ConfidenceBadge.tsx` + CSS modules

---

### Phase 7: Hooks & Feature Components (Prompts 33–34, ~20 min)

---

#### TASK FE-10: Custom hooks

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** FE-05 | **Prompt:** 33
- **Intent:** Data-fetching hooks for all API endpoints
- **Acceptance Criteria:**
  - [x] `useReviews(page, pageSize)`: returns `{ data: PaginatedList<ReviewListItem> | null, loading, error, refetch }`
  - [x] `useReviewDetail(id)`: returns `{ data: ReviewDetailResponse | null, loading, error }`
  - [x] `useSettings()`: returns `{ data: AppSettingsResponse | null, loading, error, updateSettings }`
  - [x] `useCreateReview()`: returns `{ submit, loading, error }`
  - [x] `useDeleteReview()`: returns `{ deleteReview, loading, error }`
  - [x] All normalize errors at the API boundary
  - [x] Explicit return types
- **Output:** `frontend/src/hooks/useReviews.ts`, `useReviewDetail.ts`, `useSettings.ts`, `useCreateReview.ts`, `useDeleteReview.ts`

---

#### TASK FE-11: Review feature components

- **Priority:** P0 | **Estimate:** 20 min | **Depends on:** FE-04, FE-09 | **Prompt:** 34
- **Intent:** Components for displaying review results
- **Acceptance Criteria:**
  - [x] `FindingCard`: displays finding title, severity badge, category, confidence, description, suggestion; toggle for `SuggestedFixPanel`
  - [x] `FindingsList`: accepts `findings[]` + `filters`, renders filtered `FindingCard` list grouped by category
  - [x] `ReviewSummary`: displays total, counts per severity, average confidence
  - [x] `SuggestedFixPanel`: collapsible panel showing `suggestedFix` code
  - [x] Severity colors: Critical=red, Warning=orange, Info=blue
  - [x] Props interfaces above each component
- **Output:** `frontend/src/components/review/FindingCard.tsx`, `FindingsList.tsx`, `ReviewSummary.tsx`, `SuggestedFixPanel.tsx` + CSS modules

---

### Phase 8: Pages (Prompts 35–39, ~40 min)

---

#### TASK FE-12: HomePage

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** FE-06, FE-08 | **Prompt:** 35
- **Intent:** Landing page with app overview and CTA
- **Acceptance Criteria:**
  - [x] App title and description
  - [x] "Start Review" button → navigates to `/review`
  - [x] Feature highlights (categories, severity, history)
  - [x] Admin link to `/admin/history`
- **Output:** `frontend/src/pages/HomePage.tsx` + CSS module

---

#### TASK FE-13: ReviewPage

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** FE-05, FE-09, FE-10 | **Prompt:** 36
- **Intent:** Diff input form, language select, submit
- **Acceptance Criteria:**
  - [x] Textarea for code diff (placeholder text)
  - [x] Language dropdown with 7 supported languages
  - [x] Optional PR URL input
  - [x] Client-side validation: non-empty diff, language selected
  - [x] On submit: calls `useCreateReview`, shows `LoadingSpinner`, navigates to `/review/:id` on success
  - [x] Errors shown via `ErrorMessage`
- **Output:** `frontend/src/pages/ReviewPage.tsx` + CSS module

---

#### TASK FE-14: ReviewResultPage

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** FE-10, FE-11 | **Prompt:** 37
- **Intent:** Full results with filters and summary
- **Acceptance Criteria:**
  - [x] Loads review by `id` from URL params
  - [x] Shows `ReviewSummary` at top
  - [x] Severity filter buttons (All, Critical, Warning, Info)
  - [x] Category filter dropdown (All + 6 categories)
  - [x] `FindingsList` renders filtered findings
  - [x] Original diff displayed in code block (collapsible)
  - [x] Loading and error states handled
- **Output:** `frontend/src/pages/ReviewResultPage.tsx` + CSS module

---

#### TASK FE-15: HistoryPage

- **Priority:** P0 | **Estimate:** 15 min | **Depends on:** FE-07, FE-10 | **Prompt:** 38
- **Intent:** Paginated list of past reviews in admin layout
- **Acceptance Criteria:**
  - [x] Table/list: date, language, snippet (80 chars), finding counts
  - [x] Click row → navigate to `/review/:id`
  - [x] Delete button with confirmation dialog
  - [x] Pagination controls (previous/next)
  - [x] Empty state message when no reviews
  - [x] Loading and error states
- **Output:** `frontend/src/pages/HistoryPage.tsx` + CSS module

---

#### TASK FE-16: AdminSettingsPage

- **Priority:** P1 | **Estimate:** 10 min | **Depends on:** FE-07, FE-10 | **Prompt:** 39
- **Intent:** Toggle mock AI mode, view stats
- **Acceptance Criteria:**
  - [x] Toggle switch for `useMockAi`
  - [x] Current AI model display
  - [x] Save button → calls `useSettings().updateSettings`
  - [x] Success/error feedback
- **Output:** `frontend/src/pages/AdminSettingsPage.tsx` + CSS module

---

### Phase 9: Polish (Prompts 40–41, ~20 min)

---

#### TASK FE-17: Global styling + CSS modules

- **Priority:** P1 | **Estimate:** 15 min | **Depends on:** All pages | **Prompt:** 40
- **Intent:** Consistent visual design across all pages
- **Acceptance Criteria:**
  - [x] CSS variables for colors, spacing, typography
  - [x] Consistent severity colors (red, orange, blue)
  - [x] Responsive layout (desktop-first)
  - [x] Clean, modern appearance
- **Output:** `frontend/src/styles/global.css`, update all CSS modules

---

#### TASK FE-18: Error boundaries

- **Priority:** P1 | **Estimate:** 10 min | **Depends on:** FE-08 | **Prompt:** 41
- **Intent:** Catch React rendering errors per route
- **Acceptance Criteria:**
  - [x] `ErrorBoundary` component wraps each route
  - [x] Shows friendly error message + "Go Home" button
  - [x] Resets state on navigation
  - [x] No white screen on crash
- **Output:** `frontend/src/components/common/ErrorBoundary.tsx`, updated router

---

### Phase 10: Infrastructure & Documentation (Prompts 42–50, ~40 min)

---

#### TASK INF-03: docker-compose.yml

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** All FE/BE | **Prompt:** 42
- **Intent:** Full-stack Docker Compose setup
- **Acceptance Criteria:**
  - [x] 3 services: `frontend` (port 3000), `backend` (port 8080), `db` (port 5432)
  - [x] Health checks on all services
  - [x] `depends_on` with conditions
  - [x] `.env.example` with all required variables
  - [x] `docker compose up -d` starts everything
- **Output:** `docker-compose.yml`, `.env.example`

---

#### TASK INF-01: Frontend Dockerfile

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** FE complete | **Prompt:** 43
- **Intent:** Multi-stage build: node→nginx
- **Acceptance Criteria:**
  - [x] Stage 1: `node:lts-alpine`, `npm ci`, `npm run build`
  - [x] Stage 2: `nginx:alpine`, copy dist + nginx.conf
  - [x] `.dockerignore` present
  - [x] Build arg for `VITE_API_URL`
- **Output:** `frontend/Dockerfile`, `frontend/.dockerignore`, `frontend/nginx.conf`

---

#### TASK INF-02: Backend Dockerfile

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** BE complete | **Prompt:** 44
- **Intent:** Multi-stage build: SDK→runtime with tests
- **Acceptance Criteria:**
  - [x] Stage 1: `dotnet/sdk:9.0`, restore, build, test, publish
  - [x] Stage 2: `dotnet/aspnet:9.0`, HEALTHCHECK, ENTRYPOINT
  - [x] Tests run during build — fail stops build
  - [x] `.dockerignore` present
- **Output:** `backend/Dockerfile`, `backend/.dockerignore`

---

#### TASK INF-04: GitHub Actions CI

- **Priority:** P1 | **Estimate:** 15 min | **Depends on:** INF-03 | **Prompt:** 45
- **Intent:** 3-job CI pipeline
- **Acceptance Criteria:**
  - [x] `frontend` job: lint, build, test, upload coverage
  - [x] `backend` job: restore, build, test, upload results
  - [x] `docker` job: compose build + up + health check (needs frontend+backend)
  - [x] Triggers on `pull_request` to `main`
  - [x] Caches npm + NuGet
- **Output:** `.github/workflows/ci.yml`

---

#### TASK BE-21: Backend unit tests

- **Priority:** P1 | **Estimate:** 20 min | **Depends on:** All BE | **Prompt:** 46
- **Intent:** Tests for handlers, validators, mock AI service
- **Acceptance Criteria:**
  - [x] `CreateReviewCommandHandler` tests: valid input, empty diff
  - [x] `GetReviewsQueryHandler` tests: pagination, empty set
  - [x] `CreateReviewRequestValidator` tests: empty diff fails, valid passes
  - [x] `MockAiAnalysisService` tests: returns findings, varies by language
  - [x] AAA pattern, one assertion per test, `{Method}_{Scenario}_{Expected}` naming
- **Output:** `backend/tests/PRReviewAssistant.Tests/` test files

---

#### TASK FE-tests: Frontend component tests

- **Priority:** P1 | **Estimate:** 15 min | **Depends on:** All FE | **Prompt:** 47
- **Intent:** Key component and hook tests
- **Acceptance Criteria:**
  - [x] `FindingCard` renders title, severity, description
  - [x] `ReviewSummary` renders counts
  - [x] `ErrorBoundary` catches and renders fallback
  - [x] `useReviewDetail` returns data on success, error on failure
- **Output:** `frontend/src/**/*.test.tsx` files

---

#### TASK DOC-01: README.md

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** All | **Prompt:** 48
- **Intent:** Project documentation from template
- **Acceptance Criteria:**
  - [x] Follows template from `5-documentation-templates.md`
  - [x] All sections filled: overview, getting started, API endpoints, AI stats placeholder
  - [x] Docker and local run instructions accurate
- **Output:** `README.md`

---

#### TASK DOC-03: INSIGHTS.md

- **Priority:** P1 | **Estimate:** 10 min | **Depends on:** All | **Prompt:** 49
- **Intent:** Lessons learned document
- **Acceptance Criteria:**
  - [x] Prompts that worked well (≥3)
  - [x] Prompts that didn't work (≥2)
  - [x] Effective patterns
  - [x] Key metrics table
  - [x] Tools assessment
- **Output:** `docs/INSIGHTS.md`

---

#### TASK Final: Validation & cleanup

- **Priority:** P0 | **Estimate:** 10 min | **Depends on:** All | **Prompt:** 50
- **Intent:** Final quality check
- **Acceptance Criteria:**
  - [x] `docker compose up -d` starts all 3 services
  - [x] All 6 API endpoints respond correctly
  - [x] Full user journey works: submit → view → history → delete
  - [x] Frontend builds with 0 errors/warnings
  - [x] Backend builds with 0 errors/warnings
  - [x] All prompt logs exist in `prompts/`
  - [x] No `console.log`, `debugger`, or TODO in code
- **Output:** Validation report

---

## 4. Prompt Map

| Prompt | Task(s) | Output |
|--------|---------|--------|
| 6 | INF-05 | `.gitignore` |
| 7 | BE-01 | .NET 9 scaffold |
| 8 | FE-01, FE-02, FE-03 | React + Vite scaffold |
| 9 | BE-20 | `Program.cs` skeleton |
| 10 | BE-02, BE-03 | Domain entities + enums |
| 11 | BE-02 | `AppSettings` entity |
| 12 | BE-04 | All DTOs + `ApiResponse<T>` |
| 13 | BE-05 | Service + repository interfaces |
| 14 | BE-06 | `AppDbContext` + configurations |
| 15 | BE-08 | Repository implementations |
| 16 | BE-09 | `MockAiAnalysisService` |
| 17 | BE-07 | EF Core migration |
| 18 | BE-10, BE-11 | FluentValidation validators |
| 19 | BE-12 | `CreateReview` handler |
| 20 | BE-13 | `GetReviews` handler |
| 21 | BE-14 | `GetReviewById` handler |
| 22 | BE-15 | `DeleteReview` handler |
| 23 | BE-16 | Settings handlers |
| 24 | BE-18 | Minimal API endpoint maps |
| 25 | BE-19 | `GlobalExceptionHandler` |
| 26 | BE-17 | `Program.cs` final |
| 27 | FE-04 | TypeScript models |
| 28 | FE-05 | Axios API service |
| 29 | FE-06 | `MainLayout` |
| 30 | FE-07 | `AdminLayout` |
| 31 | FE-08 | Router configuration |
| 32 | FE-09 | Common components |
| 33 | FE-10 | Custom hooks |
| 34 | FE-11 | Review feature components |
| 35 | FE-12 | `HomePage` |
| 36 | FE-13 | `ReviewPage` |
| 37 | FE-14 | `ReviewResultPage` |
| 38 | FE-15 | `HistoryPage` |
| 39 | FE-16 | `AdminSettingsPage` |
| 40 | FE-17 | Global styles |
| 41 | FE-18 | Error boundary |
| 42 | INF-03 | `docker-compose.yml` |
| 43 | INF-01 | Frontend Dockerfile |
| 44 | INF-02 | Backend Dockerfile |
| 45 | INF-04 | GitHub Actions CI |
| 46 | BE-21 | Backend unit tests |
| 47 | FE-tests | Frontend component tests |
| 48 | DOC-01 | `README.md` |
| 49 | DOC-03 | `INSIGHTS.md` |
| 50 | All | Final validation + cleanup |

---

## 5. Scaffolding Commands (Windows PowerShell)

### Step 1: Initialize Git repo

```powershell
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant
git init
git checkout -b main
```

### Step 2: Backend scaffold

```powershell
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant

# Create directories
New-Item -ItemType Directory -Path backend\src\PRReviewAssistant.API -Force
New-Item -ItemType Directory -Path backend\tests\PRReviewAssistant.Tests -Force

# Create solution and projects
cd backend
dotnet new sln -n PRReviewAssistant
dotnet new webapi -n PRReviewAssistant.API -o src/PRReviewAssistant.API --use-minimal-apis
dotnet new xunit -n PRReviewAssistant.Tests -o tests/PRReviewAssistant.Tests
dotnet sln add src/PRReviewAssistant.API/PRReviewAssistant.API.csproj
dotnet sln add tests/PRReviewAssistant.Tests/PRReviewAssistant.Tests.csproj
dotnet add tests/PRReviewAssistant.Tests reference src/PRReviewAssistant.API

# Add NuGet packages to API project
cd src/PRReviewAssistant.API
dotnet add package MediatR
dotnet add package FluentValidation.AspNetCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Swashbuckle.AspNetCore
dotnet add package NUlid
dotnet add package Microsoft.EntityFrameworkCore.Design

# Add NuGet packages to test project
cd ../../tests/PRReviewAssistant.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory

# Verify build
cd ../..
dotnet build
```

### Step 3: Frontend scaffold

```powershell
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant
npm create vite@latest frontend -- --template react-ts
cd frontend
npm install
npm install react-router-dom axios
npm install -D @testing-library/react @testing-library/jest-dom @testing-library/user-event
npm install -D jest ts-jest @types/jest jest-environment-jsdom
npm install -D eslint @typescript-eslint/eslint-plugin @typescript-eslint/parser
npm install -D prettier eslint-config-prettier

# Verify build
npm run build
```

### Step 4: Create folder structure

```powershell
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant

# Backend directories
$apiBase = "backend\src\PRReviewAssistant.API"
@(
  "$apiBase\Domain\Entities",
  "$apiBase\Domain\Enums",
  "$apiBase\Domain\Interfaces",
  "$apiBase\Shared\DTOs\Requests",
  "$apiBase\Shared\DTOs\Responses",
  "$apiBase\Shared\DTOs\Common",
  "$apiBase\Shared\Middleware",
  "$apiBase\Infrastructure\Data\Configurations",
  "$apiBase\Infrastructure\Repositories",
  "$apiBase\Infrastructure\Services",
  "$apiBase\Features\Reviews\Commands",
  "$apiBase\Features\Reviews\Queries",
  "$apiBase\Features\Reviews\Validators",
  "$apiBase\Features\Settings\Commands",
  "$apiBase\Features\Settings\Queries",
  "$apiBase\Features\Settings\Validators",
  "$apiBase\Endpoints"
) | ForEach-Object { New-Item -ItemType Directory -Path $_ -Force }

# Frontend directories
$feBase = "frontend\src"
@(
  "$feBase\components\common",
  "$feBase\components\review",
  "$feBase\components\admin",
  "$feBase\hooks",
  "$feBase\layouts",
  "$feBase\pages",
  "$feBase\services",
  "$feBase\models",
  "$feBase\router",
  "$feBase\styles"
) | ForEach-Object { New-Item -ItemType Directory -Path $_ -Force }
```

---

## 6. GitHub Actions Auto-Update Workflow

```yaml
# .github/workflows/update-readme.yml
name: Update README Stats

on:
  push:
    branches: [main]
    paths:
      - 'prompts/**'
      - 'backend/src/**'
      - 'frontend/src/**'

jobs:
  update-readme:
    runs-on: ubuntu-latest
    permissions:
      contents: write
    steps:
      - uses: actions/checkout@v4
        with:
          token: ${{ secrets.GITHUB_TOKEN }}

      - name: Count stats
        run: |
          PROMPT_COUNT=$(find prompts -name '*.md' ! -name '_*' | wc -l)
          BE_FILES=$(find backend/src -name '*.cs' | wc -l)
          FE_FILES=$(find frontend/src -name '*.ts' -o -name '*.tsx' | wc -l)
          TOTAL_FILES=$((BE_FILES + FE_FILES))
          echo "PROMPT_COUNT=$PROMPT_COUNT" >> $GITHUB_ENV
          echo "TOTAL_FILES=$TOTAL_FILES" >> $GITHUB_ENV

      - name: Update README badges
        run: |
          sed -i "s/Total files | [0-9]*/Total files | $TOTAL_FILES/" README.md
          sed -i "s/Prompts: [0-9]*/Prompts: $PROMPT_COUNT/" README.md

      - name: Commit if changed
        run: |
          git config user.name "github-actions[bot]"
          git config user.email "github-actions[bot]@users.noreply.github.com"
          git diff --quiet || (git add README.md && git commit -m "chore(docs): auto-update README stats" && git push)
```

---

## 7. MCP for GitHub — Setup Steps

### Option A: Built-in GitHub MCP (Recommended)

1. **Enable MCP in VS Code settings:**
   ```jsonc
   // .vscode/settings.json
   {
     "github.copilot.chat.mcp.enabled": true
   }
   ```
2. **Add GitHub MCP server** — create `.vscode/mcp.json` in the repo root:
   ```json
   {
     "servers": {
       "github": {
         "command": "npx",
         "args": ["-y", "@modelcontextprotocol/server-github"],
         "env": {
           "GITHUB_PERSONAL_ACCESS_TOKEN": "${env:GITHUB_TOKEN}"
         }
       }
     }
   }
   ```
3. **Create a GitHub Personal Access Token (PAT):**
   - Go to https://github.com/settings/tokens?type=beta
   - Create a fine-grained token with permissions: `Contents: Read and write`, `Pull requests: Read and write`, `Issues: Read and write`, `Metadata: Read`
   - Set env variable: `$env:GITHUB_TOKEN = "ghp_your_token_here"` (or add to system env vars)
4. **Verify:** In Copilot chat, type `@github` — should show repository context options

### Option B: GitHub CLI Auth (Simpler)

1. Install GitHub CLI: `winget install GitHub.cli`
2. Authenticate: `gh auth login`
3. Update `.vscode/mcp.json`:
   ```json
   {
     "servers": {
       "github": {
         "command": "npx",
         "args": ["-y", "@modelcontextprotocol/server-github"]
       }
     }
   }
   ```

### Required GitHub Permissions

- `repo` (full control of private repos) — or fine-grained: Contents, Pull requests, Issues, Metadata
- `workflow` (if you want CI management)

---

## 8. Git Workflow — Exact Commands

### Initial setup

```powershell
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant

# Initialize repo (if not done)
git init
git checkout -b main

# Create remote (using GitHub CLI)
gh repo create ai-pr-review-assistant --public --source=. --remote=origin

# Or manually add remote
git remote add origin https://github.com/YOUR_USERNAME/ai-pr-review-assistant.git
```

### Per-phase workflow (one branch per phase)

```powershell
# Start a phase
git checkout main
git pull origin main
git checkout -b feat/FTG16-phase1-foundation

# Work... commit incrementally
git add -A
git commit -m "chore: add .gitignore for .NET and Node.js"
git add -A
git commit -m "feat(backend): scaffold .NET 9 solution with NuGet packages"

# Push and create PR
git push -u origin feat/FTG16-phase1-foundation
gh pr create --title "feat(phase1): project foundation" --body "Scaffolds backend and frontend projects"

# After review: squash merge
gh pr merge --squash --delete-branch

# Update local main
git checkout main
git pull origin main
```

### Phase branch names

| Phase | Branch Name |
|-------|------------|
| 1 | `feat/FTG16-phase1-foundation` |
| 2 | `feat/FTG16-phase2-backend-contracts` |
| 3 | `feat/FTG16-phase3-backend-infra` |
| 4 | `feat/FTG16-phase4-backend-logic` |
| 5 | `feat/FTG16-phase5-frontend-foundation` |
| 6 | `feat/FTG16-phase6-layouts-routing` |
| 7 | `feat/FTG16-phase7-hooks-components` |
| 8 | `feat/FTG16-phase8-pages` |
| 9 | `feat/FTG16-phase9-polish` |
| 10 | `feat/FTG16-phase10-infra-docs` |

---

## 9. Recommended Agent Mode per Prompt

| Prompts | Mode | Reason |
|---------|------|--------|
| 6–9 | **Agent** | Scaffolding is mechanical |
| 10–13 | **Plan → Agent** | Plan contracts before generating |
| 14–17 | **Agent** | Infra follows contracts |
| 18–26 | **Plan → Agent** | Plan handler logic first |
| 27–28 | **Agent** | Models mirror backend |
| 29–34 | **Plan → Agent** | Plan component APIs first |
| 35–39 | **Agent** | Pages compose existing parts |
| 40–50 | **Agent** | Polish, infra, docs are mechanical |

---

## 10. Definition of Done

A task is complete only when all apply:

- [x] Code compiles without errors or warnings
- [x] Tests pass (existing + new)
- [x] No `console.log`, `debugger`, `TODO`, or unused imports
- [x] File is in the correct location per folder structure in SRS
- [x] Naming follows conventions in `.github/copilot-instructions.md`
- [x] Prompt log saved as `{N}_{key}.md` in `prompts/`
- [x] Committed with Conventional Commits message

The project is fully done when:

- [x] `docker compose up -d` starts all 3 services successfully
- [x] All 6 API endpoints respond correctly (manual check with curl)
- [x] Full user journey works: submit diff → view findings → history → delete
- [x] GitHub Actions CI would pass (frontend lint + build + test, backend build + test, Docker build)
- [x] README.md accurate and complete
- [x] All 45+ prompt logs exist in `prompts/`

---

## 11. Complete Implementation Prompts (6–50)

### PROMPT 6 — Generate .gitignore (INF-05)

**Intent:** Create comprehensive .gitignore for .NET + Node.js + Docker project
**Expected Input:** Project stack context
**Expected Output:** `.gitignore` at repo root
**Usage:** Open new Copilot Agent chat, paste prompt

```
Generate a .gitignore file at the repository root for a project using:
- .NET 9 (backend in backend/)
- React + Vite + TypeScript (frontend in frontend/)
- Docker + Docker Compose
- PostgreSQL
- Visual Studio / VS Code / JetBrains IDEs

Include patterns for:
- .NET: bin/, obj/, *.user, *.suo, .vs/
- Node: node_modules/, dist/, .vite/
- Environment: .env, .env.local, .env.*.local (but NOT .env.example)
- Docker: docker-compose.override.yml
- IDE: .idea/, *.swp, .vscode/ (except settings.json and extensions.json)
- OS: Thumbs.db, .DS_Store, Desktop.ini

Location: .gitignore (repo root)
```

---

### PROMPT 7 — Backend scaffold (BE-01)

**Intent:** Create .NET 9 solution structure with projects and NuGet packages
**Expected Input:** Folder structure from copilot-instructions.md
**Expected Output:** backend/ directory with solution, API project, test project
**Usage:** Run PowerShell commands from Section 5, OR use this in Agent mode

```
Create the .NET 9 backend scaffold for PR Review Assistant.

Structure:
  backend/
    PRReviewAssistant.sln
    src/PRReviewAssistant.API/
      PRReviewAssistant.API.csproj (net9.0, Minimal API)
    tests/PRReviewAssistant.Tests/
      PRReviewAssistant.Tests.csproj (net9.0, xUnit)

Create the directory structure inside the API project:
  Domain/Entities/
  Domain/Enums/
  Domain/Interfaces/
  Shared/DTOs/
  Shared/Middleware/
  Infrastructure/Data/Configurations/
  Infrastructure/Repositories/
  Infrastructure/Services/
  Features/Reviews/Commands/
  Features/Reviews/Queries/
  Features/Reviews/Validators/
  Features/Settings/Commands/
  Features/Settings/Queries/
  Features/Settings/Validators/
  Endpoints/

NuGet packages for API project: MediatR, FluentValidation.AspNetCore, Npgsql.EntityFrameworkCore.PostgreSQL, Swashbuckle.AspNetCore, NUlid, Microsoft.EntityFrameworkCore.Design
NuGet packages for Test project: Moq, FluentAssertions, Microsoft.EntityFrameworkCore.InMemory

Test project must reference the API project.
Verify: dotnet build passes with 0 errors.

Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 8 — Frontend scaffold (FE-01, FE-02, FE-03)

**Intent:** Create React + Vite + TypeScript project with strict config
**Expected Input:** Stack requirements
**Expected Output:** frontend/ directory with all config
**Usage:** Run npm commands from Section 5, OR use this in Agent mode

```
Create the React + Vite + TypeScript frontend scaffold for PR Review Assistant.

Use: npm create vite@latest frontend -- --template react-ts

Then configure:
1. tsconfig.json: strict: true, noUncheckedIndexedAccess: true, noImplicitReturns: true
2. Install dependencies: react-router-dom, axios
3. Install dev dependencies: @testing-library/react, @testing-library/jest-dom, @testing-library/user-event, jest, ts-jest, @types/jest, jest-environment-jsdom, eslint, @typescript-eslint/eslint-plugin, @typescript-eslint/parser, prettier, eslint-config-prettier
4. Create .eslintrc.cjs with TypeScript strict rules, no-any, explicit-return-type
5. Create .prettierrc with singleQuote: true, semi: true, tabWidth: 2

Create directory structure:
  src/components/common/
  src/components/review/
  src/components/admin/
  src/hooks/
  src/layouts/
  src/pages/
  src/services/
  src/models/
  src/router/
  src/styles/

Verify: npm run build passes.
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 9 — Program.cs skeleton (BE-20)

**Intent:** Minimal Program.cs with health endpoint, CORS, Swagger
**Expected Input:** BE-01 scaffold
**Expected Output:** Working `Program.cs`
**Usage:** Agent mode

```
Generate a minimal Program.cs for PRReviewAssistant.API (.NET 9 Minimal API).

This is a skeleton — full DI and middleware will be added later.

Include:
1. WebApplication builder
2. CORS: allow origin http://localhost:5173 (Vite dev server)
3. Swagger/OpenAPI (development only)
4. Health endpoint: app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
5. HTTPS redirection
6. JSON serialization: camelCase property naming, string enums

Do NOT add (yet):
- MediatR registration
- DbContext registration
- Repository/service DI
- Endpoint maps (except health)

Location: backend/src/PRReviewAssistant.API/Program.cs
Verify: dotnet run starts; GET /health returns 200; GET /swagger shows UI.
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 10 — Domain entities + enums (BE-02, BE-03)

**Intent:** Core domain entities and enums
**Expected Input:** Entity definitions from copilot-instructions.md
**Expected Output:** 4 files in Domain/
**Usage:** Agent mode

```
Generate domain entities and enums for PR Review Assistant.

Reference: .github/copilot-instructions.md § Domain Entities and § Enums.

Files to create:

1. Domain/Entities/Review.cs
   - Id: string (ULID), CodeDiff: string, Language: string, PrUrl: string?, CreatedAt: DateTime (UTC)
   - Navigation: List<Finding> Findings
   - No EF attributes (Fluent API only)

2. Domain/Entities/Finding.cs
   - Id: string (ULID), ReviewId: string (FK), Category: FindingCategory, Severity: FindingSeverity
   - Title: string, Description: string, LineReference: string?, Suggestion: string
   - Confidence: int (0–100), SuggestedFix: string?
   - Navigation: Review Review

3. Domain/Enums/FindingCategory.cs
   - Values: Bug, Naming, Performance, Security, CodeStyle, BestPractice

4. Domain/Enums/FindingSeverity.cs
   - Values: Critical, Warning, Info

Rules:
- No framework dependencies in domain layer
- Use string for IDs (ULID generated at creation time)
- XML doc comments on entities and enums
- No constructors — use object initializers

Location: backend/src/PRReviewAssistant.API/Domain/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 11 — AppSettings entity (BE-02)

**Intent:** Singleton settings entity
**Expected Input:** Entity spec from copilot-instructions.md
**Expected Output:** 1 file
**Usage:** Agent mode

```
Generate the AppSettings entity for PR Review Assistant.

Reference: .github/copilot-instructions.md § Domain Entities (AppSettings singleton).

File: Domain/Entities/AppSettings.cs
Fields:
  - Id: string (PK, default: "default")
  - UseMockAi: bool (default: true)
  - AiModel: string (default: "mock")

This is a singleton configuration entity — only one row in the database.
No framework dependencies. XML doc comments.

Location: backend/src/PRReviewAssistant.API/Domain/Entities/AppSettings.cs
```

---

### PROMPT 12 — DTOs + ApiResponse\<T\> (BE-04)

**Intent:** All request/response C# records
**Expected Input:** DTO definitions from copilot-instructions.md
**Expected Output:** Multiple files in Shared/DTOs/
**Usage:** Agent mode

```
Generate all DTO records and the ApiResponse<T> wrapper for PR Review Assistant.

Reference: .github/copilot-instructions.md § DTOs (C# Records).

Files to create in Shared/DTOs/:

1. Requests/CreateReviewRequest.cs
   - record CreateReviewRequest(string CodeDiff, string Language, string? PrUrl)

2. Requests/UpdateSettingsRequest.cs
   - record UpdateSettingsRequest(bool UseMockAi, string AiModel)

3. Responses/ReviewDetailResponse.cs
   - record with: Id, CreatedAt, Language, CodeDiff, PrUrl?, Summary (ReviewSummaryDto), Findings (IReadOnlyList<FindingDto>)

4. Responses/ReviewListItem.cs
   - record with: Id, CreatedAt, Language, CodeSnippet, TotalFindings, CriticalCount, WarningCount, InfoCount

5. Responses/FindingDto.cs
   - record with: all Finding fields as strings (Category, Severity as string, not enum)

6. Responses/ReviewSummaryDto.cs
   - record with: TotalFindings, CriticalCount, WarningCount, InfoCount, AverageConfidence

7. Responses/AppSettingsResponse.cs
   - record with: UseMockAi, AiModel

8. Common/PaginatedList.cs
   - record PaginatedList<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize)

9. Common/ApiResponse.cs
   - record ApiResponse<T>(T? Data, object? Error)

Rules: record syntax, IReadOnlyList<T> for collections, string IDs, camelCase JSON.
Location: backend/src/PRReviewAssistant.API/Shared/DTOs/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 13 — Interfaces (BE-05)

**Intent:** Repository and service contracts
**Expected Input:** Entities and DTOs
**Expected Output:** 3 interface files
**Usage:** Agent mode

```
Generate repository and service interfaces for PR Review Assistant.

Reference: .github/copilot-instructions.md § AI Analysis Service and § Architecture.

Files:

1. Domain/Interfaces/IReviewRepository.cs
   Methods:
   - Task<ReviewDetailResponse?> GetByIdAsync(string id, CancellationToken ct)
   - Task<(IReadOnlyList<ReviewListItem> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct)
   - Task<ReviewDetailResponse> CreateAsync(Review review, CancellationToken ct)
   - Task<bool> DeleteAsync(string id, CancellationToken ct)

2. Domain/Interfaces/ISettingsRepository.cs
   Methods:
   - Task<AppSettings> GetAsync(CancellationToken ct)
   - Task<AppSettings> UpdateAsync(AppSettings settings, CancellationToken ct)

3. Domain/Interfaces/IAiAnalysisService.cs
   Methods:
   - Task<IReadOnlyList<Finding>> AnalyzeAsync(string codeDiff, string language, CancellationToken ct)

All methods: async, CancellationToken parameter, XML doc comments.
Interface naming: IPascalCase.
Location: backend/src/PRReviewAssistant.API/Domain/Interfaces/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 14 — AppDbContext + configurations (BE-06)

**Intent:** EF Core context with Fluent API
**Expected Input:** Entities, interfaces
**Expected Output:** DbContext + 3 configuration files
**Usage:** Agent mode

```
Generate AppDbContext and entity configurations for PR Review Assistant.

Files:

1. Infrastructure/Data/AppDbContext.cs
   - DbSet<Review> Reviews, DbSet<Finding> Findings, DbSet<AppSettings> AppSettings
   - Override OnModelCreating to apply configurations
   - PostgreSQL connection string from IConfiguration

2. Infrastructure/Data/Configurations/ReviewConfiguration.cs
   - Id: string PK, max 26 chars (ULID)
   - CodeDiff: required, max 50000
   - Language: required, max 50
   - PrUrl: optional, max 2000
   - CreatedAt: required
   - HasMany(Findings).WithOne(Review).HasForeignKey(ReviewId).OnDelete(Cascade)

3. Infrastructure/Data/Configurations/FindingConfiguration.cs
   - Id: string PK, max 26 chars
   - Title: required, max 200
   - Description: required, max 2000
   - Suggestion: required, max 2000
   - LineReference: optional, max 50
   - SuggestedFix: optional, max 5000
   - Confidence: required
   - Category and Severity stored as string (enum conversion)

4. Infrastructure/Data/Configurations/AppSettingsConfiguration.cs
   - Id: string PK, max 50
   - Seed data: Id="default", UseMockAi=true, AiModel="mock"

Location: backend/src/PRReviewAssistant.API/Infrastructure/Data/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 15 — Repository implementations (BE-08)

**Intent:** EF Core repository implementations
**Expected Input:** Interfaces, DbContext, DTOs
**Expected Output:** 2 repository files
**Usage:** Agent mode

```
Generate repository implementations for PR Review Assistant.

Files:

1. Infrastructure/Repositories/ReviewRepository.cs
   Implements IReviewRepository. Inject AppDbContext.
   - GetByIdAsync: Include Findings, AsNoTracking, map to ReviewDetailResponse with ReviewSummaryDto
   - GetAllAsync: OrderByDescending(CreatedAt), Skip/Take, project to ReviewListItem (CodeSnippet = first 80 chars of CodeDiff), return tuple (items, totalCount)
   - CreateAsync: Set Id = Ulid.NewUlid().ToString(), CreatedAt = DateTime.UtcNow, add + save, return mapped response
   - DeleteAsync: Find by Id, remove if found, return bool

2. Infrastructure/Repositories/SettingsRepository.cs
   Implements ISettingsRepository. Inject AppDbContext.
   - GetAsync: Find "default" settings, create default if not found
   - UpdateAsync: Find "default", update fields, save

Use AsNoTracking() for read queries.
Map entities to DTOs inside the repository.
CancellationToken on all EF calls.
Location: backend/src/PRReviewAssistant.API/Infrastructure/Repositories/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 16 — MockAiAnalysisService (BE-09)

**Intent:** Realistic mock AI that returns varied findings
**Expected Input:** IAiAnalysisService interface
**Expected Output:** 1 service file
**Usage:** Agent mode

```
Generate MockAiAnalysisService for PR Review Assistant.

Implements IAiAnalysisService. No external API calls.

Behavior:
- Returns 3–8 findings per call (based on diff length: longer diff = more findings)
- All 6 FindingCategory values used across calls
- All 3 FindingSeverity values represented
- Realistic titles, descriptions, suggestions per category:
  * Bug: "Potential null reference", "Missing null check", "Off-by-one error"
  * Naming: "Variable naming convention", "Method name too generic"
  * Performance: "N+1 query potential", "Unnecessary allocation"
  * Security: "SQL injection risk", "Unvalidated input"
  * CodeStyle: "Magic number", "Long method"
  * BestPractice: "Missing error handling", "Hardcoded configuration"
- Each finding: random Confidence 40–98, random LineReference "L{n}" or "L{n}-L{m}"
- SuggestedFix: 60% of findings have a code suggestion, rest null
- Language-aware: adjust finding descriptions for C#, TypeScript, etc.
- Simulate 300–800ms processing delay with Task.Delay
- Generate ULID for each Finding.Id

Location: backend/src/PRReviewAssistant.API/Infrastructure/Services/MockAiAnalysisService.cs
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 17 — EF Core migration (BE-07)

**Intent:** Create InitialCreate migration
**Expected Input:** DbContext with configurations
**Expected Output:** Migration files
**Usage:** Run this command in terminal (not a Copilot prompt)

```powershell
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant\backend\src\PRReviewAssistant.API
dotnet ef migrations add InitialCreate --output-dir Infrastructure/Data/Migrations
```

---

### PROMPT 18 — FluentValidation validators (BE-10, BE-11)

**Intent:** Input validators for request DTOs
**Expected Input:** Request DTOs
**Expected Output:** 2 validator files
**Usage:** Agent mode

```
Generate FluentValidation validators for PR Review Assistant.

Files:

1. Features/Reviews/Validators/CreateReviewRequestValidator.cs
   Validates CreateReviewRequest:
   - CodeDiff: NotEmpty, MaximumLength(50000), WithMessage("Code diff is required") / WithMessage("Code diff must not exceed 50000 characters")
   - Language: NotEmpty, Must(lang => new[] { "C#", "TypeScript", "JavaScript", "Python", "Java", "Go", "Other" }.Contains(lang)), WithMessage("Language is required") / WithMessage("Unsupported language")
   - PrUrl: When not null, must be valid URL format, MaximumLength(2000)

2. Features/Settings/Validators/UpdateSettingsRequestValidator.cs
   Validates UpdateSettingsRequest:
   - AiModel: NotEmpty, MaximumLength(100)

Rules: AbstractValidator<T>, one rule chain per property, WithMessage on every rule, no business logic.
Location: backend/src/PRReviewAssistant.API/Features/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 19 — CreateReview handler (BE-12)

**Intent:** Submit diff for AI analysis
**Expected Input:** Interfaces, DTOs, validators
**Expected Output:** Command + Handler files
**Usage:** Agent mode

```
Generate CreateReview MediatR Command + Handler for PR Review Assistant.

Files:
1. Features/Reviews/Commands/CreateReviewCommand.cs
   - record CreateReviewCommand(string CodeDiff, string Language, string? PrUrl) : IRequest<ReviewDetailResponse>

2. Features/Reviews/Commands/CreateReviewCommandHandler.cs
   Inject: IReviewRepository, IAiAnalysisService
   Handler logic:
   1. Call IAiAnalysisService.AnalyzeAsync(command.CodeDiff, command.Language, ct)
   2. Create Review entity: Id = Ulid.NewUlid().ToString(), set all fields, CreatedAt = DateTime.UtcNow
   3. Attach findings from AI service to the review
   4. Call IReviewRepository.CreateAsync(review, ct)
   5. Return ReviewDetailResponse

CancellationToken on all async calls. No business logic leakage.
Location: backend/src/PRReviewAssistant.API/Features/Reviews/Commands/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 20 — GetReviews handler (BE-13)

**Intent:** Paginated review list
**Expected Input:** Repository interface
**Expected Output:** Query + Handler
**Usage:** Agent mode

```
Generate GetReviews MediatR Query + Handler.

1. Features/Reviews/Queries/GetReviewsQuery.cs
   - record GetReviewsQuery(int Page = 1, int PageSize = 20) : IRequest<PaginatedList<ReviewListItem>>

2. Features/Reviews/Queries/GetReviewsQueryHandler.cs
   Inject: IReviewRepository
   Handler: call repository.GetAllAsync(query.Page, query.PageSize, ct), return PaginatedList<ReviewListItem>

Location: backend/src/PRReviewAssistant.API/Features/Reviews/Queries/
```

---

### PROMPT 21 — GetReviewById handler (BE-14)

**Intent:** Single review with findings
**Expected Input:** Repository interface
**Expected Output:** Query + Handler
**Usage:** Agent mode

```
Generate GetReviewById MediatR Query + Handler.

1. Features/Reviews/Queries/GetReviewByIdQuery.cs
   - record GetReviewByIdQuery(string Id) : IRequest<ReviewDetailResponse?>

2. Features/Reviews/Queries/GetReviewByIdQueryHandler.cs
   Inject: IReviewRepository
   Handler: call repository.GetByIdAsync(query.Id, ct), return result (null if not found)

Location: backend/src/PRReviewAssistant.API/Features/Reviews/Queries/
```

---

### PROMPT 22 — DeleteReview handler (BE-15)

**Intent:** Delete review with cascade
**Expected Input:** Repository interface
**Expected Output:** Command + Handler
**Usage:** Agent mode

```
Generate DeleteReview MediatR Command + Handler.

1. Features/Reviews/Commands/DeleteReviewCommand.cs
   - record DeleteReviewCommand(string Id) : IRequest<bool>

2. Features/Reviews/Commands/DeleteReviewCommandHandler.cs
   Inject: IReviewRepository
   Handler: call repository.DeleteAsync(command.Id, ct), return bool

Location: backend/src/PRReviewAssistant.API/Features/Reviews/Commands/
```

---

### PROMPT 23 — Settings handlers (BE-16)

**Intent:** Get and update app settings
**Expected Input:** Repository interface
**Expected Output:** 4 files
**Usage:** Agent mode

```
Generate Settings MediatR Handlers for PR Review Assistant.

Files:
1. Features/Settings/Queries/GetSettingsQuery.cs
   - record GetSettingsQuery : IRequest<AppSettingsResponse>

2. Features/Settings/Queries/GetSettingsQueryHandler.cs
   Inject: ISettingsRepository
   Map AppSettings entity to AppSettingsResponse

3. Features/Settings/Commands/UpdateSettingsCommand.cs
   - record UpdateSettingsCommand(bool UseMockAi, string AiModel) : IRequest<AppSettingsResponse>

4. Features/Settings/Commands/UpdateSettingsCommandHandler.cs
   Inject: ISettingsRepository
   Update settings, map result to AppSettingsResponse

CancellationToken on all async calls.
Location: backend/src/PRReviewAssistant.API/Features/Settings/
```

---

### PROMPT 24 — Endpoint maps (BE-18)

**Intent:** Map all REST routes to MediatR
**Expected Input:** All handlers
**Expected Output:** 2 endpoint files
**Usage:** Agent mode

```
Generate Minimal API endpoint maps for PR Review Assistant.

Files:

1. Endpoints/ReviewEndpoints.cs
   Static class with MapReviewEndpoints(this WebApplication app) extension method.
   Route group: /api/v1/reviews
   
   Endpoints:
   - POST / → mediator.Send(new CreateReviewCommand(request.CodeDiff, request.Language, request.PrUrl))
     Return: Results.Created($"/api/v1/reviews/{result.Id}", new ApiResponse<ReviewDetailResponse>(result, null))
   
   - GET / → mediator.Send(new GetReviewsQuery(page, pageSize))
     Query params: page (default 1), pageSize (default 20)
     Return: Results.Ok(new ApiResponse<PaginatedList<ReviewListItem>>(result, null))
   
   - GET /{id} → mediator.Send(new GetReviewByIdQuery(id))
     If null: Results.NotFound with ProblemDetails
     Return: Results.Ok(new ApiResponse<ReviewDetailResponse>(result, null))
   
   - DELETE /{id} → mediator.Send(new DeleteReviewCommand(id))
     If false: Results.NotFound with ProblemDetails
     Return: Results.NoContent()

2. Endpoints/SettingsEndpoints.cs
   Static class with MapSettingsEndpoints(this WebApplication app) extension method.
   Route group: /api/v1/settings
   
   - GET / → mediator.Send(new GetSettingsQuery())
   - PUT / → mediator.Send(new UpdateSettingsCommand(request.UseMockAi, request.AiModel))

No business logic in endpoints — only mediator.Send().
Wrap responses in ApiResponse<T>.
Location: backend/src/PRReviewAssistant.API/Endpoints/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 25 — GlobalExceptionHandler (BE-19)

**Intent:** Centralized error handling middleware
**Expected Input:** .NET 9 IExceptionHandler
**Expected Output:** 1 middleware file
**Usage:** Agent mode

```
Generate GlobalExceptionHandler for PR Review Assistant.

File: Shared/Middleware/GlobalExceptionHandler.cs

Implement IExceptionHandler (.NET 9's built-in interface).

Handle:
- FluentValidation.ValidationException → 400 Bad Request with ProblemDetails containing field errors in Extensions
- KeyNotFoundException → 404 Not Found with ProblemDetails
- OperationCanceledException → 499 (log as Information, client cancelled)
- All other exceptions → 500 Internal Server Error with generic message "An unexpected error occurred"

Rules:
- Never expose stack traces or internal messages to client
- Log with ILogger<GlobalExceptionHandler>: Warning for 4xx, Error for 5xx
- Return ProblemDetails RFC 7807 format always
- Include correlation ID (from HttpContext.TraceIdentifier) in ProblemDetails instance

Location: backend/src/PRReviewAssistant.API/Shared/Middleware/
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 26 — Program.cs final (BE-17)

**Intent:** Complete DI and middleware pipeline
**Expected Input:** All backend components
**Expected Output:** Updated Program.cs
**Usage:** Agent mode

```
Update Program.cs to register all services and middleware for PR Review Assistant.

Update the existing Program.cs (from Prompt 9) to add:

DI Registration:
- builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly))
- builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly)
- builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString))
- builder.Services.AddScoped<IReviewRepository, ReviewRepository>()
- builder.Services.AddScoped<ISettingsRepository, SettingsRepository>()
- builder.Services.AddScoped<IAiAnalysisService, MockAiAnalysisService>()
- builder.Services.AddExceptionHandler<GlobalExceptionHandler>()
- builder.Services.AddProblemDetails()

Middleware Pipeline (order matters):
1. app.UseExceptionHandler()
2. app.UseHttpsRedirection()
3. app.UseCors()
4. app.UseSwagger() + app.UseSwaggerUI() (dev only)

Endpoint Registration:
- app.MapReviewEndpoints()
- app.MapSettingsEndpoints()
- app.MapGet("/health", ...) (keep existing)

Apply pending migrations on startup (dev only):
  using var scope = app.Services.CreateScope();
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  db.Database.Migrate();

Connection string from: builder.Configuration.GetConnectionString("Default")
Add appsettings.Development.json with: "ConnectionStrings": { "Default": "Host=localhost;Port=5432;Database=prreview;Username=appuser;Password=apppassword" }

Verify: dotnet build passes; dotnet run starts; all 6 endpoints respond.
Location: backend/src/PRReviewAssistant.API/Program.cs
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 27 — TypeScript models (FE-04)

**Intent:** Mirror backend DTOs as TypeScript interfaces
**Expected Input:** copilot-instructions.md TypeScript Interfaces section
**Expected Output:** Model files
**Usage:** Agent mode

```
Generate TypeScript models for PR Review Assistant frontend.

Reference: .github/copilot-instructions.md § TypeScript Interfaces (copy exactly).

File: frontend/src/models/index.ts

Include all interfaces:
- ReviewDetailResponse, ReviewListItem, Finding, ReviewSummary
- AppSettingsResponse, PaginatedList<T>
- FindingCategory (type alias: string union of 6 values)
- FindingSeverity (type alias: string union of 3 values)
- CreateReviewRequest, UpdateSettingsRequest
- ApiResponse<T> { data: T | null; error: ProblemDetails | null }
- ProblemDetails { type?: string; title?: string; status?: number; detail?: string; instance?: string; errors?: Record<string, string[]> }
- ApiError { message: string; code?: string }

No any types. All exported. Strict mode compatible.
Location: frontend/src/models/index.ts
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 28 — API service (FE-05)

**Intent:** Typed Axios client with error normalization
**Expected Input:** TypeScript models
**Expected Output:** API service file
**Usage:** Agent mode

```
Generate the Axios API service layer for PR Review Assistant frontend.

File: frontend/src/services/api.ts

Configuration:
- Base URL: import.meta.env.VITE_API_URL || 'http://localhost:5000'
- Create Axios instance with baseURL and JSON content type
- Response interceptor: extract .data from ApiResponse<T> wrapper
- Error interceptor: normalize errors to ApiError { message: string; code?: string }

Export these typed functions:
- createReview(request: CreateReviewRequest): Promise<ReviewDetailResponse>
- getReviews(page?: number, pageSize?: number): Promise<PaginatedList<ReviewListItem>>
- getReviewById(id: string): Promise<ReviewDetailResponse>
- deleteReview(id: string): Promise<void>
- getSettings(): Promise<AppSettingsResponse>
- updateSettings(request: UpdateSettingsRequest): Promise<AppSettingsResponse>

No any types. Explicit return types on all functions.
Error handling: catch AxiosError, extract ProblemDetails from response, normalize to ApiError.
Location: frontend/src/services/api.ts
Follow conventions from .github/copilot-instructions.md.
```

---

### PROMPT 29 — MainLayout (FE-06)

**Intent:** Main application layout with header, nav, footer
**Expected Input:** Route structure
**Expected Output:** Layout component + CSS module
**Usage:** Agent mode

```
Generate MainLayout for PR Review Assistant.
Header: app name "PR Review Assistant", nav links: Home (/), New Review (/review)
Active link highlighted with CSS class. Footer: "Powered by AI · v1.0"
Uses <Outlet /> from react-router-dom. CSS Module for styling.
Location: frontend/src/layouts/MainLayout.tsx + MainLayout.module.css
```

---

### PROMPT 30 — AdminLayout (FE-07)

**Intent:** Admin layout with sidebar
**Expected Input:** Route structure
**Expected Output:** Layout component + CSS module
**Usage:** Agent mode

```
Generate AdminLayout for PR Review Assistant.
Sidebar: links to History (/admin/history), Settings (/admin/settings).
Top bar: "Admin" badge, "← Back to Main" link to /.
Uses <Outlet /> for content. CSS Module for styling.
Location: frontend/src/layouts/AdminLayout.tsx + AdminLayout.module.css
```

---

### PROMPT 31 — Router (FE-08)

**Intent:** React Router v6 nested route configuration
**Expected Input:** Layouts, page stubs
**Expected Output:** Router config + updated App.tsx
**Usage:** Agent mode

```
Generate React Router v6 configuration for PR Review Assistant.
Nested routes:
  MainLayout wrapper:
    / → HomePage, /review → ReviewPage, /review/:id → ReviewResultPage
  AdminLayout wrapper:
    /admin/history → HistoryPage, /admin/settings → AdminSettingsPage
  Catch-all: redirect to /

Use createBrowserRouter + RouterProvider pattern.
Pages can be placeholder stubs initially (just <h1>Page Name</h1>).
Update App.tsx to use RouterProvider.
Location: frontend/src/router/index.tsx, update frontend/src/App.tsx
```

---

### PROMPT 32 — Common components (FE-09)

**Intent:** Shared UI primitives
**Expected Input:** TypeScript models
**Expected Output:** 4 components + CSS modules
**Usage:** Agent mode

```
Generate common UI components for PR Review Assistant:
1. ErrorMessage.tsx — Props: { message: string }, renders red alert box
2. LoadingSpinner.tsx — Props: { message?: string }, renders CSS spinner with optional text
3. SeverityBadge.tsx — Props: { severity: FindingSeverity }, colored badge (Critical=red, Warning=orange, Info=blue)
4. ConfidenceBadge.tsx — Props: { confidence: number }, percentage badge with color (green>80, yellow>50, red≤50)

All: Props interface above component, named export, CSS Module per component.
Location: frontend/src/components/common/
```

---

### PROMPT 33 — Custom hooks (FE-10)

**Intent:** Data-fetching hooks for all API endpoints
**Expected Input:** API service
**Expected Output:** 5 hook files
**Usage:** Agent mode

```
Generate custom hooks for PR Review Assistant:
1. useReviews(page, pageSize) — fetches paginated reviews, returns { data, loading, error, refetch }
2. useReviewDetail(id) — fetches single review, returns { data, loading, error }
3. useCreateReview() — returns { submit(request), loading, error, data }
4. useDeleteReview() — returns { deleteReview(id), loading, error }
5. useSettings() — fetches settings, returns { data, loading, error, updateSettings(request) }

All use api.ts service. Explicit return types. Error normalized to string | null.
useEffect for data fetching with cleanup. No any types.
Location: frontend/src/hooks/
```

---

### PROMPT 34 — Review components (FE-11)

**Intent:** Feature components for displaying review results
**Expected Input:** Models, common components
**Expected Output:** 4 components + CSS modules
**Usage:** Agent mode

```
Generate review feature components for PR Review Assistant:
1. FindingCard.tsx — Props: { finding: Finding }. Displays: severity badge, category, title, confidence badge, description, suggestion. Collapsible SuggestedFixPanel if suggestedFix exists.
2. FindingsList.tsx — Props: { findings: Finding[], severityFilter?: FindingSeverity, categoryFilter?: FindingCategory }. Groups findings by category, filters applied.
3. ReviewSummary.tsx — Props: { summary: ReviewSummary }. Cards showing total, critical (red), warning (orange), info (blue), average confidence.
4. SuggestedFixPanel.tsx — Props: { code: string }. Collapsible panel with code block showing the suggested fix.

Severity colors: Critical=#dc3545, Warning=#fd7e14, Info=#0d6efd.
Location: frontend/src/components/review/
```

---

### PROMPT 35 — HomePage (FE-12)

**Intent:** Landing page with CTA
**Expected Input:** MainLayout, router
**Expected Output:** Page component + CSS module
**Usage:** Agent mode

```
Generate HomePage for PR Review Assistant.
Route: /. Layout: MainLayout.
Content: Hero section with app title, description, "Start Review" button → /review.
Feature cards: 6 categories, severity levels, review history.
Link to admin: /admin/history.
Location: frontend/src/pages/HomePage.tsx + HomePage.module.css
```

---

### PROMPT 36 — ReviewPage (FE-13)

**Intent:** Code diff input form
**Expected Input:** API service, hooks, common components
**Expected Output:** Page component + CSS module
**Usage:** Agent mode

```
Generate ReviewPage for PR Review Assistant.
Route: /review. Layout: MainLayout.
Form:
  - Textarea for code diff (min 10 rows, placeholder with example diff)
  - Dropdown: language selection (C#, TypeScript, JavaScript, Python, Java, Go, Other)
  - Optional text input: GitHub PR URL
  - "Analyze" submit button
Validation: diff not empty, language selected. Show ErrorMessage on validation fail.
On submit: useCreateReview().submit(), show LoadingSpinner, on success navigate to /review/:id.
Location: frontend/src/pages/ReviewPage.tsx + ReviewPage.module.css
```

---

### PROMPT 37 — ReviewResultPage (FE-14)

**Intent:** Results page with filters
**Expected Input:** Hooks, review components
**Expected Output:** Page component + CSS module
**Usage:** Agent mode

```
Generate ReviewResultPage for PR Review Assistant.
Route: /review/:id. Layout: MainLayout.
Loads review via useReviewDetail(id from URL params).
Top: ReviewSummary component.
Filters: severity buttons (All, Critical, Warning, Info), category dropdown.
Main: FindingsList with applied filters.
Collapsible section: original code diff in <pre><code> block.
Handle loading (LoadingSpinner) and error (ErrorMessage) states.
Handle not-found (review doesn't exist).
Location: frontend/src/pages/ReviewResultPage.tsx + ReviewResultPage.module.css
```

---

### PROMPT 38 — HistoryPage (FE-15)

**Intent:** Paginated review history in admin layout
**Expected Input:** AdminLayout, hooks
**Expected Output:** Page component + CSS module
**Usage:** Agent mode

```
Generate HistoryPage for PR Review Assistant.
Route: /admin/history. Layout: AdminLayout.
Uses useReviews(page, pageSize=20).
Table: date (formatted), language, code snippet (80 chars), total findings, critical count, warning count, info count.
Click row → navigate to /review/:id.
Delete button per row with confirm dialog → useDeleteReview.
Pagination: Previous/Next buttons, page indicator.
Empty state: "No reviews yet" message.
Handle loading and error states.
Location: frontend/src/pages/HistoryPage.tsx + HistoryPage.module.css
```

---

### PROMPT 39 — AdminSettingsPage (FE-16)

**Intent:** Settings management page
**Expected Input:** AdminLayout, hooks
**Expected Output:** Page component + CSS module
**Usage:** Agent mode

```
Generate AdminSettingsPage for PR Review Assistant.
Route: /admin/settings. Layout: AdminLayout.
Uses useSettings().
Form: toggle switch for useMockAi, text display for current aiModel.
Save button → updateSettings(). Success toast/message. Error via ErrorMessage.
Location: frontend/src/pages/AdminSettingsPage.tsx + AdminSettingsPage.module.css
```

---

### PROMPT 40 — Styling (FE-17)

**Intent:** Global CSS and consistent design
**Expected Input:** All components and pages
**Expected Output:** Global stylesheet + updated CSS modules
**Usage:** Agent mode

```
Generate global CSS and update component styles for PR Review Assistant.
File: frontend/src/styles/global.css
CSS variables: --color-primary, --color-critical (#dc3545), --color-warning (#fd7e14), --color-info (#0d6efd), --color-bg, --color-text, --spacing-*, --font-family, --border-radius.
Import in main.tsx.
Desktop-first responsive (min-width: 768px breakpoint).
Clean, modern look: card-based layouts, subtle shadows, clear typography.
Update all existing CSS modules to use CSS variables.
```

---

### PROMPT 41 — Error boundaries (FE-18)

**Intent:** React error boundary component
**Expected Input:** Router config
**Expected Output:** ErrorBoundary component + updated router
**Usage:** Agent mode

```
Generate ErrorBoundary component for PR Review Assistant.
File: frontend/src/components/common/ErrorBoundary.tsx
Class component (required for error boundaries in React).
Catches render errors, displays: error message, "Go Home" button (link to /).
Resets error state on navigation (componentDidUpdate checking location).
Wrap each route in the router config with <ErrorBoundary>.
Update frontend/src/router/index.tsx to integrate.
```

---

### PROMPT 42 — Docker Compose (INF-03)

**Intent:** Full-stack Docker Compose setup
**Expected Input:** All FE/BE code
**Expected Output:** docker-compose.yml + .env.example
**Usage:** Agent mode

```
Generate docker-compose.yml and .env.example for PR Review Assistant.

docker-compose.yml at repo root:
  frontend: build ./frontend, ports 3000:80, depends_on backend (healthy)
  backend: build ./backend, ports 8080:8080, env ConnectionStrings__Default, depends_on db (healthy), healthcheck curl /health
  db: postgres:16-alpine, env POSTGRES_DB/USER/PASSWORD from .env, volume db_data, healthcheck pg_isready

.env.example:
  DB_PASSWORD=your_password_here
  POSTGRES_DB=prreview
  POSTGRES_USER=appuser
  VITE_API_URL=http://localhost:8080

Follow conventions from .github/prompts/dockerize.prompt.md.
```

---

### PROMPT 43 — Frontend Dockerfile (INF-01)

**Intent:** Multi-stage frontend Docker build
**Expected Input:** Frontend project
**Expected Output:** Dockerfile + nginx.conf + .dockerignore
**Usage:** Agent mode

```
Generate frontend Dockerfile and nginx.conf for PR Review Assistant.
frontend/Dockerfile: two-stage (node:lts-alpine → nginx:alpine), npm ci, npm run build, copy dist.
frontend/nginx.conf: serve SPA (try_files $uri /index.html), proxy /api to backend:8080.
frontend/.dockerignore: node_modules, dist, .env.
Build arg: VITE_API_URL.
```

---

### PROMPT 44 — Backend Dockerfile (INF-02)

**Intent:** Multi-stage backend Docker build with tests
**Expected Input:** Backend project
**Expected Output:** Dockerfile + .dockerignore
**Usage:** Agent mode

```
Generate backend Dockerfile for PR Review Assistant.
backend/Dockerfile: two-stage (dotnet/sdk:9.0 → dotnet/aspnet:9.0).
Stage 1: restore, build, test, publish.
Stage 2: copy published output, EXPOSE 8080, HEALTHCHECK, ENTRYPOINT dotnet PRReviewAssistant.API.dll.
backend/.dockerignore: bin, obj, .env.
Tests must pass during build or build fails.
```

---

### PROMPT 45 — GitHub Actions CI (INF-04)

**Intent:** 3-job CI pipeline
**Expected Input:** Docker setup
**Expected Output:** CI workflow file
**Usage:** Agent mode

```
Generate GitHub Actions CI workflow for PR Review Assistant.
File: .github/workflows/ci.yml
Follow conventions from .github/prompts/setup-ci.prompt.md.
3 jobs: frontend (lint+build+test), backend (restore+build+test), docker (compose build+up+health).
Trigger: pull_request to main.
Cache npm and NuGet.
Upload test artifacts.
```

---

### PROMPT 46 — Backend tests (BE-21)

**Intent:** Unit tests for handlers, validators, mock AI
**Expected Input:** All backend components
**Expected Output:** Test files
**Usage:** Agent mode

```
Generate xUnit tests for PR Review Assistant backend.
Location: backend/tests/PRReviewAssistant.Tests/

Test files:
1. CreateReviewCommandHandlerTests.cs
   - CreateReview_WithValidDiff_ReturnsReviewWithFindings
   - CreateReview_CallsAiService_WithCorrectParameters

2. GetReviewsQueryHandlerTests.cs
   - GetReviews_WithDefaultPagination_ReturnsFirstPage
   - GetReviews_EmptyDatabase_ReturnsEmptyList

3. CreateReviewRequestValidatorTests.cs
   - Validate_EmptyCodeDiff_FailsValidation
   - Validate_ValidRequest_PassesValidation
   - Validate_ExceedsMaxLength_FailsValidation

4. MockAiAnalysisServiceTests.cs
   - AnalyzeAsync_ReturnsFindingsWithVariety
   - AnalyzeAsync_FindingsHaveValidConfidenceRange

Use Moq for mocking, FluentAssertions for assertions.
AAA pattern. One assertion per test. {Method}_{Scenario}_{Expected} naming.
```

---

### PROMPT 47 — Frontend tests (FE-tests)

**Intent:** Component and hook tests
**Expected Input:** All frontend components
**Expected Output:** Test files
**Usage:** Agent mode

```
Generate Jest + RTL tests for PR Review Assistant frontend.
Location: alongside components (co-located).

Test files:
1. FindingCard.test.tsx — renders title, severity badge, description
2. ReviewSummary.test.tsx — displays correct counts and colors
3. ErrorBoundary.test.tsx — catches error and shows fallback
4. ErrorMessage.test.tsx — renders message prop

Use @testing-library/react. No any types.
AAA pattern where applicable.
```

---

### PROMPT 48 — README.md (DOC-01)

**Intent:** Complete project documentation
**Expected Input:** Template from 5-documentation-templates.md
**Expected Output:** README.md at repo root
**Usage:** Agent mode

```
Generate README.md for PR Review Assistant.
Follow template from project-requarements/5-documentation-templates.md (Template 1).
Fill all sections with actual project information.
Include: overview, features, architecture, getting started (Docker + local), API endpoints table, documentation links, tools used, AI generation stats (placeholders for now).
Location: README.md (repo root)
```

---

### PROMPT 49 — INSIGHTS.md (DOC-03)

**Intent:** AI development lessons learned
**Expected Input:** Development experience
**Expected Output:** INSIGHTS.md
**Usage:** Agent mode

```
Generate docs/INSIGHTS.md for PR Review Assistant.
Follow template from project-requarements/5-documentation-templates.md (Template 3).
Fill sections based on actual development experience:
- Prompts that worked well (at least 3)
- Prompts that didn't work (at least 2)
- Effective prompting patterns (5 patterns)
- What would be done differently
- Key metrics table (fill with actual numbers)
- Tools assessment
Location: docs/INSIGHTS.md
```

---

### PROMPT 50 — Final validation (All)

**Intent:** End-to-end quality check
**Expected Input:** Complete project
**Expected Output:** Validation report
**Usage:** Agent mode

```
Perform final validation of PR Review Assistant:
1. Run: cd backend && dotnet build && dotnet test
2. Run: cd frontend && npm run build && npm test -- --passWithNoTests
3. Run: docker compose build && docker compose up -d
4. Test: curl http://localhost:8080/health
5. Test: curl http://localhost:3000
6. Test full journey:
   - POST /api/v1/reviews with sample diff → should return findings
   - GET /api/v1/reviews → should list the review
   - GET /api/v1/reviews/{id} → should return detail
   - DELETE /api/v1/reviews/{id} → should delete

Report: any failures, missing prompts, TODO items, console.log remnants.
Fix all issues before final commit.
```

---

## 12. What to Do Next — First Steps

### Immediate actions (before starting implementation):

1. **Fix the 4 documentation issues** (items 1–4 from readiness assessment) — ~5 min
2. **Create GitHub repo** (run `gh repo create` or create via browser)
3. **Run scaffolding commands** from Section 5 above
4. **Initial commit:** `.github/`, `docs/`, `project-requarements/`, `prompts/_prompt-log-template.md`

### First implementation task:

**Prompt 6 → INF-05: Generate `.gitignore`**

Open a new Copilot chat, switch to Agent mode, and use this prompt:

> Generate a `.gitignore` file for the PR Review Assistant project at the repository root. The project uses: .NET 9 (backend), React + Vite + TypeScript (frontend), Docker, PostgreSQL. Include patterns for: `bin/`, `obj/`, `node_modules/`, `dist/`, `.env`, `*.user`, `.vs/`, `.idea/`, `docker-compose.override.yml`, OS files (Thumbs.db, .DS_Store), and IDE files. Follow conventions from `.github/copilot-instructions.md`.

### Then proceed sequentially:

- **Prompt 7** → Backend scaffold (or run shell commands manually)
- **Prompt 8** → Frontend scaffold
- **Prompt 9** → Program.cs skeleton
- Continue through prompts 10–50 in order
