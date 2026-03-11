## Task / Issue

Closes #<!-- issue number -->  
Task ID: <!-- e.g. FTG-42 -->

---

## Changes

<!-- List every meaningful change. One bullet per file/component/module. -->

-
-
-

---

## Type of Change

- [ ] Bug fix (non-breaking)
- [ ] New feature (non-breaking)
- [ ] Breaking change (existing functionality affected)
- [ ] Refactor (no behavior change)
- [ ] Infrastructure / CI
- [ ] Documentation only

---

## Test Coverage

<!-- Describe what was tested and how. -->

- [ ] Unit tests added / updated
- [ ] Integration tests added / updated
- [ ] Manual testing performed (describe scenario below)

**Manual test scenario:**
<!-- If tested manually, describe steps to reproduce. -->

---

## Validation Checklist

All boxes must be checked before requesting review.

### Code Quality
- [ ] No `any` in TypeScript
- [ ] No duplicated logic
- [ ] No unused imports or variables
- [ ] No `console.log`, `debugger`, or TODO left in code

### Build & Tests
- [ ] `npm run build` passes (frontend)
- [ ] `npm test` passes (frontend)
- [ ] `dotnet build` passes (backend)
- [ ] `dotnet test` passes (backend)

### Docker (if applicable)
- [ ] `docker compose build` succeeds
- [ ] `docker compose up -d` starts all services healthy
- [ ] `/health` endpoint responds

### Documentation
- [ ] README or relevant docs updated
- [ ] `.env.example` updated (if new environment variables added)
- [ ] API documentation updated (if contracts changed)

### Security
- [ ] No secrets or credentials committed
- [ ] New dependencies reviewed and justified
- [ ] AI-generated code manually reviewed

---

## Breaking Changes

- None  
<!-- OR describe what breaks and the migration path -->

---

## Summary for Reviewer

<!-- One paragraph: what this PR does, why it was done this way, and what to focus on during review. -->
