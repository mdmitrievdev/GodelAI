# PR Review Assistant — Documentation Templates

---

## Template 1: README.md

```markdown
# PR Review Assistant

AI-powered code review assistant that analyzes code diffs and provides categorized, severity-rated feedback — built with 90%+ AI-generated code using GitHub Copilot.

## Overview

PR Review Assistant helps developers get instant feedback on code changes before submitting a pull request. Paste a code diff or provide a GitHub PR URL, and the AI analyzes it for bugs, naming issues, performance hints, security flags, and best practice violations.

### Features
- Submit code diffs for instant AI analysis
- Findings categorized: Bug, Naming, Performance, Security, Code Style, Best Practice
- Severity levels: Critical (red), Warning (orange), Info (blue)
- Confidence scores on each finding
- Inline suggested fixes with before/after preview
- Filter results by category and severity
- Review history with admin management
- Two layouts: Main + Admin

### Architecture
- **Frontend:** React 18 + TypeScript (strict) + Vite + React Router v6
- **Backend:** .NET 9 Minimal API + MediatR + FluentValidation
- **Database:** PostgreSQL 16 (Dockerized)
- **AI:** Mock analysis service (with optional OpenAI integration)

## Getting Started

### Prerequisites
- Node.js 18+
- .NET 9 SDK
- Docker + Docker Compose

### Run with Docker (recommended)
```bash
docker compose up -d
```
- Frontend: http://localhost:3000
- Backend API: http://localhost:8080/swagger
- PostgreSQL: localhost:5432

### Run locally

**Backend:**
```bash
cd backend/src/PRReviewAssistant.API
dotnet restore
dotnet ef database update
dotnet run
```
API: https://localhost:5001/swagger

**Frontend:**
```bash
cd frontend
npm install
npm run dev
```
UI: http://localhost:5173

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/v1/reviews | Submit code for AI review |
| GET | /api/v1/reviews | List reviews (paginated) |
| GET | /api/v1/reviews/{id} | Get review with findings |
| DELETE | /api/v1/reviews/{id} | Delete a review |
| GET | /api/v1/settings | Get app settings |
| PUT | /api/v1/settings | Update settings |

## Documentation

- [Prompt Logs](prompts/) — Prompt history (per-file: `{N}_{short-key}.md`)
- [Insights](docs/INSIGHTS.md) — Observations on AI-assisted development
- [AI Workflow](docs/ai-workflow.md) — Development standard

## Tools & Models Used

| Tool | Usage |
|------|-------|
| GitHub Copilot (VS Code) | Primary code generation (Agent mode) |
| Claude Opus 4.6 | Model for complex generation |
| Docker + Docker Compose | Containerized development |
| GitHub Actions | CI pipeline |

## AI Generation Stats

| Metric | Value |
|--------|-------|
| Total files | {N} |
| AI-generated | {N} ({X}%) |
| Manually edited | {N} ({Y}%) |
| Fully manual | {N} ({Z}%) |
| **Overall AI %** | **{X}%** |

## License

MIT
```

---

## Template 2: Prompt Log File (`{N}_{short-key}.md`)

