# Implementation Plan — PR Review Assistant

> **Generated:** 2026-03-04  
> **Project:** AI-ED-FTG16 — PR Review Assistant  
> **Stack:** React 18 + Vite + TypeScript · .NET 9 Minimal API · PostgreSQL 16 · Docker Compose  
> **Total Effort:** ~4 hours (2 sessions × 2 hours)

---

## 1. Readiness Assessment

All documentation in `.github/`, `docs/`, `project-requarements/`, and `prompts/` is complete and ready for implementation. The following minor issues were found:

| # | File | Issue | Fix |
|---|------|-------|-----|
| 1 | `docs/ai-workflow.md` | Duplicate `## Section 8` heading ("Code Generation Order" and "Definition of Done") | Rename second to `## Section 11 — Definition of Done` |
| 2 | `project-requarements/2-functional-requirements-srs.md` | NFR-AI-02 updated: prompts are documented using `{N}_{short-key}.md` per-file naming in `prompts/` folder | ✓ Fixed |
| 3 | `project-requarements/3-backlog-tasks.md` | DOC-02 updated: references `prompts/_prompt-log-template.md` and `{N}_{short-key}.md` per-file naming | ✓ Fixed |
| 4 | `project-requarements/1-project-overview.md` | Lists only 3 `.github/prompts/` files (6 exist) | Update list to match actual count |
| 5 | `project-requarements/1-project-overview.md` | docker-compose.yml shown in `infra/` | Correct to repo root |
| 6 | Repo root | `.gitignore` missing | Created in Prompt 6 (Phase 1) |
| 7 | All `.github/prompts/*.prompt.md` | `applyTo` not set in all files | Verify on use — low priority |
| 8 | `project-requarements/` | Folder name `requarements` misspelled | Keep as-is for consistency with existing references |

**Decision — Prompt logging:** Per-file in `prompts/` (active Copilot instructions win over SRS)  
**Decision — docker-compose.yml:** Repo root (standard convention)  
**Decision — CSS approach:** CSS Modules (lighter than Tailwind for MVP)

---

## 2. Implementation Milestones

### Day 1 (~2 hours)

| Milestone | Time | Tasks | Deliverable |
|-----------|------|-------|-------------|
| 1 — Foundation | 0:00–0:20 | Prompt 6 (.gitignore), Prompt 7 (BE scaffold), Prompt 8 (FE scaffold), Prompt 9 (Program.cs skeleton) | Repository builds; health endpoint responds |
| 2 — Backend Contracts | 0:20–0:45 | Prompts 10–13 (entities, enums, DTOs, interfaces) | All domain types defined; no EF/framework deps |
| 3 — Backend Infrastructure | 0:45–1:05 | Prompts 14–17 (DbContext, repositories, MockAI, migration) | Database schema created; mock AI returns findings |
| 4 — Backend Logic | 1:05–1:45 | Prompts 18–26 (validators, 5 handlers, endpoints, error handling, Program.cs final) | All 6 API endpoints respond correctly |
| 5 — Frontend Foundation | 1:45–2:00 | Prompts 27–28 (TypeScript models, API service) | Frontend compiles; API calls typed end-to-end |

### Day 2 (~2 hours)

| Milestone | Time | Tasks | Deliverable |
|-----------|------|-------|-------------|
| 6 — Layouts & Routing | 0:00–0:20 | Prompts 29–32 (MainLayout, AdminLayout, router, common components) | All routes render; layouts complete |
| 7 — Hooks & Components | 0:20–0:40 | Prompts 33–34 (custom hooks, review components) | Live data flows end-to-end |
| 8 — Pages | 0:40–1:20 | Prompts 35–39 (all 5 pages) | Full user journey: submit → view → history |
| 9 — Polish | 1:20–1:40 | Prompts 40–41 (styling, error boundaries) | Consistent design; crashes handled |
| 10 — Infra & Docs | 1:40–2:00 | Prompts 42–50 (Docker, CI, tests, README, INSIGHTS, validation) | Builds in Docker; CI would pass |

---

## 3. Backlog (51 Tasks)

### Backend — 22 Tasks

