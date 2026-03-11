# PR Review Assistant — Backlog & Implementation Plan

---

## Sprint Plan

**Total time:** ~4 hours (2 days × 2 hrs/day)
**Goal:** Working end-to-end application with 90%+ AI-generated code

---

## Phase Breakdown

### Phase 1: Foundation (~40 min)
Scaffold projects, configure tooling, establish patterns.

### Phase 2: Backend Core (~50 min)
Domain, data access, services, endpoints.

### Phase 3: Frontend Core (~60 min)
Layouts, routing, pages, components.

### Phase 4: Integration & Polish (~30 min)
Connect frontend to backend, error handling, styling.

### Phase 5: Infrastructure & Docs (~20 min)
Docker, CI, documentation.

---

## Backend Tasks

| ID | Task | Priority | Est. | Phase | Depends On |
|----|------|----------|------|-------|------------|
| BE-01 | Create .NET 9 solution + Minimal API project scaffold | P0 | 5m | 1 | — |
| BE-02 | Generate domain entities (Review, Finding) and enums (FindingCategory, FindingSeverity) | P0 | 10m | 2 | BE-01 |
| BE-03 | Generate AppSettings entity | P1 | 5m | 2 | BE-01 |
| BE-04 | Generate DTOs as C# records (requests + responses) | P0 | 10m | 2 | BE-02 |
| BE-05 | Generate AppDbContext with PostgreSQL config + entity configurations | P0 | 10m | 2 | BE-02, BE-03 |
| BE-06 | Generate IReviewRepository + ReviewRepository | P0 | 10m | 2 | BE-05 |
| BE-07 | Generate ISettingsRepository + SettingsRepository | P1 | 5m | 2 | BE-05 |
| BE-08 | Generate IAiAnalysisService interface | P0 | 5m | 2 | BE-04 |
| BE-09 | Generate MockAiAnalysisService (realistic varied findings) | P0 | 15m | 2 | BE-08 |
| BE-10 | Generate CreateReview Command + Handler (MediatR) | P0 | 10m | 2 | BE-06, BE-09 |
| BE-11 | Generate GetReviews Query + Handler (paginated) | P0 | 10m | 2 | BE-06 |
| BE-12 | Generate GetReviewById Query + Handler | P0 | 5m | 2 | BE-06 |
| BE-13 | Generate DeleteReview Command + Handler | P0 | 5m | 2 | BE-06 |
| BE-14 | Generate GetSettings + UpdateSettings (MediatR) | P1 | 10m | 2 | BE-07 |
| BE-15 | Generate FluentValidation validators (CreateReviewValidator) | P0 | 5m | 2 | BE-04 |
| BE-16 | Generate Minimal API endpoint maps (ReviewEndpoints, SettingsEndpoints) | P0 | 10m | 2 | BE-10–BE-14 |
| BE-17 | Generate Program.cs (DI, CORS, Swagger, PostgreSQL, MediatR, middleware) | P0 | 10m | 1 | BE-01 |
| BE-18 | Generate GlobalExceptionHandler middleware (ProblemDetails) | P0 | 5m | 2 | BE-17 |
| BE-19 | Generate EF Core migrations | P0 | 5m | 2 | BE-05 |
| BE-20 | Generate ApiResponse\<T\> wrapper | P0 | 5m | 2 | BE-04 |
| BE-21 | Optional: OpenAiAnalysisService | P2 | 20m | 4 | BE-08 |
| BE-22 | Optional: GitHubApiService (fetch PR diff) | P2 | 15m | 4 | BE-08 |

**Backend total (P0+P1): ~2.5 hrs | P0 only: ~2 hrs**

---

## Frontend Tasks

