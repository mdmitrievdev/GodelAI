---
name: my-implementFeature
description: Implement a feature using the deterministic AI-ED-FTG16 workflow.
agent: agent
---

# Implement Feature — AI-ED-FTG16

**Role:** Senior Full-Stack Engineer (React + TypeScript + .NET 9)  
**Task ID:** `${input:TASK_ID:Enter the task or issue ID (e.g. FTG-42)}`

---

## Pre-Conditions

Before writing any code:

1. Read the task description or linked issue for `${input:TASK_ID}`.
2. Locate all related files (components, handlers, validators, tests, docs).
3. Determine if this feature **already exists** in any form.
   - If it exists → update docs and tests only. Do NOT reimplement. Stop here.
   - If it does not exist → proceed to Step 1.

---

## Step 1 — Output Implementation Plan (No Code)

Produce a written plan covering **all** of the following:

| Section             | Required Content                                      |
|---------------------|-------------------------------------------------------|
| Files               | List every file to create or modify with reason       |
| API Changes         | New endpoints, modified contracts, breaking changes   |
| Data Flow           | How data moves from input to persistence              |
| Validation Strategy | Frontend rules + Backend FluentValidation rules       |
| Test Strategy       | What to test, why, and which layer (unit/integration) |
| Risk Areas          | What could break, side effects, edge cases            |

**STOP. Do not write code. Wait for explicit approval before continuing.**

---

## Step 2 — Implement (After Approval)

Follow these rules strictly:

### General
- Commit in small logical units.
- Keep diff minimal and reviewable.
- Prefer refactoring existing code over adding new.
- Never duplicate logic — extract shared utilities instead.

### TypeScript (Frontend)
- Strict mode. No `any`. No `as unknown as X`.
- Use explicit types for all function signatures.
- Component props must have named interfaces.
- State management: prefer local state; use global only when justified.
- Tests: Jest + React Testing Library for critical paths only.

### C# (Backend)
- Follow MediatR pattern: Command/Query → Handler.
- Validate all inputs with FluentValidation. No manual null checks.
- Return typed `Result<T>` or `ProblemDetails` on failure.
- Tests: xUnit, AAA pattern, one assertion per test.

### Infrastructure
- Docker and Compose changes follow multi-stage build conventions.
- New environment variables → update `.env.example` and docs.

---

## Step 3 — Mandatory Deliverables

Do not finalize without all of the following:

- [ ] Working implementation
- [ ] Unit tests passing for critical paths
- [ ] README or docs updated (if public API or behavior changed)
- [ ] Structured summary:

```markdown
## Task: ${input:TASK_ID}

### Changes
- 

### Tests
- 

### Breaking Changes
- None / (describe if any)
```

---

## Step 4 — Self-Validation Checklist

Run before delivering:

```bash
# Frontend
cd frontend
npm run build
npm test -- --passWithNoTests

# Backend
cd backend
dotnet build
dotnet test
```

- [ ] Frontend builds without errors
- [ ] Backend builds without errors
- [ ] All tests pass
- [ ] No ESLint warnings or TypeScript errors
- [ ] No unused imports, variables, or debug code
- [ ] No `console.log`, `debugger`, or `TODO` left in delivery

**Fix all failures before providing the structured summary.**