| ID | Task | Depends On | Effort | Priority |
|----|------|-----------|--------|----------|
| BE-01 | Scaffold .NET 9 solution + projects + NuGet | — | 15 min | P0 |
| BE-02 | Domain entities: Review, Finding, AppSettings | BE-01 | 10 min | P0 |
| BE-03 | Enums: FindingCategory, FindingSeverity | BE-01 | 5 min | P0 |
| BE-04 | DTO records: all request/response types + ApiResponse\<T\> | BE-02, BE-03 | 15 min | P0 |
| BE-05 | Repository + AI service interfaces | BE-02, BE-04 | 10 min | P0 |
| BE-06 | AppDbContext + Fluent API entity configurations | BE-02 | 15 min | P0 |
| BE-07 | EF Core migrations (InitialCreate) | BE-06 | 5 min | P0 |
| BE-08 | ReviewRepository + SettingsRepository implementations | BE-05, BE-06 | 20 min | P0 |
| BE-09 | MockAiAnalysisService (no external API) | BE-05 | 15 min | P0 |
| BE-10 | FluentValidation: CreateReviewRequestValidator | BE-04 | 10 min | P0 |
| BE-11 | FluentValidation: UpdateSettingsRequestValidator | BE-04 | 5 min | P0 |
| BE-12 | CreateReview Command + Handler | BE-05, BE-08, BE-09 | 20 min | P0 |
| BE-13 | GetReviews Query + Handler | BE-05, BE-08 | 10 min | P0 |
| BE-14 | GetReviewById Query + Handler | BE-05, BE-08 | 10 min | P0 |
| BE-15 | DeleteReview Command + Handler | BE-05, BE-08 | 5 min | P0 |
| BE-16 | GetSettings + UpdateSettings Handlers | BE-05, BE-08 | 15 min | P0 |
| BE-17 | Program.cs final: DI, middleware, endpoint registration | BE-06…BE-16 | 15 min | P0 |
| BE-18 | Minimal API endpoint maps (ReviewEndpoints, SettingsEndpoints) | BE-12…BE-16 | 10 min | P0 |
| BE-19 | GlobalExceptionHandler middleware | BE-17 | 10 min | P0 |
| BE-20 | Program.cs skeleton (startup, CORS, Swagger, health) | BE-01 | 10 min | P0 |
| BE-21 | Backend unit tests (handlers, validators, MockAI) | BE-12…BE-19 | 30 min | P1 |
| BE-22 | OpenAiAnalysisService (optional, behind feature flag) | BE-09 | 30 min | P3 |

**Acceptance Criteria (key tasks):**
- **BE-12:** POST `/api/v1/reviews` with valid diff returns `201` with `ReviewDetailResponse` containing ≥3 findings
- **BE-13:** GET `/api/v1/reviews?page=1&pageSize=20` returns `PaginatedList<ReviewListItem>` with `totalCount` field
- **BE-14:** GET `/api/v1/reviews/{id}` with invalid ID returns `404` with `ProblemDetails`
- **BE-17:** `dotnet build` passes with 0 errors; `/health` returns `200`
- **BE-19:** Unhandled exception returns `500` ProblemDetails (no stack trace)

---

### Frontend — 18 Tasks

| ID | Task | Depends On | Effort | Priority |
|----|------|-----------|--------|----------|
| FE-01 | Vite React TS scaffold | — | 10 min | P0 |
| FE-02 | tsconfig.json (strict mode, noUncheckedIndexedAccess) | FE-01 | 5 min | P0 |
| FE-03 | ESLint + Prettier configuration | FE-01 | 5 min | P0 |
| FE-04 | TypeScript models (all interfaces matching backend DTOs) | BE-04 | 10 min | P0 |
| FE-05 | Axios API service layer | FE-04 | 15 min | P0 |
| FE-06 | MainLayout component | FE-01 | 10 min | P0 |
| FE-07 | AdminLayout component | FE-01 | 10 min | P0 |
| FE-08 | React Router v6 configuration | FE-06, FE-07 | 10 min | P0 |
| FE-09 | Common components: ErrorMessage, LoadingSpinner, SeverityBadge, ConfidenceBadge | FE-04 | 20 min | P0 |
| FE-10 | Custom hooks: useReviews, useReviewDetail, useSettings | FE-05 | 20 min | P0 |
| FE-11 | Review components: FindingCard, FindingsList, ReviewSummary, SuggestedFixPanel | FE-04, FE-09 | 30 min | P0 |
| FE-12 | HomePage | FE-06 | 10 min | P0 |
| FE-13 | ReviewPage (form, validation, submit) | FE-05, FE-06, FE-09 | 20 min | P0 |
| FE-14 | ReviewResultPage (filters, findings list) | FE-10, FE-11 | 20 min | P0 |
| FE-15 | HistoryPage (paginated list, delete) | FE-07, FE-10 | 20 min | P0 |
| FE-16 | AdminSettingsPage | FE-07, FE-10 | 15 min | P1 |
| FE-17 | Global CSS styling + component CSS modules | FE-12…FE-16 | 20 min | P1 |
| FE-18 | ErrorBoundary component + router integration | FE-08 | 10 min | P1 |

