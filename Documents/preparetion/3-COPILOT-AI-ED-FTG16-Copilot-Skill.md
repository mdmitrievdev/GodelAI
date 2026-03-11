# Copilot Skill: AI‑ED‑FTG16 Development Workflow (Frontend + Backend + Docker + CI)

> **Purpose**: Turn vague requirements into shipped code with GitHub Copilot (or Claude-like Copilot agents) using a reproducible, review-first, AI‑assisted workflow. Optimized for React/TypeScript (Vite) + .NET 9 backend, Dockerized DB, and GitHub Actions CI.

---

## 0) Roles & Modes
- **Chat** — Q&A, exploration.
- **Plan** — create step-by-step implementation plan *before* writing code.
- **Agent** — execute the plan: edit files, run commands, create PRs.

> **Rule**: One chat per task. Keep context small and relevant.

---

## 1) Project Context (paste once per repo)
**General instructions for the agent**
- Tech stack: React + TS (Vite), Jest/RTL, .NET 9 (Minimal API, MediatR), Dockerized Postgres, Docker Compose, GitHub Actions.
- Conventions: TypeScript strict, ESLint + Prettier; backend uses FluentValidation; tests are *focused* (no test bloat).
- Deliverables for each task:
  1. Minimal plan with steps.
  2. Code changes.
  3. Unit tests (only critical paths).
  4. Update docs/README.
  5. Self-check: build + tests green.
- Never duplicate code. Prefer refactors to reuse.
- If something *already exists*, skip re‑implementation and instead update docs/tests.

---

## 2) Prompt Templates

### 2.1 Implement Task (save as `.promt.md`)
```
Role: Senior Full‑Stack Assistant
Mission: Implement task {{TASK_ID}} end‑to‑end and ensure build & tests pass.

Workflow:
1) Analyze context: read SRS or issue, open related files.
2) Check if the feature already exists; if yes, update docs/tests only.
3) Create a concrete plan (bullets): files to change, APIs, data flow.
4) Implement with small, reviewable commits.
5) Add/adjust unit tests (front/back): keep focused.
6) Run build & tests; fix errors.
7) Update CHANGELOG/README with usage notes.
8) Post a concise summary of changes.

Constraints:
- TypeScript strict, no `any`.
- Don’t skip TypeScript/Jest config tweaks if needed.
- Keep context under budget; ask for missing details.
```

### 2.2 Dockerize Frontend & Backend
```
Goal: Create production‑grade Dockerfiles for frontend and backend and wire them via docker‑compose.
Steps:
- Frontend Dockerfile: multi‑stage (build with node:lts, run with nginx:alpine). Expose 80; copy built `dist`.
- Backend Dockerfile: multi‑stage (SDK build/test, runtime `mcr.microsoft.com/dotnet/aspnet:9.0`), healthcheck.
- docker‑compose.yml: services {frontend, backend, db}; networks; env; volumes for dev; depends_on with healthchecks.
- Provide `README` run commands and `.dockerignore` files.
- Validate locally: `docker compose build` + `docker compose up -d` + curl health endpoints.
```

### 2.3 CI: GitHub Actions for Docker + Tests
```
Goal: On PR, build & test backend and frontend; build Docker images; fail fast on errors.
- Workflow name: CI
- Triggers: pull_request
- Jobs: node(frontend), dotnet(backend), docker (build images)
- Cache: node and NuGet
- Artifacts: test reports
```

### 2.4 Parallel Dev with `git worktree`
```
Explain and apply git worktree to develop tasks in parallel locally:
- Create new worktree for branch {{BRANCH}}.
- Merge back with conflict resolution guidance.
```

---

## 3) Ready‑to‑Run Commands (Agent Mode)
- Implement feature task: `/implement-task {{TASK_ID}}`
- Dockerize: `/dockerize-app`
- Setup CI: `/setup-ci`
- Create worktree: `/git-worktree-create {{BRANCH}}`

> Map these commands in your agent if the tool supports slash‑commands.

---

## 4) Validation Checklist (copy into PR description)
- [ ] Build frontend (Node) passes; lint ok
- [ ] Backend builds; unit tests pass
- [ ] Docker images build for FE/BE
- [ ] `docker compose up` runs; healthchecks green
- [ ] README updated; breaking changes documented

---

## 5) Context Hygiene & Cost Control
- Keep one chat per task; purge unrelated files.
- Save durable knowledge in `/docs/*.md`; reference it in later chats.
- Choose models by task complexity; fall back to cheaper ones for simple edits.

---

## 6) Security & Data Handling
- Don’t send secrets/tokens to external tools.
- Prefer verified MCP servers; review permissions.
- Always manually review AI‑generated code before merge.