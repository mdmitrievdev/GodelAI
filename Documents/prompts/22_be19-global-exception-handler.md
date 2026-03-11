# Prompt Log — 22_be19-global-exception-handler

| Field         | Value |
|---------------|-------|
| **Number**    | 22 |
| **Key**       | be19-global-exception-handler |
| **Date**      | 2026-03-10 |
| **Mode**      | Agent |
| **Model**     | Claude Sonnet 4.6 |
| **Task Ref**  | BE-19 |

---

## Context Provided

- `.github/plan-pr-review-assistant.md` — TASK BE-19 acceptance criteria
- `.github/copilot-instructions.md` — conventions and architecture rules
- `backend/src/PRReviewAssistant.API/Program.cs` — existing middleware pipeline
- `backend/src/PRReviewAssistant.API/Shared/DTOs/ApiResponse.cs` — existing ProblemDetails usage
- `backend/src/PRReviewAssistant.API/PRReviewAssistant.API.csproj` — installed packages

---

## Prompt

> Implement TASK BE-19: GlobalExceptionHandler
> 
> Acceptance criteria:
> - `ValidationException` → 400 with field errors
> - `KeyNotFoundException` → 404
> - All other → 500 with generic message (no stack trace)
> - Logs error with `ILogger<T>`
> - Implements `IExceptionHandler` or middleware
> 
> Output: `Shared/Middleware/GlobalExceptionHandler.cs`

---

## Response Summary

- Created `Shared/Middleware/GlobalExceptionHandler.cs` implementing `IExceptionHandler` (ASP.NET Core 8+ built-in interface)
- Handles three cases via `switch` expression:
  - `ValidationException` (FluentValidation) → 400 with `errors` dictionary grouped by `PropertyName`
  - `KeyNotFoundException` → 404 ProblemDetails
  - All other exceptions → 500 with generic message, no stack trace exposed
- Logs `Warning` for handled exceptions (400/404), `Error` for unhandled (500)
- Appends `traceId` to `ProblemDetails.Extensions` for correlation
- Registered `GlobalExceptionHandler` in `Program.cs` via `AddExceptionHandler<GlobalExceptionHandler>()` and `AddProblemDetails()`
- Added `app.UseExceptionHandler()` at the top of the middleware pipeline

---

## Files Created / Modified

| File | Action |
|------|--------|
| `backend/src/PRReviewAssistant.API/Shared/Middleware/GlobalExceptionHandler.cs` | Created |
| `backend/src/PRReviewAssistant.API/Program.cs` | Modified — added using, DI registration, middleware |

---

## Outcome

Accepted — `dotnet build` passes with 0 errors.

---

## Notes

- Used `IExceptionHandler` (not custom middleware) as it is the idiomatic ASP.NET Core 8+ approach.
- `AddProblemDetails()` registered alongside to ensure `ProblemDetails` factory is available.
- `app.UseExceptionHandler()` placed first in the pipeline so all downstream exceptions are caught.
