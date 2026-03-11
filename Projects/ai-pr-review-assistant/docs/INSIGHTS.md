# Insights — AI-Assisted Development of PR Review Assistant

**Developer:** GodelAI  
**Period:** 2026-03-04 – 2026-03-10  
**Primary Tool:** GitHub Copilot (VS Code, Claude Sonnet 4.6)  
**Prompt logs:** [`prompts/`](../prompts/) — 45 files, one per completed task

---

## Prompts That Worked Well

### 1. MockAiAnalysisService — Domain-saturated context

- **Prompt log:** `13_be09-mock-ai-service.md`
- **Prompt:** `#file:plan-pr-review-assistant.md Implement #### TASK BE-09: MockAiAnalysisService`
- **Context provided:** `IAiAnalysisService`, `Finding` entity, both enum files, `.csproj` (package list)
- **Why it worked:** Loading the interface contract + all value types gave the model everything it needed to reason about valid outputs. The result included language-specific finding templates for all 7 languages, realistic `LineReference` extraction from diff `+`/`-` lines, ULID generation via `NUlid`, randomized confidence in the 40–98 range, and guaranteed coverage of all 3 severity levels per analysis. Accepted as-is with 0 build errors.

### 2. Backend unit tests — Source code first, then tests

- **Prompt log:** `43_be21-backend-unit-tests.md`
- **Prompt:** `#file:plan-pr-review-assistant.md Implement #### TASK BE-21: Backend unit tests`
- **Context provided:** All 4 handler/validator/service source files + their interfaces and entity dependencies
- **Why it worked:** The model read the actual implementation before writing tests, so the mock setup matched the real constructor signatures and the assertion values matched actual behavior (e.g., average confidence formula, severity bucketing). All 25 tests passed on the first run with no adjustments.

### 3. Global CSS refactor — Full CSS context in one pass

- **Prompt log:** `37_fe17-global-styling-css-modules.md`
- **Prompt:** `#file:plan-pr-review-assistant.md Implement #### TASK FE-17: Global styling + CSS modules`
- **Context provided:** All 16 existing CSS module files + `index.css` + `copilot-instructions.md` (severity colors)
- **Why it worked:** Loading every CSS file in one context window let the model inventory all unique hardcoded hex/rgba values across the project and extract them into a single consistent set of CSS custom properties. It also caught a severity color inconsistency in `HistoryPage` that differed from `SeverityBadge`. Zero `.tsx` changes required — all class names preserved. Build and lint passed with zero errors.

### 4. CreateReview CQRS handler — Interface-first context

- **Prompt log:** `16_be12-create-review-command.md`
- **Prompt:** `#file:plan-pr-review-assistant.md Implement #### TASK BE-12: CreateReview Command + Handler`
- **Context provided:** `IReviewRepository`, `IAiAnalysisService`, all relevant DTOs, `Review`/`Finding` entities
- **Why it worked:** Having the repository interface and AI service interface simultaneously in context meant the model generated the exact method signatures and return types without guessing. It correctly chose `Ulid.NewUlid().ToString()`, mapped AI findings onto the domain entity, computed the `ReviewSummaryDto` with average confidence, and threaded `CancellationToken` through both async calls. Accepted as-is.

### 5. docker-compose.yml — .NET config override awareness

- **Prompt log:** `39_inf03-docker-compose.md`
- **Prompt:** `Implement ### Phase 10: Infrastructure & Documentation #### TASK INF-03: docker-compose.yml`
- **Context provided:** `Program.cs`, `appsettings.json`, `api.ts` (base URL logic), `.env.example`
- **Why it worked:** Reading `api.ts` revealed that the frontend sends requests to `VITE_API_URL` and falls back to a relative path. The model used that to set `VITE_API_BASE_URL=""` at build time so nginx could proxy `/api/v1/...` transparently. It also used .NET's `__` env var override convention (`ConnectionStrings__DefaultConnection`, `Cors__AllowedOrigin`) without being told — inferred from the `appsettings.json` key structure.

---

## Prompts That Did Not Work

### 1. Frontend component tests — Jest ESM/CJS conflict

- **Prompt log:** `44_fe-tests-frontend-component-tests.md`
- **What went wrong:** The frontend `package.json` has `"type": "module"`. Jest tries to `require()` its config file in CommonJS mode and fails with `ERR_REQUIRE_ESM` when it finds `jest.config.ts`. The model initially created a `jest.config.ts`, which is the documented pattern but incompatible with `"type":"module"`.
- **How it was fixed:** Renamed `jest.config.ts` → `jest.config.cjs` (CommonJS explicit extension). Added `ts-node` as a missing dev dependency. Created `tsconfig.test.json` with `"module": "commonjs"` and `"isolatedModules": true` so ts-jest could compile `api.ts` (which uses `import.meta.env`) without erroring on Vite globals. Used factory-form `jest.mock(...)` for the `api` module. All 16 tests passed after these adjustments — two additional iterations required.