**Acceptance Criteria (key tasks):**
- **FE-13:** Submitting empty diff shows inline error; valid submission navigates to result page
- **FE-14:** Severity/category filters reduce displayed findings count; `FindingCard` shows `SuggestedFixPanel` toggle
- **FE-15:** Delete with confirmation reloads list; pagination controls work; code snippet truncated at 80 chars
- **FE-18:** Throw in any page renders error boundary instead of white screen; "Go Home" resets state

---

### Infrastructure — 6 Tasks

| ID | Task | Depends On | Effort | Priority |
|----|------|-----------|--------|----------|
| INF-01 | Frontend Dockerfile (multi-stage: node → nginx) | FE complete | 10 min | P0 |
| INF-02 | Backend Dockerfile (multi-stage: SDK → runtime, tests in build) | BE complete | 10 min | P0 |
| INF-03 | docker-compose.yml (3 services: frontend, backend, db) | INF-01, INF-02 | 15 min | P0 |
| INF-04 | GitHub Actions CI workflow (3 jobs: frontend, backend, docker) | INF-03 | 20 min | P1 |
| INF-05 | .gitignore (repo root) | — | 5 min | P0 |
| INF-06 | nginx.conf for frontend container | INF-01 | 10 min | P1 |

**Acceptance Criteria:**
- **INF-03:** `docker compose up -d` starts all 3 services; `curl localhost:8080/health` returns `{"status":"healthy"}`; `curl localhost:3000` returns HTML
- **INF-04:** CI passes on clean branch: lint, build, test, Docker all green

---

### Documentation — 5 Tasks

| ID | Task | Depends On | Effort | Priority |
|----|------|-----------|--------|----------|
| DOC-01 | README.md (full project doc, Docker instructions, API table) | Implementation complete | 15 min | P0 |
| DOC-02 | Prompt logs: verify all N prompts logged in `prompts/` folder | Each prompt execution | Ongoing | P0 |
| DOC-03 | `docs/INSIGHTS.md` (AI development lessons learned) | Implementation complete | 15 min | P1 |
| DOC-04 | Fix 8 documentation issues identified in readiness assessment | — | 10 min | P1 |
| DOC-05 | Update backlog tracking table with final task status | Implementation complete | 5 min | P2 |

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
| 47 | FE tests | Frontend component tests |
| 48 | DOC-01 | `README.md` |
| 49 | DOC-03 | `INSIGHTS.md` |
| 50 | All | Final validation + cleanup |

---

## 5. Scaffolding Commands (Windows PowerShell)

```powershell
# Backend scaffold (run once)
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant
md backend\src\PRReviewAssistant.API, backend\tests\PRReviewAssistant.Tests

cd backend
dotnet new sln -n PRReviewAssistant
dotnet new webapi -n PRReviewAssistant.API -o src/PRReviewAssistant.API --use-minimal-apis
dotnet new xunit -n PRReviewAssistant.Tests -o tests/PRReviewAssistant.Tests
dotnet sln add src/PRReviewAssistant.API/PRReviewAssistant.API.csproj
dotnet sln add tests/PRReviewAssistant.Tests/PRReviewAssistant.Tests.csproj
dotnet add tests/PRReviewAssistant.Tests reference src/PRReviewAssistant.API

cd src/PRReviewAssistant.API
dotnet add package MediatR
dotnet add package FluentValidation.AspNetCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Swashbuckle.AspNetCore
dotnet add package NUlid
dotnet add package Microsoft.EntityFrameworkCore.Design

cd ../../tests/PRReviewAssistant.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory

# Frontend scaffold (run once)
cd E:\Dev\GodelAI\Projects\ai-pr-review-assistant
npm create vite@latest frontend -- --template react-ts
cd frontend
npm install
npm install react-router-dom axios
npm install -D @testing-library/react @testing-library/jest-dom @testing-library/user-event
npm install -D jest ts-jest @types/jest jest-environment-jsdom
npm install -D eslint @typescript-eslint/eslint-plugin @typescript-eslint/parser
npm install -D prettier
```

