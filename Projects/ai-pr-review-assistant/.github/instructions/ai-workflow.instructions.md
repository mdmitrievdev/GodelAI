---
applyTo: "**"
---

# AI Development Workflow — PR Review Assistant

> Source: `docs/ai-workflow.md`. Full detail in that file; unique/supplemental content surfaced here.
> For Copilot behavioral rules and conventions see `.github/copilot-instructions.md`.

---

## Reusable Prompt Files

Use these files from `.github/prompts/` by attaching them in chat or typing `#` to reference them.

| Task | Prompt File |
|------|-------------|
| Implement a feature (any task ID) | `.github/prompts/implement.prompt.md` |
| Dockerize the application | `.github/prompts/dockerize.prompt.md` |
| Set up GitHub Actions CI | `.github/prompts/setup-ci.prompt.md` |
| Generate a backend entity / DTO / enum / interface | `.github/prompts/generate-entity.prompt.md` |
| Generate a React component / page / hook / layout | `.github/prompts/generate-component.prompt.md` |
| Generate a MediatR handler / validator / endpoint | `.github/prompts/generate-handler.prompt.md` |

Each prompt file includes pre-conditions, variables (`${input:VARIABLE}`), generation rules, validation checklist, and deliverables. Always use these instead of writing ad-hoc prompts.

---

## Code Generation Order (Phases)

Never mix phases in one prompt. Never skip phases.

| Phase | Area | What to Generate |
|-------|------|-----------------|
| 1 | Foundation | Solution scaffolds, `.gitignore`, `.env.example`, `.github/` folder, prompt log template |
| 2 | Backend — Contracts | Domain entities, enums, DTOs (`record`), service interfaces, `ApiResponse<T>` |
| 3 | Backend — Infrastructure | `AppDbContext`, EF migrations, repository implementations, `MockAiAnalysisService` |
| 4 | Backend — Application Logic | FluentValidation validators, MediatR Commands + Queries + Handlers, Minimal API endpoint maps, `Program.cs`, `GlobalExceptionHandler` |
| 5 | Frontend — Foundation | Vite + TS scaffold, TypeScript models, Axios API service layer, router config |
| 6 | Frontend — Layouts & Components | `MainLayout`, `AdminLayout`, common components (`ErrorMessage`, `LoadingSpinner`, `SeverityBadge`) |
| 7 | Frontend — Pages | All page components composing layouts and hooks |
| 8 | Frontend — Polish | Error boundaries, styling, loading states, edge case handling |
| 9 | Infrastructure | `docker-compose.yml`, multi-stage Dockerfiles, GitHub Actions CI |
| 10 | Documentation | `README.md`, verify all prompts logged in `prompts/`, `INSIGHTS.md` |

---

## AI Code Review Checklist

Run before accepting **any** AI-generated code.

### Universal
- [ ] Compiles without errors
- [ ] No `console.log`, `debugger`, or TODO left
- [ ] No unused imports or variables
- [ ] File is in the correct location per folder structure
- [ ] Naming follows `.github/copilot-instructions.md` conventions
- [ ] No secrets or hardcoded credentials

### TypeScript / React
- [ ] No `any` types
- [ ] No `as unknown as X`
- [ ] Props interface defined immediately above the component
- [ ] Explicit return type on every exported function and hook
- [ ] Errors normalized at API boundary, shown via `<ErrorMessage>` — never `alert()`
- [ ] No raw `console.error` in production paths

### C# / .NET
- [ ] FluentValidation used for all input validation (no manual null checks in handlers)
- [ ] MediatR: handler contains logic, endpoint only calls `Send()`
- [ ] `CancellationToken` passed on all async methods
- [ ] `ProblemDetails` (RFC 7807) returned on all error paths
- [ ] Repository used for all DB access — no raw EF queries outside repositories
- [ ] `IReadOnlyList<T>` used for collection return types

### Tests
- [ ] AAA pattern (Arrange / Act / Assert)
- [ ] One assertion per test
- [ ] Method name: `{Method}_{Scenario}_{ExpectedResult}`
- [ ] No magic strings or hardcoded IDs

---

## Manual vs AI Code

| Scenario | Who Writes | Required Documentation |
|----------|-----------|----------------------|
| New file from scratch | AI | Save prompt as `{N}_{key}.md` in `prompts/` |
| Bug fix (< 5 lines) | AI or manual | `// MANUAL EDIT: <reason>` if manual |
| Configuration tweak | Manual OK | Note in corresponding prompt log |
| `package.json` / `.csproj` edits | AI or manual | Note in corresponding prompt log |
| Connection strings / env vars | Manual OK | Document in `.env.example` |
| AI code that doesn't compile | Fix with AI first | Manual fix: mark with `// MANUAL EDIT:` |

### Escalation Rule
If AI fails after **2 attempts**:
1. Simplify — break the prompt into smaller pieces.
2. Add context — reference existing interfaces and related files.
3. Manual fix with comment: `// MANUAL EDIT: AI failed after 2 attempts — <description>`

---

## Parallel Development with git worktree

Use worktrees to work on isolated branches without stashing or switching:

```bash
# Create isolated worktree for a new branch
git worktree add ../worktree-{BRANCH} -b feat/{BRANCH}
cd ../worktree-{BRANCH}

# Work in isolation, commit normally
git add . && git commit -m "feat({scope}): ..."

# Merge back into main
cd ../main-repo
git merge --no-ff feat/{BRANCH}

# Clean up
git worktree remove ../worktree-{BRANCH}
git branch -d feat/{BRANCH}
```

Rules:
- Never share an open file between two worktrees simultaneously.
- Each worktree must have its own `.env` if env vars differ.
- Resolve conflicts on the merge-back branch, not in the worktree.

---

## Context & Cost Control

- One task = one chat. Never mix tasks in the same chat.
- Store durable knowledge in `docs/` and `.github/` — not in chat history.
- Use stronger models (Claude Sonnet, GPT-4o) for planning and architecture.
- Use lightweight models for small edits and formatting.
- Avoid loading unrelated files into context.
- Reference only the files actually needed for the current task.

---

## Self-Validation (Before Submitting Any Task)

- [ ] Frontend builds (`npm run build` with zero errors)
- [ ] Backend builds (`dotnet build` with zero errors)  
- [ ] All tests pass (`npm test` / `dotnet test`)
- [ ] No lint errors (`npm run lint` / `dotnet build --warnaserror`)
- [ ] No unused code or debug leftovers
- [ ] No `any` types or missing return types in TypeScript

Fix all failures before providing the task summary.

---

## Post-Completion Bookkeeping (Mandatory)

After every successfully completed task, in this order:

### 1. Mark the plan file
Open `.github/plan-pr-review-assistant.md` and change `[ ]` to `[x]` for all acceptance-criteria checkboxes belonging to completed tasks only.

### 2. Log the prompt
Save as `{N}_{short-key}.md` in `project-root/prompts/`. Next number = count of files in `prompts/` (excluding `_prompt-log-template.md`). Use the template at `prompts/_prompt-log-template.md`.

### 3. Update README.md
The README must always reflect the **current working state** only:
- `## What This App Does` — user-facing features that are fully working
- `## Technical Overview` — stack, folder structure, run commands, env vars
- `## Changelog` — append `- [date] [task-id] <one-line description>`
- Remove any references to features not yet built
