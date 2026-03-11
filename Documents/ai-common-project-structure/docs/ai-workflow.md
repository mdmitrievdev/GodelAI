# AI-ED-FTG16 Deterministic Development Workflow

**Stack: React (Vite + TypeScript) + .NET 9 + Docker + GitHub Actions**

> Full reference document.  
> For Copilot behavioral rules see [`.github/copilot-instructions.md`](../.github/copilot-instructions.md).  
> For reusable prompts see [`.github/prompts/`](../.github/prompts/).  
> For PR checklist see [`.github/pull_request_template.md`](../.github/pull_request_template.md).

---

## Purpose

Convert vague requirements into production-ready code using a structured, plan-first, AI-assisted workflow optimized for GitHub Copilot Agents.

This standard enforces **deterministic execution**, minimal context usage, test discipline, and reproducible builds.

---

## Section 0 — Operating Modes

| Mode  | Purpose                      | Output                            |
|-------|------------------------------|-----------------------------------|
| Chat  | Clarification & exploration  | Direct answer only                |
| Plan  | Implementation strategy      | Step-by-step plan (no code)       |
| Agent | Execute approved plan         | Code + tests + docs + summary     |

### Global Rules

- One task = one chat.
- Plan **must** be approved before implementation.
- Never assume missing requirements — ask.
- Keep context minimal and relevant.
- Never duplicate logic — refactor instead.
- If a feature already exists → update docs/tests only.

---

## Section 1 — Repository Assumptions

If the repository structure differs from the below, **stop and ask**.

### Frontend
- React
- TypeScript (strict mode)
- Vite
- Jest + React Testing Library
- ESLint + Prettier

### Backend
- .NET 9 Minimal API
- MediatR pattern
- FluentValidation
- xUnit

### Infrastructure
- Dockerized PostgreSQL
- Docker Compose
- GitHub Actions CI

---

## Section 2 — Deterministic Task Workflow

### Step 1 — Context Analysis

- Read issue or SRS.
- Locate related files.
- Check if implementation already exists.

If it exists:
- Do **NOT** reimplement.
- Update documentation/tests only.

---

### Step 2 — Output Implementation Plan (No Code)

Plan must include:

- Files to create/modify
- API changes
- Data flow impact
- Validation strategy
- Test strategy
- Risk areas

**Stop and wait for approval.**

---

### Step 3 — Implementation Rules

After approval:

1. Use small logical commits.
2. No duplicated code.
3. Prefer refactor over rewrite.
4. No `any` in TypeScript.
5. Backend input validation via FluentValidation.
6. Tests only for critical paths.
7. Keep changes minimal and reviewable.

---

### Step 4 — Mandatory Deliverables

Every task must produce:

1. Working implementation
2. Passing unit tests
3. Updated README or relevant docs
4. Structured summary:

```
## Changes
- ...

## Tests
- ...

## Breaking Changes
- None / ...
```

---

### Step 5 — Self-Validation

Before finalizing:

- [ ] Frontend builds
- [ ] Backend builds
- [ ] Tests pass
- [ ] No lint errors
- [ ] No unused code
- [ ] No debug leftovers

If anything fails → fix before summarizing.

---

## Section 3 — Prompt Templates (Reference)

Prompt files live in `.github/prompts/`. This section is human-readable documentation.

| Task | Prompt File |
|------|------------|
| Implement feature | [`implement.prompt.md`](../.github/prompts/implement.prompt.md) |
| Dockerize app | [`dockerize.prompt.md`](../.github/prompts/dockerize.prompt.md) |
| Set up CI | [`setup-ci.prompt.md`](../.github/prompts/setup-ci.prompt.md) |
| Generate backend entity | [`generate-entity.prompt.md`](../.github/prompts/generate-entity.prompt.md) |
| Generate React component/page/hook | [`generate-component.prompt.md`](../.github/prompts/generate-component.prompt.md) |
| Generate MediatR handler / validator / endpoint | [`generate-handler.prompt.md`](../.github/prompts/generate-handler.prompt.md) |

### 3.1 Implement Feature → [`implement.prompt.md`](../.github/prompts/implement.prompt.md)

Deterministic feature implementation workflow. Requires `TASK_ID` variable.

### 3.2 Dockerize Application → [`dockerize.prompt.md`](../.github/prompts/dockerize.prompt.md)

Full Docker setup: Frontend Dockerfile, Backend Dockerfile, docker-compose.yml.

### 3.3 GitHub Actions CI → [`setup-ci.prompt.md`](../.github/prompts/setup-ci.prompt.md)

CI workflow with frontend, backend, and Docker build jobs.

### 3.4 Parallel Development with git worktree

```bash
# Create isolated worktree for a new branch
git worktree add ../worktree-{{BRANCH}} -b feat/{{BRANCH}}
cd ../worktree-{{BRANCH}}

# Work in isolation, commit normally
git add . && git commit -m "feat({{scope}}): ..."

# Merge back into main
cd ../main-repo
git merge --no-ff feat/{{BRANCH}}

# Clean up
git worktree remove ../worktree-{{BRANCH}}
git branch -d feat/{{BRANCH}}
```

