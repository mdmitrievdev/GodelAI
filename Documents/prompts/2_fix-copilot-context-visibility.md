# Prompt Log — 2_fix-copilot-context-visibility

---

## Metadata

| Field | Value |
|-------|-------|
| **Number** | 2 |
| **Key** | fix-copilot-context-visibility |
| **Date** | 2026-03-04 |
| **Mode** | Agent |
| **Model** | GitHub Copilot (Claude Sonnet 4.6) |
| **Task ref** | Infrastructure — make project files visible to Copilot |

---

## Context Provided

- `.github/copilot-instructions.md` — existing global Copilot system prompt
- `.github/instructions/ai-generation.instructions.md` — existing instruction file
- `.github/instructions/prompt-logging.instructions.md` — existing instruction file
- `project-requarements/1-project-overview.md` — architecture, folder structure, tech stack, success criteria
- `project-requarements/2-functional-requirements-srs.md` — FR-* and NFR-* requirement IDs (full SRS)
- `project-requarements/3-backlog-tasks.md` — BE-*, FE-*, INF-*, DOC-* task IDs
- `project-requarements/4-ai-generation-rules.md` — AI generation strategy
- `docs/ai-workflow.md` — deterministic development workflow (all 11 sections)

---

## Prompt

Fix two visibility problems identified in the project file audit:

**Problem 1:** `project-requarements/` (5 SRS files) is invisible to Copilot — not in `.github/instructions/` and has no `applyTo` frontmatter.

**Problem 2:** `docs/ai-workflow.md` is referenced in `copilot-instructions.md` but Copilot will never auto-read it.

**Solution:** Create two new `.github/instructions/` files with `applyTo: "**"` that surface the key content, avoiding duplication with what's already in `copilot-instructions.md`.

---

## Response Summary

- Created `.github/instructions/project-requirements.instructions.md` containing:
  - All FR-* functional requirements (sections 3.1–3.6)
  - All NFR-* non-functional requirements (sections 4.1–4.6)
  - Full backlog task ID tables (BE-*, FE-*, INF-*, DOC-*) with phase/priority
  - Detailed folder structure (component subdirectory breakdown)
  - Technology stack with exact versions
  - Success criteria checklist

- Created `.github/instructions/ai-workflow.instructions.md` containing:
  - Reusable prompt files reference table with paths
  - Code generation phase order (phases 1–10)
  - AI code review checklist (TypeScript, C#, tests)
  - Manual vs AI code rules + escalation rule
  - Git worktree workflow (unique to `ai-workflow.md`, not in `copilot-instructions.md`)
  - Context & cost control rules
  - Self-validation checklist
  - Post-completion bookkeeping steps (plan file, prompt log, README)

---

## Files Created / Modified

| File | Action | Notes |
|------|--------|-------|
| `.github/instructions/project-requirements.instructions.md` | Created | 14.2 KB — FR/NFR IDs, task IDs, folder structure, tech stack |
| `.github/instructions/ai-workflow.instructions.md` | Created | 7.1 KB — workflow, checklists, git worktree, bookkeeping |

---

## Outcome

Accepted — both files created with `applyTo: "**"` frontmatter. Copilot will now auto-inject all 4 instruction files on every chat.

## Notes

- Content was curated to avoid duplication with `copilot-instructions.md` (domain entities, DTOs, TypeScript interfaces, API endpoints, conventions — all already in `copilot-instructions.md`)
- Git worktree section and slash commands are unique to `ai-workflow.md` and surfaced in the new file
- `project-requarements/` folder remains the source of truth; instruction file is a curated projection