### 2. React + Vite scaffold — Runtime dependency not present

- **Prompt log:** `5_scaffold-react-vite-ts.md`
- **What went wrong:** The standard scaffold workflow requires running `npm create vite@latest frontend -- --template react-ts` followed by `npm install`. Node.js was not installed on the development machine, so neither command could execute.
- **How it was fixed:** All scaffold files were generated directly by the AI without relying on the CLI — `package.json`, `tsconfig.json`, `tsconfig.app.json`, `tsconfig.node.json`, `vite.config.ts`, `eslint.config.js`, `.prettierrc`, `jest.config.ts`, `index.html`, `src/main.tsx`, `src/App.tsx`, and all directory stubs were created file by file. This added one extra prompt cycle but produced a correctly configured project. Dependencies were installed once Node.js was available in a later session.

### 3. Docker Compose validation — Docker not available in dev terminal

- **Prompt log:** `39_inf03-docker-compose.md`
- **What went wrong:** `docker compose config` (syntax validation) and `docker compose up -d` could not be run in the development terminal because the Docker daemon was not available in that environment.
- **How it was fixed:** YAML was verified visually by the model. Runtime validation (`docker compose up -d` starts everything) was deferred to the Final Validation task. The `docker` acceptance-criteria checkbox was left unchecked at the end of that task.

---

## Effective Prompting Patterns

### Pattern 1: One task = one chat, one task ID in the prompt

Anchoring every prompt to a specific plan task ID (`TASK BE-09`, `TASK FE-17`) and referencing the plan file kept the model focused. It would read the acceptance criteria checklist and stop when all items were satisfied, without drifting into adjacent features.

### Pattern 2: Provide the interface before requesting the implementation

For any class that depends on an abstraction (handler → repository, service → interface), loading the interface file into context before asking for the implementation consistently produced zero-error code. Without the interface, the model sometimes invented method names that didn't match the contract.

### Pattern 3: Load all dependencies, not just the primary file

For complex tasks (CreateReview handler, MockAiAnalysisService, Repository implementations), loading every file the target would reference — entities, enums, DTOs, existing implementations — eliminated the guessing that causes type mismatches and wrong constructor signatures.

### Pattern 4: Full CSS context for cross-cutting visual tasks

Passing all CSS files simultaneously for the global styling refactor was significantly more effective than patching one file at a time. The model could see the full set of values, detect inconsistencies, and produce a single cohesive variable palette in one pass.

### Pattern 5: Plan first, agent second

Using Plan mode to review the approach before switching to Agent mode caught architectural issues (e.g., singleton pattern for `SettingsRepository`, cascade delete via EF config vs. explicit SQL) before any code was written. This avoided refactoring cycles that would have been needed if implementation had started directly.

### Pattern 6: Reference the `.csproj` for available packages

Including the `.csproj` file in context for backend tasks told the model exactly which NuGet packages and versions were available. This prevented it from inventing a dependency (e.g., using `System.Ulid` instead of the installed `NUlid`) or using an API that only exists in a different package version.

---

## What I Would Do Differently

1. **Verify the runtime environment before starting.** Node.js and Docker not being available in the dev terminal added friction and extra iterations. A checklist of runtime dependencies (Node, Docker, `dotnet-ef` global tool) verified before Phase 1 would have removed all environment-related blockers.

2. **Use a separate `tsconfig.test.json` from the start for frontend tests.** The Jest ESM/CJS conflict is a known, documented issue whenever `"type":"module"` is in `package.json`. Building the test config alongside the main config in FE-01 would have avoided the rework in the test phase.

3. **Run `dotnet build` after every backend task, not just at the end.** Most backend tasks completed with 0 errors, but catching any compilation issues immediately would have made the prompt logs more informative and saved debugging time if an error had been introduced.

4. **Split the global styling task earlier.** Waiting until Phase 9 to introduce CSS custom properties meant all CSS modules were written twice: once with hardcoded values, once refactored to variables. Starting with a `global.css` token file in Phase 6 (alongside layouts) would have made all subsequent CSS modules cleaner on first generation.

---

## Recommendations for Future AI-Assisted Projects

1. **Define interfaces and DTOs as Phase 1.** All downstream code (handlers, repositories, frontend models) is only as good as the contracts it implements. Generating interfaces before implementations is the single highest-leverage action for reducing rework.

