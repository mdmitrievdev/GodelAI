# GPT-AI-ED-FTG16 — Copilot Architecture Summary

**Date:** March 2, 2026  
**Stack:** React (Vite + TypeScript) + .NET 9 + Docker + GitHub Actions

---

## Origin

The file `GPT-AI-ED-FTG16-Copilot-Skill.md` was a deterministic AI development standard written as a single monolithic markdown document and labelled as a "Copilot Skill". 

**Problem:** GitHub Copilot does not have a "skills" concept for `.md` files. To make the content actionable, it needed to be restructured into the architecture GitHub Copilot actually supports: instructions + prompt files + PR templates.

---

## Decision

**Option B (3-layer architecture)** was selected as the correct approach:

| Layer | Purpose |
|---|---|
| Behavioral instructions | Rules Copilot loads automatically on every session |
| Executable prompt templates | Reusable, invokable prompts for specific tasks |
| PR template | Validation checklist auto-injected by GitHub on every PR |

The original document was preserved as a full reference and linked from all other files.

---

## Output Structure

All files created in `E:\Dev\GodelAI\Documents\ai-common-project-structure\`:

```
.
├── docs/
│   └── ai-workflow.md                        ← Full reference document (original content, restructured)
└── .github/
    ├── copilot-instructions.md               ← Auto-loaded by Copilot Chat in VS Code
    ├── pull_request_template.md              ← Auto-injected by GitHub into every PR
    └── prompts/
        ├── implement.prompt.md               ← Implement any feature (uses TASK_ID variable)
        ├── dockerize.prompt.md               ← Full Docker setup (Frontend + Backend + Compose)
        └── setup-ci.prompt.md                ← GitHub Actions CI (3-job pipeline)
```

---

## File Descriptions

### `docs/ai-workflow.md`
Complete human-readable reference for the AI-ED-FTG16 standard. Contains all 8 sections from the original file, cleaned and cross-linked. Acts as the source of truth for the development workflow.

### `.github/copilot-instructions.md`
The primary Copilot configuration file. GitHub Copilot Chat in VS Code loads this automatically when it exists in the repository. Contains:

- **Operating Modes** — Chat / Plan / Agent with triggers and expected output
- **Global Rules** — plan-first, no duplication, no assumptions
- **Communication Style** — concise responses, no filler, assumption-first approach
- **Repository Structure** — expected folder layout; stop and ask if it differs
- **Code Conventions** — full naming, typing, error handling, and logging rules for TypeScript/React and C#/.NET, plus API contract standards
- **Deterministic Task Workflow** — 5-step mandatory sequence (analyze → plan → approve → implement → validate)
- **Git Workflow** — Conventional Commits format, branch naming strategy, squash merge policy
- **Context & Cost Control** — model selection guidance, knowledge storage rules
- **Security Rules** — no secrets, no PII in logs, manual AI code review required
- **Definition of Done** — 6-point completion checklist

### `.github/prompts/implement.prompt.md`
Executable agent prompt for feature implementation. Enforces the full plan → approve → implement → test → validate cycle. Accepts `TASK_ID` as an input variable. Contains TypeScript and C#-specific implementation rules and a mandatory structured summary template.

### `.github/prompts/dockerize.prompt.md`
Executable agent prompt for full Docker setup. Specifies exact multi-stage Dockerfile requirements for both frontend (node:lts → nginx:alpine) and backend (dotnet/sdk:9.0 → dotnet/aspnet:9.0 with integrated tests and HEALTHCHECK), plus docker-compose.yml with all 3 services, healthchecks, named network, volumes, and `.env.example`.

### `.github/prompts/setup-ci.prompt.md`
Executable agent prompt for GitHub Actions CI. Produces a 3-job workflow: `frontend` (lint + build + test + coverage upload), `backend` (restore + build + test + results upload), and `docker` (build + up + health check), with caching, fail-fast, and artifact upload on every run.

### `.github/pull_request_template.md`
GitHub PR description template. Automatically used by GitHub when opening a pull request. Contains sections for task reference, change description, type of change, test coverage, full validation checklist (code quality, build, Docker, documentation, security), breaking changes, and a reviewer summary paragraph.

---

## Key Additions Beyond the Original

The original file did not cover these areas. They were added during the restructuring as best-practice additions:

### Communication Style
Explicit rules for how Copilot should respond — imperative mood, no filler phrases, assumptions stated and acted on rather than listed as options.

### Code Conventions (TypeScript/React)
- Naming conventions for components, hooks, utilities, constants, types
- Strict TypeScript rules: no `any`, no `!` assertion, `unknown` + narrowing
- React patterns: functional only, props interface above component, error boundaries on every route
- Error handling: normalize at API boundary, shared error component, no silent `catch {}`
- Logging: structured wrapper only, no raw `console.log` in production

### Code Conventions (C#/.NET)
- Naming: `_camelCase` fields, `Async` suffix, `IInterface` prefix, test method `Method_Scenario_Result`
- Architecture: MediatR strictly enforced, zero business logic in endpoints or validators
- Error handling: `ProblemDetails` (RFC 7807) via global middleware, no stack traces exposed to client
- Logging: `ILogger<T>` with correlation IDs, structured properties, no PII

### API Contract Standards
- Response envelope: `{ data: T | null, error: ProblemDetails | null }`
- IDs as strings (ULID or UUID — never raw integer DB IDs)
- Timestamps: ISO 8601 UTC
- Pagination shape defined
- URL versioning: `/api/v1/`

### Git Workflow
- Branch naming: `<type>/<ticket-id>-<short-description>` (e.g. `feat/FTG-42-user-authentication`)
- Commit format: Conventional Commits (`feat(auth): add JWT refresh token rotation`)
- Merge strategy: squash merge to `main`, rebase before PR, no direct commits to `main`

---

## How to Use in a Repository

1. Copy `.github/` and `docs/` into the repository root.
2. Copilot Chat picks up `.github/copilot-instructions.md` automatically on the next session.
3. Use prompt files by attaching them in Copilot Chat or referencing them by path.
4. The PR template activates automatically when opening a pull request on GitHub.
5. Refer to `docs/ai-workflow.md` for the complete standard.