---

## 6. GitHub Actions Auto-Update Workflow

```yaml
# .github/workflows/update-readme.yml
name: Update README Stats

on:
  push:
    branches: [ main ]
    paths:
      - 'prompts/**'

jobs:
  update-readme:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
      - name: Count prompts
        run: |
          PROMPT_COUNT=$(ls prompts/*.md | grep -v template | wc -l)
          echo "PROMPT_COUNT=$PROMPT_COUNT" >> $GITHUB_ENV
      - name: Update badge in README
        run: |
          sed -i "s/Prompts: [0-9]*/Prompts: $PROMPT_COUNT/" README.md
      - name: Commit if changed
        run: |
          git config user.name "github-actions[bot]"
          git config user.email "github-actions[bot]@users.noreply.github.com"
          git diff --quiet || (git add README.md && git commit -m "chore(docs): update prompt count" && git push)
```

---

## 7. MCP GitHub Setup

To connect GitHub Copilot to the project's GitHub repository:

1. In VS Code, open the Copilot chat sidebar
2. Click **+** → **Add Context** → **GitHub**
3. Authenticate with GitHub OAuth if prompted
4. Select repository `ai-pr-review-assistant`
5. Optional: Install the `GitHub MCP Server` extension for file-level PR context
6. Verify: Type `@github` in chat — should show repository context options

For local MCP server integration (advanced):
```json
// .vscode/settings.json
{
  "github.copilot.chat.mcp.enabled": true,
  "github.copilot.chat.mcp.server": {
    "github": { "command": "npx", "args": ["@modelcontextprotocol/server-github"] }
  }
}
```

---

## 8. Git Workflow

### Branch Strategy

| Phase | Branch Name | Merge When |
|-------|------------|------------|
| Foundation | `feat/FTG16-phase1-foundation` | Health endpoint responds |
| Backend Contracts | `feat/FTG16-phase2-backend-contracts` | All entities + DTOs compile |
| Backend Infrastructure | `feat/FTG16-phase3-backend-infra` | Migration runs, mock AI works |
| Backend Logic | `feat/FTG16-phase4-backend-logic` | All 6 API endpoints correct |
| Frontend Foundation | `feat/FTG16-phase5-frontend-foundation` | Models + API service compile |
| Frontend Layouts | `feat/FTG16-phase6-layouts-routing` | All routes render |
| Frontend Hooks & Components | `feat/FTG16-phase7-hooks-components` | Live data end-to-end |
| Frontend Pages | `feat/FTG16-phase8-pages` | Full user journey works |
| Polish | `feat/FTG16-phase9-polish` | No visual regressions |
| Infra & Docs | `feat/FTG16-phase10-infra-docs` | Docker + CI green |

### Workflow Commands

```bash
# Start each phase
git checkout main && git pull
git checkout -b feat/FTG16-phase{N}-{name}

# Commit during phase
git add -A
git commit -m "feat(backend): add review entity and finding entity

Implements domain model per SRS §5.1"

# Merge phase
git checkout main
git merge --squash feat/FTG16-phase{N}-{name}
git commit -m "feat(phase{N}): implement {name}"
git push origin main
git branch -d feat/FTG16-phase{N}-{name}
git push origin --delete feat/FTG16-phase{N}-{name}
```

---

## 9. Definition of Done

A task is complete only when all apply:

- [ ] Code compiles without errors or warnings
- [ ] Tests pass (existing + new)
- [ ] No `console.log`, `debugger`, `TODO`, or unused imports
- [ ] File is in the correct location per folder structure in SRS
- [ ] Naming follows conventions in `.github/copilot-instructions.md`
- [ ] Prompt log saved as `{N}_{key}.md` in `prompts/`
- [ ] Committed with Conventional Commits message

The project is fully done when:

- [ ] `docker compose up -d` starts all 3 services successfully
- [ ] All 6 API endpoints respond correctly (manual check with curl)
- [ ] Full user journey works: submit diff → view findings → history → delete
- [ ] GitHub Actions CI would pass (frontend lint + build + test, backend build + test, Docker build)
- [ ] README.md accurate and complete
- [ ] All 45+ prompt logs exist in `prompts/`