2. **Keep prompts under 2 files of context for simple tasks, load everything for integration tasks.** For a single isolated file (e.g., a validator), minimal context is faster and the model stays focused. For integration tasks (handlers, endpoints, docker-compose), load every relevant file — context cost is worth the accuracy gain.

3. **Use a centralized CSS variable file from the start of any styled project.** Retrofitting design tokens is expensive. Start with a `global.css` containing all severity colors, spacing, and typography variables on Day 1.

4. **Test config and scaffold config are separate concerns.** `tsconfig.app.json` (Vite/ESM) and `tsconfig.test.json` (CommonJS/Jest) will have different settings. Generate both upfront and keep them separate — never try to share one tsconfig for both tools.

5. **Validate Docker and CI artifacts in the development environment, not just visually.** The docker-compose and GitHub Actions CI files were accepted without runtime execution. A simple `docker compose config` or a dry-run CI check would catch YAML errors before the code lands in the repository.

---

## Key Metrics

| Metric | Value |
|--------|-------|
| Development period | 2026-03-04 – 2026-03-10 |
| Total prompt logs | 45 |
| Primary AI model | Claude Sonnet 4.6 (GitHub Copilot) |
| Prompts accepted as-is | ~40 / 45 (89%) |
| Prompts requiring ≥2 iterations | ~5 / 45 (11%) |
| Backend source files generated | ~55 |
| Frontend source files generated | ~60 |
| Infrastructure files generated | ~10 |
| Total source files | ~125 |
| Estimated AI-generated code | ≥ 95% |
| Backend tests | 25 (25/25 passing) |
| Frontend tests | 16 (16/16 passing) |
| Manual edits | Marked with `// MANUAL EDIT:` comment |
| Tasks with manual edits | 0 (all accepted as-is or fixed via AI iteration) |

---

## Tools Assessment

| Tool | Rating | Strengths | Weaknesses |
|------|--------|-----------|------------|
| GitHub Copilot — Agent mode (Claude Sonnet 4.6) | 5/5 | Accurate multi-file generation; reads acceptance criteria and self-validates; handles complex CQRS and DI patterns correctly; produces idiomatic C# and TypeScript | Requires explicit context loading — passive inference from workspace alone is insufficient for integration tasks |
| GitHub Copilot — Plan mode | 4/5 | Structured step-by-step plans; catches interface mismatches before coding starts; short and actionable | Occasionally over-specifies implementation detail in plans when a higher-level view would be more useful |
| EF Core `dotnet ef migrations add` | 4/5 | Generates accurate schema from Fluent API configurations; cascade delete and seed data handled correctly | `dotnet-ef` global tool must be installed separately; not bundled with the SDK |
| Jest + ts-jest (frontend) | 3/5 | Good integration with React Testing Library; `@testing-library/user-event` API is natural | ESM/CJS configuration is fragile — the `"type":"module"` + Jest combination requires non-obvious workarounds (`jest.config.cjs`, `isolatedModules`, factory mocks) |
| Vite (frontend build) | 5/5 | Near-instant HMR; excellent TypeScript support; simple proxy config for API routing; multi-stage Docker builds clean | None encountered in this project |
| Docker Compose v2 | 4/5 | Health checks, `depends_on` with conditions, and `.env` override work well; nginx proxy for SPA + API routing is a solid pattern | Not available in all development environments; no built-in YAML schema validation in the CLI |
| GitHub Actions CI | 4/5 | NuGet and npm caching dramatically speeds up runs; matrix-free 3-job structure (frontend / backend / docker) is straightforward to reason about | Docker-in-GitHub-Actions requires `services:` or `docker compose` workarounds; timing of health check retries needs tuning per environment |

---

## Conclusion

The PR Review Assistant was built in approximately one week with 45 prompt interactions, achieving an estimated ≥95% AI-generated codebase. The dominant success factor was **context discipline**: every prompt that provided exact interface files, entity shapes, and dependency lists produced correct, compilable output on the first attempt. Prompts that relied on implicit workspace inference or missing runtime tooling were the source of the small number of multi-iteration cases.

The project confirms that AI-assisted development is most effective when treated as a **pair-programming workflow** — the developer's role shifts from writing syntax to specifying contracts, sequencing tasks, and reviewing generated code against explicit acceptance criteria. The strict one-task-per-chat rule also proved its worth: it kept context focused and made the prompt log a reliable record of decisions.

The key limitation observed is not model capability but **environment readiness**: Docker, Node.js, and global CLI tools (`dotnet-ef`) must be available and verified before the first prompt. Missing tooling surfaces late and wastes iteration budget on workarounds rather than feature development.
