---
name: my-generateFrontendArtifact
description: Generate a React TypeScript component, page, or custom hook for the frontend.
agent: agent
---

# Generate Frontend Artifact — AI-ED-FTG16

**Role:** Senior Frontend Engineer (React 18 + TypeScript strict)  
**Target:** `${input:ARTIFACT_TYPE:Choose — Component | Page | Hook | Layout}`  
**Name:** `${input:NAME:Enter the PascalCase component/hook name (e.g. FindingCard, useReviews)}`

---

## Pre-Conditions

1. Read `.github/copilot-instructions.md` for naming, TypeScript, and React conventions.
2. Locate existing components, hooks, and services that `${input:NAME}` should compose or call.
3. Confirm `${input:NAME}` does not already exist.

---

## Generation Rules by Artifact Type

### Component

```
Location: src/components/{subfolder}/{Name}.tsx
- Functional component only. No class components.
- Props interface defined immediately above the component declaration (named {Name}Props).
- No any types. Explicit return type: React.ReactElement | null.
- Errors: display via <ErrorMessage> component — never raw alert().
- No data fetching inside a component — delegate to a hook.
- Export: named export only.
```

### Page

```
Location: src/pages/{Name}.tsx
- Route: {describe route}
- Layout wrapper: {MainLayout | AdminLayout} — use <Outlet /> pattern.
- Uses hooks for data: {list hooks}.
- Composes these components: {list components}.
- Handles loading state: show <LoadingSpinner />.
- Handles error state: show <ErrorMessage />.
- Error boundary must wrap this page at the router level.
- No inline data fetching.
```

### Custom Hook

```
Location: src/hooks/{useName}.ts
- Name must be prefixed with use.
- Purpose: {describe state/data managed}.
- Uses API service from services/api.ts.
- Return type (explicit):
  { data: T | null; loading: boolean; error: string | null; ...actions }
- Errors normalized at the API call boundary.
- No any types. No direct DOM access.
```

### Layout

```
Location: src/layouts/{Name}.tsx
- Uses React Router <Outlet /> for child route rendering.
- Contains nav/header/sidebar — no page-specific logic.
- Props: none (layout wraps via Router config, not props).
- Named export.
```

---

## Step 1 — Output Plan (No Code)

List:
- File path
- Props interface (if component)
- Return type (if hook)
- Data sources (which hooks / API calls)
- Components composed
- State managed locally
- Risk areas (loading race conditions, missing null checks, etc.)

**STOP. Wait for explicit approval.**

---

## Step 2 — Generate

After approval:

- Implement `${input:NAME}` according to the type-specific rules above.
- No `any`. No `console.log`. No hardcoded strings (use constants or props).
- Co-locate test file: `{Name}.test.tsx` or `{useName}.test.ts`.

---

## Step 3 — Generate Tests (If P0)

```
Framework: Jest + React Testing Library
Scenarios to cover:
  - Renders without crash
  - Displays data correctly when loaded
  - Shows <LoadingSpinner /> while loading
  - Shows <ErrorMessage /> on error
  - (Hook) returns correct state on API success
  - (Hook) returns error string on API failure
```

---

## Step 4 — Deliverables

- [ ] Component/hook file in correct location
- [ ] Props/return type explicitly defined
- [ ] No `any`, no TODO, no debug code
- [ ] Test file created for critical paths
- [ ] Prompt logged: `{N}_generate-{name-kebab}.md` in `prompts/`
