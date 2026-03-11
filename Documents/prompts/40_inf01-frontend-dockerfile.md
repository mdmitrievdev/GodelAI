# Prompt Log — 40_inf01-frontend-dockerfile

> **File naming convention:** `{IDENTITY_NUMBER}_{SHORT_KEY}.md`

---

## Metadata

| Field | Value |
|-------|-------|
| **Number** | 40 |
| **Key** | inf01-frontend-dockerfile |
| **Date** | 2026-03-10 |
| **Mode** | Agent |
| **Model** | GitHub Copilot (claude-sonnet-4-5) |
| **Task ref** | INF-01 |

---

## Context Provided

- `.github/plan-pr-review-assistant.md` — TASK INF-01 acceptance criteria
- `frontend/package.json` — build scripts (`npm run build`)
- `frontend/vite.config.ts` — Vite config
- `infra/nginx.conf` — existing nginx configuration with backend proxy

---

## Prompt

```
#file:plan-pr-review-assistant.md Implement #### TASK INF-01: Frontend Dockerfile
```

---

## Response Summary

- Created `frontend/Dockerfile` with two-stage build: `node:lts-alpine` (build) → `nginx:alpine` (serve)
- Stage 1 accepts `VITE_API_URL` build arg (defaults to `/api`), runs `npm ci` then `npm run build`
- Stage 2 copies `dist/` into nginx html root and `nginx.conf` into default site config
- Adds `HEALTHCHECK` via `wget` on `/index.html`
- Created `frontend/.dockerignore` excluding `node_modules/`, `dist/`, `.env*`, logs, coverage
- Created `frontend/nginx.conf` mirroring `infra/nginx.conf` (SPA fallback, `/api/` proxy to backend, gzip)
- Marked all INF-01 acceptance criteria checkboxes as complete in the plan

---

## Files Created / Modified

| File | Action |
|------|--------|
| `frontend/Dockerfile` | created |
| `frontend/.dockerignore` | created |
| `frontend/nginx.conf` | created |
| `.github/plan-pr-review-assistant.md` | modified — INF-01 checkboxes marked `[x]` |

---

## Outcome

- [x] Accepted as-is
- [ ] Accepted with manual edits (`// MANUAL EDIT:` comments added)
- [ ] Rejected — reason: ___

---

## Notes

- `frontend/nginx.conf` is the config used inside the Docker image; `infra/nginx.conf` is a shared reference copy for the repo. Both are identical.
- `VITE_API_URL` defaults to `/api` so the nginx proxy handles routing in production without needing a runtime env var.
