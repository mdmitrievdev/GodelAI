# Copilot Instructions — AI-ED-FTG16

**Stack: React (Vite + TypeScript) + .NET 9 + Docker + GitHub Actions**

> Full workflow reference: [`docs/ai-workflow.md`](../docs/ai-workflow.md)

---

## Project Context

### Application: PR Review Assistant

An AI-powered code review assistant that analyzes code diffs (pasted text or fetched from a GitHub PR URL) and returns structured, categorized findings — built with 90%+ AI-generated code using GitHub Copilot.

**Stack:** React 18 + Vite + TypeScript (strict) | .NET 9 Minimal API | MediatR + FluentValidation | EF Core + PostgreSQL 16 | Docker Compose

---

### Domain Entities

#### Review

| Field | Type | Notes |
|-------|------|-------|
| Id | string (ULID) | PK |
| CodeDiff | string | Required, max 50 000 chars |
| Language | string | Required |
| PrUrl | string? | Nullable |
| CreatedAt | DateTime | UTC |
| Findings | List\<Finding\> | Navigation property |

#### Finding

| Field | Type | Notes |
|-------|------|-------|
| Id | string (ULID) | PK |
| ReviewId | string | FK → Review |
| Category | FindingCategory | Enum |
| Severity | FindingSeverity | Enum |
| Title | string | Required, max 200 chars |
| Description | string | Required, max 2000 chars |
| LineReference | string? | Nullable — line number or range |
| Suggestion | string | Required, max 2000 chars |
| Confidence | int | 0–100 (AI confidence %) |
| SuggestedFix | string? | Nullable — inline code suggestion, max 5000 chars |

#### AppSettings (singleton)

| Field | Type | Default |
|-------|------|---------|
| Id | string | PK |
| UseMockAi | bool | true |
| AiModel | string | "mock" |

---

### Enums

```
FindingCategory : Bug | Naming | Performance | Security | CodeStyle | BestPractice
FindingSeverity : Critical | Warning | Info
```

UI severity colors: Critical = red, Warning = orange, Info = blue.

---

### DTOs (C# Records)

```csharp
// Requests
record CreateReviewRequest(string CodeDiff, string Language, string? PrUrl);
record UpdateSettingsRequest(bool UseMockAi, string AiModel);

// Responses
record ReviewDetailResponse(
    string Id, DateTime CreatedAt, string Language, string CodeDiff,
    string? PrUrl, ReviewSummaryDto Summary, IReadOnlyList<FindingDto> Findings);

record ReviewListItem(
    string Id, DateTime CreatedAt, string Language, string CodeSnippet,
    int TotalFindings, int CriticalCount, int WarningCount, int InfoCount);

record FindingDto(
    string Id, string Category, string Severity, string Title,
    string Description, string? LineReference, string Suggestion,
    int Confidence, string? SuggestedFix);

record ReviewSummaryDto(
    int TotalFindings, int CriticalCount, int WarningCount,
    int InfoCount, double AverageConfidence);

record AppSettingsResponse(bool UseMockAi, string AiModel);
record PaginatedList<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
```

---

### TypeScript Interfaces

```typescript
interface ReviewDetailResponse {
  id: string;
  createdAt: string;
  language: string;
  codeDiff: string;
  prUrl: string | null;
  summary: ReviewSummary;
  findings: Finding[];
}

interface ReviewListItem {
  id: string;
  createdAt: string;
  language: string;
  codeSnippet: string;
  totalFindings: number;
  criticalCount: number;
  warningCount: number;
  infoCount: number;
}

interface Finding {
  id: string;
  category: FindingCategory;
  severity: FindingSeverity;
  title: string;
  description: string;
  lineReference: string | null;
  suggestion: string;
  confidence: number;
  suggestedFix: string | null;
}

interface ReviewSummary {
  totalFindings: number;
  criticalCount: number;
  warningCount: number;
  infoCount: number;
  averageConfidence: number;
}

interface AppSettingsResponse { useMockAi: boolean; aiModel: string; }
interface PaginatedList<T> { items: T[]; totalCount: number; page: number; pageSize: number; }

type FindingCategory = 'Bug' | 'Naming' | 'Performance' | 'Security' | 'CodeStyle' | 'BestPractice';
type FindingSeverity = 'Critical' | 'Warning' | 'Info';
```

---

### AI Analysis Service

```csharp
interface IAiAnalysisService
{
    Task<IReadOnlyList<Finding>> AnalyzeAsync(
        string codeDiff, string language, CancellationToken ct);
}
```

- Default: `MockAiAnalysisService` — no external API required, returns realistic varied findings.
- Optional: `OpenAiAnalysisService` (requires `OPENAI_API_KEY` env var).
- Supported languages: `C#`, `TypeScript`, `JavaScript`, `Python`, `Java`, `Go`, `Other`.