```markdown
# Prompt Log — PR Review Assistant

This document records all key prompts used during AI-assisted development of the PR Review Assistant.

**Developer:** {Name}
**Period:** {Start Date} – {End Date}
**Primary Tool:** GitHub Copilot (VS Code, Claude Opus 4.6)

---

## Phase 1: Foundation & Scaffolding

### Step 1: Project Scaffold

**Task ID:** BE-01, FE-01
**Prompt:**
> {exact prompt}

**Result:**
{1-3 sentence summary}

**Accepted/Modified:**
- Accepted: {what}
- Modified: {what and why}

**Tool:** GitHub Copilot (VS Code)
**Files:** {list}

---

### Step 2: {Title}

**Task ID:** {ID}
**Prompt:**
> {exact prompt}

**Result:**
{summary}

**Accepted/Modified:**
- {details}

**Tool:** {tool}
**Files:** {list}

---

## Phase 2: Backend Development

### Step N: {Title}

**Task ID:** {ID}
**Prompt:**
> {exact prompt}

**Result:**
{summary}

**Accepted/Modified:**
- {details}

**Tool:** {tool}
**Files:** {list}

---

## Phase 3: Frontend Development

### Step N: {Title}
...

---

## Phase 4: Integration & Polish

### Step N: {Title}
...

---

## Phase 5: Infrastructure

### Step N: {Title}
...

---

## Phase 6: Documentation

### Step N: {Title}
...

---

## Summary

| Phase | Steps | AI-Generated Files | Manual Edits |
|-------|-------|--------------------|-------------|
| Foundation | {N} | {N} | {N} |
| Backend | {N} | {N} | {N} |
| Frontend | {N} | {N} | {N} |
| Integration | {N} | {N} | {N} |
| Infrastructure | {N} | {N} | {N} |
| Documentation | {N} | {N} | {N} |
| **Total** | **{N}** | **{N}** | **{N}** |
```

---

## Template 3: INSIGHTS.md

```markdown
# Insights — AI-Assisted Development of PR Review Assistant

## Prompts That Worked Well

1. **{Pattern name}**
   - Prompt: "{brief example}"
   - Why it worked: {explanation}

2. **{Pattern name}**
   - Prompt: "{brief example}"
   - Why it worked: {explanation}

3. **{Pattern name}**
   - Prompt: "{brief example}"
   - Why it worked: {explanation}

## Prompts That Did Not Work

1. **{Pattern name}**
   - Prompt: "{brief example}"
   - What went wrong: {explanation}
   - How I fixed it: {explanation}

2. **{Pattern name}**
   - Prompt: "{brief example}"
   - What went wrong: {explanation}
   - How I fixed it: {explanation}

## Effective Prompting Patterns

### Pattern 1: Architecture First, Implementation Second
{Description of the pattern and why it works}

### Pattern 2: Reference Existing Code in Every Prompt
{Description}

### Pattern 3: One File Per Prompt
{Description}

### Pattern 4: Plan Mode Before Agent Mode
{Description}

### Pattern 5: Provide Interface When Requesting Implementation
{Description}

## What I Would Do Differently

1. {Observation}
2. {Observation}
3. {Observation}

## Recommendations for Future AI-Assisted Projects

1. {Recommendation}
2. {Recommendation}
3. {Recommendation}
4. {Recommendation}

## Key Metrics

| Metric | Value |
|--------|-------|
| Total development time | {X} hours |
| AI-generated code percentage | {X}% |
| Number of prompts used | {N} |
| Average attempts per task | {N} |
| Tasks completed with 1 prompt | {N}% |
| Tasks requiring 2+ prompts | {N}% |

## Tools Assessment

| Tool | Rating | Strengths | Weaknesses |
|------|--------|-----------|-----------|
| GitHub Copilot (Agent) | {1-5}/5 | {strengths} | {weaknesses} |
| {Other tool} | {1-5}/5 | {strengths} | {weaknesses} |

## Conclusion

{2-3 sentence overall conclusion about AI-assisted development}
```

---

## Template 4: AI Generation Tracking Table

Use this table to track AI generation percentage per file. Include in the prompt log file or as a separate file.

```markdown
# AI Generation Tracking

| # | File Path | Generated By | Prompt Step | Manual Lines | Total Lines | AI % |
|---|-----------|-------------|-------------|-------------|-------------|------|
| 1 | backend/src/.../Entities/Review.cs | Copilot | Step 3 | 0 | 25 | 100% |
| 2 | backend/src/.../Program.cs | Copilot + Manual | Step 9 | 5 | 60 | 92% |
| 3 | frontend/src/pages/HomePage.tsx | Copilot | Step 12 | 0 | 40 | 100% |
| ... | ... | ... | ... | ... | ... | ... |

**Overall:** {Total AI lines} / {Total lines} = **{X}%**
```
