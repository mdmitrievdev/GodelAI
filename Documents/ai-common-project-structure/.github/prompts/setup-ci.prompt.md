---
name: my-setupCI
description: Create or update GitHub Actions CI workflow for the React + .NET 9 + Docker stack.
agent: agent
---

# Set Up GitHub Actions CI — AI-ED-FTG16

**Role:** Senior DevOps Engineer  
**Goal:** Create a complete, efficient CI pipeline that validates the full stack on every pull request.

---

## Pre-Conditions

1. Check if `.github/workflows/ci.yml` already exists.
   - If it exists → patch and improve only. Do NOT recreate.
2. Output a plan and **wait for approval** before modifying any workflow file.

---

## Required Workflow

**File:** `.github/workflows/ci.yml`  
**Name:** `CI`  
**Trigger:** `pull_request` (all branches)

---

## Jobs

### Job 1 — `frontend`

```yaml
runs-on: ubuntu-latest
steps:
  - uses: actions/checkout@v4
  - uses: actions/setup-node@v4
    with:
      node-version: lts/*
      cache: npm
      cache-dependency-path: frontend/package-lock.json
  - run: npm ci
    working-directory: frontend
  - run: npm run lint
    working-directory: frontend
  - run: npm run build
    working-directory: frontend
  - run: npm test -- --ci --passWithNoTests --coverage
    working-directory: frontend
  - uses: actions/upload-artifact@v4
    if: always()
    with:
      name: frontend-test-results
      path: frontend/coverage/
```

---

### Job 2 — `backend`

```yaml
runs-on: ubuntu-latest
steps:
  - uses: actions/checkout@v4
  - uses: actions/setup-dotnet@v4
    with:
      dotnet-version: 9.0.x
      cache: true
      cache-dependency-path: backend/**/*.csproj
  - run: dotnet restore
    working-directory: backend
  - run: dotnet build --no-restore --configuration Release
    working-directory: backend
  - run: dotnet test --no-build --configuration Release --logger trx --results-directory TestResults
    working-directory: backend
  - uses: actions/upload-artifact@v4
    if: always()
    with:
      name: backend-test-results
      path: backend/TestResults/
```

---

### Job 3 — `docker`

```yaml
runs-on: ubuntu-latest
needs: [frontend, backend]
steps:
  - uses: actions/checkout@v4
  - run: docker compose build
  - run: docker compose up -d
  - run: sleep 10
  - run: curl --fail http://localhost:8080/health
  - run: docker compose down
```

---

## Rules

- `fail-fast: true` — stop all jobs on first failure.
- Each job is independent; no shared state between `frontend` and `backend`.
- `docker` job runs only after both `frontend` and `backend` succeed (`needs`).
- Test artifacts uploaded on every run (`if: always()`), even on failure.
- No redundant steps — do not install tools that are already in `ubuntu-latest`.
- No hardcoded secrets — use `${{ secrets.* }}` where needed.
- Cache npm and NuGet to keep workflow under 3 minutes on cold runs.

---

## Deliverables Checklist

- [ ] `.github/workflows/ci.yml` created or updated
- [ ] `frontend` job: lint + build + test + upload coverage
- [ ] `backend` job: restore + build + test + upload results
- [ ] `docker` job: build + up + health check + down
- [ ] Workflow passes on a test PR in the repository

---

## Structured Summary

```markdown
### Changes
- 

### Workflow Jobs
- frontend: lint → build → test → upload
- backend: restore → build → test → upload
- docker: build → up → health check (depends on frontend + backend)

### Secrets Required
- None / (list if any)

### Breaking Changes
- None / (describe if any)
```
