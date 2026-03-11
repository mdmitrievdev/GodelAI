# Copilot Skill: AI-ED-FTG16 Deterministic Development Workflow

**Stack: React (Vite + TypeScript) + .NET 9 + Docker + GitHub Actions**

------------------------------------------------------------------------

## 🎯 Purpose

Convert vague requirements into production-ready code using a
structured, plan-first, AI-assisted workflow optimized for GitHub
Copilot Agents.

This skill enforces deterministic execution, minimal context usage, test
discipline, and reproducible builds.

------------------------------------------------------------------------

# 0) Operating Modes

  Mode    Purpose                       Output
  ------- ----------------------------- -------------------------------
  Chat    Clarification & exploration   Direct answer only
  Plan    Implementation strategy       Step-by-step plan (no code)
  Agent   Execute approved plan         Code + tests + docs + summary

## Global Rules

-   One task = one chat.
-   Plan MUST be approved before implementation.
-   Never assume missing requirements --- ask.
-   Keep context minimal and relevant.
-   Never duplicate logic --- refactor instead.
-   If feature already exists → update docs/tests only.

------------------------------------------------------------------------

# 1) Repository Assumptions

If the repository structure differs, stop and ask.

## Frontend

-   React
-   TypeScript (strict mode)
-   Vite
-   Jest + React Testing Library
-   ESLint + Prettier

## Backend

-   .NET 9 Minimal API
-   MediatR pattern
-   FluentValidation
-   xUnit

## Infrastructure

-   Dockerized PostgreSQL
-   Docker Compose
-   GitHub Actions CI

------------------------------------------------------------------------

# 2) Deterministic Task Workflow

## Step 1 --- Context Analysis

-   Read issue or SRS
-   Locate related files
-   Check if implementation already exists

If exists: - Do NOT reimplement - Update documentation/tests only

------------------------------------------------------------------------

## Step 2 --- Output Implementation Plan (No Code)

Plan must include:

-   Files to create/modify
-   API changes
-   Data flow impact
-   Validation strategy
-   Test strategy
-   Risk areas

Stop and wait for approval.

------------------------------------------------------------------------

## Step 3 --- Implementation Rules

After approval:

1.  Use small logical commits
2.  No duplicated code
3.  Prefer refactor over rewrite
4.  No `any` in TypeScript
5.  Backend input validation via FluentValidation
6.  Tests only for critical paths
7.  Keep changes minimal and reviewable

------------------------------------------------------------------------

## Step 4 --- Mandatory Deliverables

Every task must include:

1.  Working implementation
2.  Passing unit tests
3.  Updated README or relevant docs
4.  Structured summary:

```{=html}
<!-- -->
```
    ## Changes
    - ...

    ## Tests
    - ...

    ## Breaking Changes
    - None / ...

------------------------------------------------------------------------

## Step 5 --- Self-Validation

Before finalizing:

-   Frontend builds
-   Backend builds
-   Tests pass
-   No lint errors
-   No unused code
-   No debug leftovers

If something fails → fix before summarizing.

------------------------------------------------------------------------

# 3) Prompt Templates

------------------------------------------------------------------------

## 3.1 Implement Feature

    Role: Senior Full-Stack Engineer
    Goal: Implement task {{TASK_ID}} using deterministic workflow.

    Follow strictly:
    1. Analyze context
    2. Check for existing implementation
    3. Output plan only
    4. Wait for approval
    5. Implement
    6. Add focused tests
    7. Validate build & tests
    8. Update docs
    9. Provide structured summary

------------------------------------------------------------------------

## 3.2 Dockerize Application

Requirements:

### Frontend Dockerfile

-   Multi-stage build
-   node:lts for build
-   nginx:alpine for runtime
-   Copy `dist`
-   Expose port 80

### Backend Dockerfile

-   Multi-stage SDK build + test
-   Runtime: `mcr.microsoft.com/dotnet/aspnet:9.0`
-   Include HEALTHCHECK

### docker-compose.yml

-   Services: frontend, backend, db
-   Named network
-   Proper `depends_on` with healthchecks
-   Environment variables
-   Volumes for development

### Validation

    docker compose build
    docker compose up -d
    curl /health

------------------------------------------------------------------------

## 3.3 GitHub Actions CI

Workflow name: `CI`

Trigger: - pull_request

Jobs: - frontend (Node + cache) - backend (.NET + cache) - docker (image
build)

Rules: - Fail fast on error - Upload test reports as artifacts - No
redundant steps

------------------------------------------------------------------------

## 3.4 Parallel Development with git worktree

-   Create new worktree for branch {{BRANCH}}
-   Explain merge-back process
-   Provide conflict resolution guidance
-   Keep branches isolated

------------------------------------------------------------------------

# 4) Slash Commands (Optional)

If supported by agent:

-   `/implement {{TASK_ID}}`
-   `/dockerize`
-   `/setup-ci`
-   `/worktree {{BRANCH}}`

------------------------------------------------------------------------

# 5) Pull Request Validation Checklist

-   [ ] Frontend builds
-   [ ] Backend builds
-   [ ] Tests pass
-   [ ] Docker images build
-   [ ] docker compose runs
-   [ ] Docs updated
-   [ ] No duplicated logic
-   [ ] No secrets committed

------------------------------------------------------------------------

# 6) Context & Cost Control

-   One chat per task
-   Store durable knowledge in `/docs/*.md`
-   Use stronger models for planning
-   Use lightweight models for small edits
-   Avoid loading unrelated files

------------------------------------------------------------------------

# 7) Security Rules

-   Never expose secrets
-   Never commit credentials
-   Validate dependencies before adding
-   Manually review AI-generated code
-   No external data leaks

------------------------------------------------------------------------

# 8) Definition of Done

A task is complete only when:

-   Code works
-   Tests pass
-   Documentation is updated
-   CI would pass
-   Docker setup works (if applicable)
-   Summary is structured and clear

------------------------------------------------------------------------

**End of Skill Definition**
