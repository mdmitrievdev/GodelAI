---
applyTo: "**"
name: my-DockerizeApplication
description: Dockerize the full application — Frontend, Backend, and PostgreSQL with Docker Compose.
agent: agent
---

# Dockerize Application — AI-ED-FTG16

**Role:** Senior DevOps / Full-Stack Engineer  
**Goal:** Produce a complete, production-ready Docker setup for the React + .NET 9 + PostgreSQL stack.

---

## Pre-Conditions

Before writing any Dockerfile:

1. Confirm the repository structure matches the expected layout:
   ```
   frontend/    (React + Vite)
   backend/     (.NET 9)
   ```
2. Check if any Dockerfiles or `docker-compose.yml` already exist.
   - If they exist → review and patch only. Do NOT recreate.
3. Output a plan and **wait for approval** before creating any file.

---

## Required Deliverables

### 1. `frontend/Dockerfile` — Multi-Stage Build

```
Stage 1 (build):
  Base image:   node:lts-alpine
  Working dir:  /app
  Steps:        copy package files → npm ci → npm run build

Stage 2 (runtime):
  Base image:   nginx:alpine
  Copy:         dist/ from Stage 1 → /usr/share/nginx/html
  Copy:         nginx.conf (if exists)
  Expose:       80
  CMD:          nginx -g "daemon off;"
```

Requirements:
- `.dockerignore` must exclude `node_modules/`, `.env`, `dist/`.
- Build args for `VITE_API_URL` or other env vars injected at build time.

---

### 2. `backend/Dockerfile` — Multi-Stage Build

```
Stage 1 (build):
  Base image:   mcr.microsoft.com/dotnet/sdk:9.0
  Working dir:  /src
  Steps:        restore → build → test → publish

Stage 2 (runtime):
  Base image:   mcr.microsoft.com/dotnet/aspnet:9.0
  Working dir:  /app
  Copy:         published output from Stage 1
  Expose:       8080
  HEALTHCHECK:  curl --fail http://localhost:8080/health || exit 1
  ENTRYPOINT:   dotnet YourApp.dll
```

Requirements:
- Tests must run in Stage 1. Build fails if tests fail.
- No SDK in the final image.
- `.dockerignore` must exclude `bin/`, `obj/`.

---

### 3. `docker-compose.yml`

Services: `frontend`, `backend`, `db`

```yaml
# Required structure — fill values from project context
services:
  frontend:
    build: ./frontend
    ports: ["3000:80"]
    depends_on:
      backend:
        condition: service_healthy

  backend:
    build: ./backend
    ports: ["8080:8080"]
    environment:
      - ConnectionStrings__Default=...
    depends_on:
      db:
        condition: service_healthy
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
      interval: 10s
      retries: 5

  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: appdb
      POSTGRES_USER: appuser
      POSTGRES_PASSWORD: ${DB_PASSWORD}
    volumes:
      - db_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U appuser"]
      interval: 10s
      retries: 5

networks:
  default:
    name: app_network

volumes:
  db_data:
```

Requirements:
- All secrets via environment variables. Never hardcoded.
- Provide `.env.example` with all required variables.
- Named network for service isolation.

---

## Validation Commands

After delivering files, run and confirm:

```bash
docker compose build
docker compose up -d
docker compose ps
curl http://localhost:8080/health
curl http://localhost:3000
```

---

## Deliverables Checklist

- [ ] `frontend/Dockerfile` (multi-stage)
- [ ] `frontend/.dockerignore`
- [ ] `backend/Dockerfile` (multi-stage with tests + healthcheck)
- [ ] `backend/.dockerignore`
- [ ] `docker-compose.yml` (all 3 services with healthchecks)
- [ ] `.env.example` (all variables documented)
- [ ] `docker compose build` succeeds
- [ ] `docker compose up -d` runs all services healthy
- [ ] `/health` endpoint responds

---

## Structured Summary

```markdown
### Changes
- 

### Images
- frontend: node:lts-alpine → nginx:alpine
- backend: dotnet/sdk:9.0 → dotnet/aspnet:9.0

### Environment Variables Added
- 

### Breaking Changes
- None / (describe if any)
```