Rules:
- Never share an open file between two worktrees.
- Each worktree must have its own `.env` if needed.
- Resolve conflicts on the merge-back branch, not in the worktree.

---

## Section 4 — Slash Commands (If Supported)

```
/implement {{TASK_ID}}
/dockerize
/setup-ci
/worktree {{BRANCH}}
```

---

## Section 5 — Pull Request Validation Checklist

See [`.github/pull_request_template.md`](../.github/pull_request_template.md).

---

## Section 6 — Context & Cost Control

- One chat per task.
- Store durable knowledge in `/docs/*.md`.
- Use stronger models for planning.
- Use lightweight models for small edits.
- Avoid loading unrelated files.

---

## Section 7 — Security Rules

- Never expose secrets.
- Never commit credentials.
- Validate dependencies before adding.
- Manually review AI-generated code.

---

## Section 8 — Code Generation Order

Always follow this sequence. Do not skip phases or mix them in a single prompt.

| Phase | Area | What to Generate |
|-------|------|-----------------|
| 1 | Foundation | Solution/project scaffolds, `.gitignore`, `.env.example`, `.github/` folder, prompt log template |
| 2 | Backend — Contracts | Domain entities, enums, DTOs as C# records, service interfaces, `ApiResponse<T>` |
| 3 | Backend — Infrastructure | `AppDbContext` + entity configs, EF Core migrations, repository implementations, `MockAiAnalysisService` |
| 4 | Backend — Application Logic | FluentValidation validators, MediatR Commands + Queries + Handlers, Minimal API endpoint maps, `Program.cs`, `GlobalExceptionHandler` |
| 5 | Frontend — Foundation | Vite + TS scaffold, TypeScript models, Axios API service layer, router configuration |
| 6 | Frontend — Layouts & Components | `MainLayout`, `AdminLayout`, common components (ErrorMessage, LoadingSpinner, SeverityBadge), feature components |
| 7 | Frontend — Pages | All page components composing layouts and components |
| 8 | Frontend — Polish | Error boundaries, styling, loading states, edge case handling |
| 9 | Infrastructure | `docker-compose.yml`, Dockerfiles (multi-stage), GitHub Actions CI |
| 10 | Documentation | README.md, verify all prompts logged in `prompts/`, INSIGHTS.md |

**Rule:** Never generate entire project in one prompt. Never mix frontend and backend in the same prompt.

---

## Section 9 — AI Code Review Checklist

Run before accepting **any** AI-generated code.

### Universal

- [ ] Code compiles without errors
- [ ] No `console.log`, `debugger`, or TODO left
- [ ] No unused imports or variables
- [ ] File is in the correct location per folder structure
- [ ] Naming follows conventions in `.github/copilot-instructions.md`
- [ ] No secrets or hardcoded credentials

### TypeScript / React

- [ ] No `any` types
- [ ] No `as unknown as X`
- [ ] Props interface defined above component
- [ ] Explicit return type on every exported function or hook
- [ ] Error handled at API boundary, displayed via `<ErrorMessage>`
- [ ] No raw `console.error` or `alert()` in production paths

### C# / .NET

- [ ] FluentValidation used for all input validation (no manual null checks)
- [ ] MediatR pattern: handler contains the logic, endpoint only calls `Send()`
- [ ] `CancellationToken` passed on all async methods
- [ ] `ProblemDetails` returned on all error paths (no raw exceptions)
- [ ] Repository used for all DB access (no raw EF queries outside repositories)
- [ ] `IReadOnlyList<T>` used for collection returns

### Tests

- [ ] Tests follow AAA pattern
- [ ] One assertion per test
- [ ] Test method name: `{Method}_{Scenario}_{ExpectedResult}`
- [ ] No magic strings or hardcoded IDs in tests

---

## Section 10 — Manual vs AI Code Rules

| Scenario | Who Writes | Documentation Required |
|----------|-----------|----------------------|
| New file from scratch | AI | Save prompt as `{N}_{key}.md` in `prompts/` |
| Bug fix (< 5 lines) | AI or manual | `// MANUAL EDIT: <reason>` if manual |
| Configuration tweak | Manual OK | Note in corresponding prompt log |
| `package.json` / `.csproj` edits | AI or manual | Note in corresponding prompt log |
| Connection strings / env vars | Manual OK | Document in `.env.example` |
| AI-generated code that doesn't compile | Fix with AI first | If manual fix: mark with comment |

### Escalation Rule

If AI cannot produce correct code after **2 attempts**:

1. Simplify the prompt — break into smaller pieces
2. Provide more context — reference existing interfaces and related files
3. If still failing → manual fix with:

```
// MANUAL EDIT: AI failed after 2 attempts — <description of what was done and why>
```

### Marking Manual Edits

Any manual change to an AI-generated file must be marked:

```typescript
// MANUAL EDIT: <reason>
const correctedValue = ...;
```

```csharp
// MANUAL EDIT: <reason>
var correctedValue = ...;
```
- No external data leaks.

---

## Section 8 — Definition of Done

A task is complete **only** when:

- [ ] Code works
- [ ] Tests pass
- [ ] Documentation is updated
- [ ] CI would pass
- [ ] Docker setup works (if applicable)
- [ ] Summary is structured and clear

---

*End of AI Workflow Reference*