---

### Pages & Routes

| Page | Route | Layout | Description |
|------|-------|--------|-------------|
| HomePage | `/` | MainLayout | App overview + "Start Review" CTA |
| ReviewPage | `/review` | MainLayout | Paste diff or enter PR URL, select language, submit |
| ReviewResultPage | `/review/:id` | MainLayout | AI findings grouped by category, filterable by severity/category |
| HistoryPage | `/admin/history` | AdminLayout | Paginated list of past reviews; click to view, delete |
| AdminSettingsPage | `/admin/settings` | AdminLayout | Toggle mock AI, view stats |

Unknown routes → redirect to `/`.

---

### Layouts

**MainLayout:** Header (logo + nav: Home, New Review) → `<Outlet />` → Footer  
**AdminLayout:** Sidebar (History, Settings) + Top bar ("Admin" badge + back-to-main link) → `<Outlet />`

---

### API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/v1/reviews` | Submit code diff for AI review |
| GET | `/api/v1/reviews` | List reviews — query: `?page=1&pageSize=20` |
| GET | `/api/v1/reviews/{id}` | Get review with all findings |
| DELETE | `/api/v1/reviews/{id}` | Delete review (cascade deletes findings) |
| GET | `/api/v1/settings` | Get app settings |
| PUT | `/api/v1/settings` | Update app settings |

---

## Operating Modes

| Mode  | Trigger          | Output                         |
|-------|------------------|--------------------------------|
| Chat  | Question asked   | Direct answer only — no code   |
| Plan  | "Plan: ..."      | Step-by-step plan — no code    |
| Agent | "Implement: ..." | Code + tests + docs + summary  |

---

## Global Rules

- One task = one chat. Do not mix tasks.
- **Plan must be approved before any implementation.**
- Never assume missing requirements — ask first.
- If a feature already exists → update docs/tests only. Do NOT reimplement.
- Never duplicate logic — refactor instead.
- Keep context minimal. Do not load unrelated files.
- No `any` in TypeScript. Strict mode only.
- Backend validation via FluentValidation only.
- No secrets, credentials, or debug code in commits.
- After every prompt that produces code: save as `{N}_{key}.md` in `prompts/` — see `.github/instructions/prompt-logging.instructions.md`.

### Escalation Rule

If AI cannot produce correct code after **2 attempts**:
1. Simplify the prompt — break into smaller pieces.
2. Provide more context — reference existing interfaces and files.
3. If still failing → manual fix with:

```
// MANUAL EDIT: AI failed after 2 attempts — <description>
```

### Manual Edit Marking

Any manual change to an AI-generated file must be annotated:

```typescript
// MANUAL EDIT: <reason>
```

```csharp
// MANUAL EDIT: <reason>
```

---

## Communication Style

- Be concise. Answer only what was asked. Do not pad responses.
- In Chat mode: plain prose, no code blocks unless a snippet is essential.
- In Plan mode: numbered steps, no code, no implementation details.
- In Agent mode: code first, brief explanation after. No motivational language.
- Never say "Great question", "Certainly", "Of course", or similar filler phrases.
- Never summarize what you are about to do — just do it.
- Use markdown tables and bullet lists for structured output; avoid walls of prose.
- When something is unclear, ask one targeted question. Do not list multiple hypotheticals.
- If a request is ambiguous, state the assumption made and proceed.

---

## Repository Structure

```
frontend/       React + Vite + TypeScript (strict)
                Jest + React Testing Library
                ESLint + Prettier

backend/        .NET 9 Minimal API
                MediatR + FluentValidation
                xUnit

infra/          Docker Compose + PostgreSQL
.github/        CI (GitHub Actions)
docs/           Project documentation
```

If the actual structure differs → **stop and ask before proceeding**.

---

## Code Conventions

### TypeScript / React

**Naming**
- Components: `PascalCase` — file and export must match (`UserCard.tsx` → `export function UserCard`)
- Hooks: `camelCase` prefixed with `use` (`useAuthState`, `useProductList`)
- Utilities / helpers: `camelCase` (`formatDate`, `parseApiError`)
- Constants: `UPPER_SNAKE_CASE` (`MAX_RETRY_COUNT`, `API_BASE_URL`)
- Types and interfaces: `PascalCase`, no `I` prefix (`User`, `ProductDto`, `ApiResponse<T>`)
- Test files: co-located, suffix `.test.tsx` or `.test.ts`

**TypeScript**
- Strict mode always. No `any`. No `as unknown as X`.
- Prefer `type` for unions/intersections; use `interface` for object shapes that may be extended.
- Explicit return types on all exported functions and hooks.
- Avoid non-null assertion (`!`) — handle null/undefined explicitly.
- Use `unknown` instead of `any` when type is genuinely unknown, then narrow.