| ID | Task | Priority | Est. | Phase | Depends On |
|----|------|----------|------|-------|------------|
| FE-01 | Create React + Vite + TypeScript project scaffold | P0 | 5m | 1 | — |
| FE-02 | Install dependencies (react-router-dom, axios) + configure | P0 | 5m | 1 | FE-01 |
| FE-03 | Configure ESLint + Prettier + strict TypeScript | P0 | 5m | 1 | FE-01 |
| FE-04 | Generate TypeScript models/interfaces (matching backend DTOs) | P0 | 10m | 3 | FE-01 |
| FE-05 | Generate API service layer (typed Axios wrapper with error normalization) | P0 | 10m | 3 | FE-04 |
| FE-06 | Generate MainLayout component (header, nav, footer, Outlet) | P0 | 10m | 3 | FE-01 |
| FE-07 | Generate AdminLayout component (sidebar, top bar, Outlet) | P0 | 10m | 3 | FE-01 |
| FE-08 | Generate router configuration with nested routes | P0 | 10m | 3 | FE-06, FE-07 |
| FE-09 | Generate common components: ErrorMessage, LoadingSpinner, SeverityBadge, ConfidenceBadge | P0 | 10m | 3 | FE-01 |
| FE-10 | Generate HomePage component | P0 | 10m | 3 | FE-08 |
| FE-11 | Generate ReviewPage (diff input, language select, submit) | P0 | 15m | 3 | FE-05, FE-08 |
| FE-12 | Generate review components: FindingCard, FindingsList, ReviewSummary, SuggestedFixPanel | P0 | 15m | 3 | FE-04, FE-09 |
| FE-13 | Generate ReviewResultPage (full results with filters) | P0 | 15m | 3 | FE-05, FE-12 |
| FE-14 | Generate HistoryPage (paginated list of past reviews) | P0 | 10m | 3 | FE-05, FE-08 |
| FE-15 | Generate AdminSettingsPage | P1 | 10m | 3 | FE-05, FE-08 |
| FE-16 | Generate custom hooks: useReviews, useReviewDetail, useSettings | P0 | 10m | 3 | FE-05 |
| FE-17 | Styling (CSS Modules or Tailwind) — all layouts and pages | P1 | 15m | 4 | All FE |
| FE-18 | Error boundaries on route level | P1 | 5m | 4 | FE-08 |

**Frontend total (P0+P1): ~3 hrs | P0 only: ~2.25 hrs**

---

## Infrastructure Tasks

| ID | Task | Priority | Est. | Phase | Depends On |
|----|------|----------|------|-------|------------|
| INF-01 | Generate docker-compose.yml (frontend, backend, PostgreSQL) | P0 | 10m | 5 | BE-17, FE-01 |
| INF-02 | Generate frontend Dockerfile (multi-stage: node → nginx) | P1 | 5m | 5 | FE-01 |
| INF-03 | Generate backend Dockerfile (multi-stage: SDK → runtime) | P1 | 5m | 5 | BE-01 |
| INF-04 | Generate .env.example | P0 | 5m | 5 | INF-01 |
| INF-05 | Generate .gitignore | P0 | 5m | 1 | — |
| INF-06 | Generate GitHub Actions CI workflow | P2 | 10m | 5 | INF-01 |

---

## Documentation Tasks

| ID | Task | Priority | Est. | Phase | Depends On |
|----|------|----------|------|-------|------------|
| DOC-01 | Generate README.md (overview, architecture, run instructions) | P0 | 10m | 5 | All |
| DOC-02 | Set up `prompts/` folder with `_prompt-log-template.md`; log each prompt as `{N}_{short-key}.md` during dev | P0 | 5m | 1 | — |
| DOC-03 | Log prompts during development (ongoing) | P0 | ongoing | All | DOC-02 |
| DOC-04 | Generate INSIGHTS.md after completion | P0 | 15m | 5 | All |
| DOC-05 | Copy .github/ folder from ai-common-project-structure/ and adapt for this project | P0 | 10m | 1 | — |

---

## Day-by-Day Execution Plan

### Day 1 (2 hours) — Backend + Frontend Foundation

| Time | Tasks | Focus |
|------|-------|-------|
| 0:00–0:05 | DOC-02, DOC-05, INF-05 | Templates, .github setup, .gitignore |
| 0:05–0:15 | BE-01, FE-01 | Project scaffolds |
| 0:15–0:20 | BE-17, FE-02, FE-03 | Program.cs skeleton + frontend config |
| 0:20–0:35 | BE-02, BE-03, BE-04 | Domain entities, enums, DTOs |
| 0:35–0:45 | BE-05, BE-19, BE-20 | DbContext, migrations, ApiResponse wrapper |
| 0:45–1:00 | BE-06, BE-07, BE-08 | Repositories + AI service interface |
| 1:00–1:15 | BE-09, BE-15 | Mock AI service + validators |
| 1:15–1:30 | BE-10, BE-11, BE-12, BE-13 | MediatR handlers |
| 1:30–1:45 | BE-16, BE-18 | Endpoints + exception middleware |
| 1:45–2:00 | FE-04, FE-05 | Frontend models + API service |

**Day 1 checkpoint:** Backend API fully functional, frontend models ready.

### Day 2 (2 hours) — Frontend + Integration + Docs

| Time | Tasks | Focus |
|------|-------|-------|
| 0:00–0:20 | FE-06, FE-07, FE-08 | Layouts + router |
| 0:20–0:30 | FE-09, FE-16 | Common components + hooks |
| 0:30–0:40 | FE-10 | HomePage |
| 0:40–0:55 | FE-11 | ReviewPage |
| 0:55–1:10 | FE-12, FE-13 | Review components + ResultPage |
| 1:10–1:20 | FE-14 | HistoryPage |
| 1:20–1:30 | FE-15, FE-17 | AdminSettings + styling |
| 1:30–1:40 | FE-18, INF-01, INF-04 | Error boundaries + Docker |
| 1:40–2:00 | DOC-01, DOC-04 | README + Insights |

**Day 2 checkpoint:** Full app working end-to-end, documentation complete.

---

## Task Dependencies Graph

```
Phase 1 (Foundation):
  BE-01 ─────┬──> BE-02 ──> BE-04 ──> BE-05 ──> BE-06 ──> BE-10, BE-11, BE-12, BE-13
              │              │                    └──> BE-07 ──> BE-14
              │              └──> BE-08 ──> BE-09 ──> BE-10
              └──> BE-17 ──> BE-18
  FE-01 ──> FE-02, FE-03 ──> FE-04 ──> FE-05

Phase 3 (Frontend):
  FE-05 + FE-06 + FE-07 ──> FE-08 ──> FE-10, FE-11, FE-13, FE-14, FE-15
  FE-09, FE-12 ──> used in pages

Phase 5 (Infrastructure):
  BE + FE ready ──> INF-01, INF-02, INF-03 ──> INF-06
  All ──> DOC-01, DOC-04
```

---

## Commit Strategy

Follow Conventional Commits format. Suggested commit sequence:

| Order | Commit | Scope |
|-------|--------|-------|
| 1 | `chore: scaffold solution and project structure` | BE-01, FE-01, INF-05 |
| 2 | `feat(backend): add domain entities and enums` | BE-02, BE-03 |
| 3 | `feat(backend): add DTOs and API response wrapper` | BE-04, BE-20 |
| 4 | `feat(backend): add DbContext and migrations` | BE-05, BE-19 |
| 5 | `feat(backend): add repositories` | BE-06, BE-07 |
| 6 | `feat(backend): add AI analysis service with mock` | BE-08, BE-09 |
| 7 | `feat(backend): add MediatR handlers` | BE-10–BE-14 |
| 8 | `feat(backend): add validators and endpoints` | BE-15, BE-16 |
| 9 | `feat(backend): add middleware and Program.cs config` | BE-17, BE-18 |
| 10 | `feat(frontend): add models and API service` | FE-04, FE-05 |
| 11 | `feat(frontend): add layouts and router` | FE-06, FE-07, FE-08 |
| 12 | `feat(frontend): add common components and hooks` | FE-09, FE-12, FE-16 |
| 13 | `feat(frontend): add pages` | FE-10–FE-15 |
| 14 | `style(frontend): add styling` | FE-17, FE-18 |
| 15 | `infra: add Docker setup` | INF-01–INF-04 |
| 16 | `docs: add README, prompt log, and insights` | DOC-01–DOC-05 |

---

## Testing Strategy

### Backend (xUnit)
- Unit tests for MediatR handlers (CreateReview, GetReviews)
- Unit test for MockAiAnalysisService (returns valid findings)
- Unit tests for FluentValidation validators
- Integration test: POST /api/v1/reviews → 200 with findings

### Frontend (Jest + RTL)
- Component render tests for FindingCard, SeverityBadge
- Page smoke tests (renders without crash)
- Hook test for useReviews (mocked API)

### Priority
- Focus on critical paths only (as per ai-workflow.md rules)
- Do not over-test — tests that verify AI generation patterns are most valuable

---

## Priority Legend

| Priority | Meaning | Include in MVP? |
|----------|---------|----------------|
| P0 | Must have — core functionality | Yes |
| P1 | Should have — important polish | Yes if time allows |
| P2 | Nice to have — stretch goals | No |