**React**
- Functional components only. No class components.
- Props interface defined immediately above the component.
- Co-locate state logic in a custom hook when it exceeds ~15 lines.
- Use `React.memo` only with a measurable performance reason.
- Error boundaries must wrap every top-level route/page.
- Never fetch data directly in a component — use a hook or a data layer.

**Error Handling (Frontend)**
- API errors: normalize to `{ message: string; code?: string }` at the API client boundary.
- User-visible errors: display via a shared `<ErrorMessage>` component, never raw `alert()`.
- Unexpected errors: caught by the nearest error boundary, logged via structured logger.
- Never silently swallow errors with empty `catch {}` blocks.

**Logging (Frontend)**
- Use a structured logger wrapper (e.g. wrapping `console`) — never raw `console.log` in production paths.
- Log levels: `debug` (dev only), `info`, `warn`, `error`.
- Remove all `console.log` debug calls before committing.

---

### C# / .NET

**Naming**
- Classes, methods, properties: `PascalCase`
- Private fields: `_camelCase` (underscore prefix)
- Local variables and parameters: `camelCase`
- Async methods: suffix `Async` (`GetUserAsync`, `CreateOrderAsync`)
- Interfaces: `IPascalCase` (`IUserRepository`, `IOrderService`)
- Test classes: `{Subject}Tests` (`UserServiceTests`)
- Test methods: `{Method}_{Scenario}_{ExpectedResult}` (`CreateOrder_WithInvalidAmount_ReturnsBadRequest`)

**Architecture**
- Follow MediatR strictly: every use case is a `Command` or `Query` + `Handler`. No business logic in controllers/endpoints.
- Minimal API endpoint maps directly to a `Send()` call — nothing more.
- No business logic in validators — validation only (shape, range, required).
- Repository pattern for data access; no raw EF queries outside repositories.

**Error Handling (Backend)**
- Return `ProblemDetails` (RFC 7807) for all error responses.
- Use a global exception handler middleware — do not try/catch in handlers unless required for compensating logic.
- Validation failures → `400 Bad Request` with field-level `ProblemDetails`.
- Not found → `404 Not Found`. Unauthorized → `401`. Forbidden → `403`.
- Never expose stack traces or internal messages to the client.

**Logging (Backend)**
- Use `ILogger<T>` with structured logging (Serilog or Microsoft.Extensions.Logging).
- Log levels: `Debug` (dev), `Information` (key operations), `Warning` (recoverable issues), `Error` (failures).
- Include correlation IDs in all log entries for request tracing.
- Never log sensitive data (passwords, tokens, PII).

**C# Style**
- Use `record` for immutable DTOs and value objects.
- Use `required` modifier on mandatory properties instead of constructor enforcement where appropriate.
- Prefer `IReadOnlyList<T>` over `List<T>` for return types.
- Use `CancellationToken` in all async methods down the call chain.
- No `static` classes for business logic — use DI.

---

### API Contracts

- All API responses are wrapped: `{ data: T | null, error: ProblemDetails | null }`
- Timestamps: ISO 8601 UTC (`2026-03-02T14:30:00Z`)
- IDs: `string` (ULID or UUID) — never expose raw integer database IDs.
- Pagination: `{ items: T[], totalCount: number, page: number, pageSize: number }`
- Field names in JSON: `camelCase`
- HTTP methods: GET (read), POST (create), PUT (full replace), PATCH (partial update), DELETE
- API versioning: URL prefix `/api/v1/`

---

## Deterministic Task Workflow

Every implementation task follows these steps in order. **No skipping.**

### 1. Context Analysis
- Read issue or requirement.
- Locate related files.
- Check if feature already exists.

### 2. Implementation Plan (No Code)
Output a plan covering:
- Files to create/modify
- API changes
- Data flow impact
- Validation strategy
- Test strategy
- Risk areas

**STOP. Wait for explicit approval.**

### 3. Implementation (After Approval)
- Small logical commits.
- No duplication — refactor existing code.
- Tests for critical paths only.
- Changes minimal and reviewable.

### 4. Mandatory Deliverables
Every task must produce:
1. Working implementation
2. Passing unit tests
3. Updated `README.md` — see Step 6 for required content
4. Structured summary (Changes / Tests / Breaking Changes)

### 5. Self-Validation (Before Finalizing)
- Frontend builds
- Backend builds
- All tests pass
- No lint errors
- No unused code or debug leftovers

**Fix failures before providing summary.**

### 6. Post-Completion Bookkeeping (Mandatory — No Skipping)

Perform both actions after every successfully completed task, in this order:

#### 6a. Mark the task as done in the plan file

Open `.github/plan-pr-review-assistant.md` and, for every acceptance-criteria checkbox belonging to the completed task, change `[ ]` to `[x]`.

Rules:
- Only mark checkboxes for tasks whose **all acceptance criteria** are met.
- Do not modify any other content in the file (task descriptions, estimates, etc.).
- If a task spans multiple prompts, mark only the checkboxes confirmed by the current prompt.

#### 6b. Update `README.md`

Update the project's `README.md` to reflect the current state of the codebase after the completed task. The README must always contain:

**Business section** (`## What This App Does`)
- One-paragraph plain-language description of the application's current user-facing capabilities.
- List of features that are fully working at this point.

**Technical section** (`## Technical Overview`)
- Current stack with versions.
- Folder structure (high level).
- How to run locally (commands only — no prose filler).
- Environment variables required (reference `.env.example`).

Rules:
- Keep the README accurate — remove references to features not yet built.
- Do not pad with aspirational future features; document only what exists.
- Append a `## Changelog` entry: `- [date] [task-id] <one-line description>`.

---

## Git Workflow

### Branch Naming

```
<type>/<ticket-id>-<short-description>
```

| Type        | When to use                                 |
|-------------|---------------------------------------------|
| `feat`      | New feature                                 |
| `fix`       | Bug fix                                     |
| `refactor`  | Code change with no behavior change         |
| `chore`     | Tooling, dependencies, config               |
| `docs`      | Documentation only                          |
| `test`      | Adding or fixing tests only                 |
| `ci`        | CI/CD pipeline changes                      |
| `hotfix`    | Urgent production fix                       |

**Examples:**
```
feat/FTG-42-user-authentication
fix/FTG-87-cart-total-rounding
refactor/FTG-103-extract-payment-service
chore/FTG-55-upgrade-dotnet-9
```

Rules:
- Always branch from `main` (or `develop` if gitflow is in use).
- One branch per ticket. Never reuse branches.
- Delete branch after merge.

---

### Commit Messages — Conventional Commits

Format:
```
<type>(<scope>): <short summary>

[optional body]

[optional footer: BREAKING CHANGE or Closes #id]
```

**Types:** `feat`, `fix`, `refactor`, `test`, `docs`, `chore`, `ci`, `perf`, `style`

**Scope:** module, layer, or component affected (`auth`, `cart`, `frontend`, `backend`, `docker`, `ci`)

**Rules:**
- Summary: imperative mood, lowercase, no period, max 72 characters.
- Body: explain *why*, not *what*. Wrap at 72 characters.
- Breaking changes: add `BREAKING CHANGE:` footer with migration notes.
- Reference tickets: `Closes #42` or `Refs #42` in footer.

**Examples:**
```
feat(auth): add JWT refresh token rotation

Tokens now rotate on every use to limit the validity window
of stolen refresh tokens.

Closes #42
```

```
fix(cart): correct rounding error in total price calculation

JavaScript floating point caused $0.01 discrepancies on
certain quantity/price combinations. Now uses decimal math.

Closes #87
```

```
refactor(backend): extract payment logic into PaymentService

BUSINESS LOGIC WAS SPREAD ACROSS 3 HANDLERS — NOW IN ONE SERVICE.

BREAKING CHANGE: OrderHandler no longer calls Stripe directly.
Update IPaymentService registration in DI container.
```

---

### Merge Strategy

- PRs merge via **squash merge** into `main` — keeps history clean.
- Squash commit message must follow Conventional Commits format.
- Never commit directly to `main` or `develop`.
- Rebase feature branches on `main` before opening a PR (no merge commits in PRs).
- Delete remote branch immediately after merge.

---

## Context & Cost Control

- Use stronger models (GPT-4o, Claude Sonnet) for planning.
- Use lightweight models for small edits and formatting.
- Store durable knowledge in `/docs/*.md` — not in chat.
- Avoid loading unrelated files into context.

---

## Security Rules

- Never expose secrets or credentials.
- Validate all dependencies before adding them.
- Always manually review AI-generated code.
- No external data leaks.

---

## Definition of Done

A task is **complete only** when all apply:

- [ ] Code works correctly
- [ ] Tests pass
- [ ] Documentation updated
- [ ] CI would pass
- [ ] Docker setup works (if applicable)
- [ ] Structured summary provided

---

## Reusable Prompts

Use these files as starting points for common tasks:

| Task | Prompt File |
|------|-------------|
| Implement feature | `.github/prompts/implement.prompt.md` |
| Dockerize app | `.github/prompts/dockerize.prompt.md` |
| Set up CI | `.github/prompts/setup-ci.prompt.md` |
| Generate backend entity | `.github/prompts/generate-entity.prompt.md` |
| Generate React component / page / hook | `.github/prompts/generate-component.prompt.md` |
| Generate MediatR handler / validator / endpoint | `.github/prompts/generate-handler.prompt.md` |
