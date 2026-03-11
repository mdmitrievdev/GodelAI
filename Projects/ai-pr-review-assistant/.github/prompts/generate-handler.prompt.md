---
applyTo: "**"
name: my-GenerateBackendApplicationLogic
description: Generate a MediatR Command/Query+Handler, FluentValidation validator, or Minimal API endpoint map.
agent: agent
---

# Generate Backend Application Logic — AI-ED-FTG16

**Role:** Senior .NET Engineer  
**Target:** `${input:TARGET_TYPE:Choose — Command+Handler | Query+Handler | Validator | EndpointMap | All (handler + validator + endpoint)}`  
**Feature:** `${input:FEATURE_NAME:Enter feature name in PascalCase (e.g. CreateReview, GetReviews, DeleteReview)}`

---

## Pre-Conditions

1. Read `.github/copilot-instructions.md` — MediatR and architecture conventions.
2. Locate the relevant DTO (`${input:FEATURE_NAME}Request`, `${input:FEATURE_NAME}Response`).
3. Locate the relevant repository interface.
4. Confirm handler does not already exist.

---

## Generation Rules

### Command + Handler

```
Location: Features/{Domain}/Commands/{FeatureName}Command.cs
           Features/{Domain}/Commands/{FeatureName}CommandHandler.cs

- Command: record implementing IRequest<TResponse>.
- Handler: class implementing IRequestHandler<TCommand, TResponse>.
- Constructor: inject required services (IRepository, IService) — no service locator.
- CancellationToken: must be passed to all async calls.
- No business logic in endpoints — all logic is here.
- On not found: return null or throw NotFoundException (not ProblemDetails directly).
- On validation failure: throw ValidationException (FluentValidation handles it globally).
- Return: DTO record, not entity.
```

### Query + Handler

```
Location: Features/{Domain}/Queries/{FeatureName}Query.cs
           Features/{Domain}/Queries/{FeatureName}QueryHandler.cs

- Query: record implementing IRequest<TResponse>.
- Handler: class implementing IRequestHandler<TQuery, TResponse>.
- Use repository AsNoTracking pattern for read-only queries.
- Pagination: accept (int Page, int PageSize) and return PaginatedList<T>.
- CancellationToken on all async calls.
- Return: DTO record or PaginatedList<DTO>.
```

### FluentValidation Validator

```
Location: Features/{Domain}/Validators/{FeatureName}RequestValidator.cs

- Inherit AbstractValidator<{FeatureName}Request>.
- One rule chain per property.
- No business logic — shape, range, format validation only.
- WithMessage() on every rule.
- Rules must match SRS field constraints (max length, required, range).
```

### Minimal API Endpoint Map

```
Location: Endpoints/{Domain}Endpoints.cs

- Static class with MapRoutes(WebApplication app) method.
- Each endpoint: app.Map{Method}("{route}", async (...) => ...).
- Each handler: receive IMediator, call await mediator.Send(command, ct).
- Wrap results in ApiResponse<T>{ Data = result, Error = null }.
- On null result: return Results.NotFound(new ApiResponse<T>{ Data = null, Error = ... }).
- Declare route group: app.MapGroup("/api/v1/{domain}").
- No business logic in endpoint handlers.
```

---

## Step 1 — Output Plan (No Code)

List:
- Files to create
- Request/Response types used
- Dependencies injected
- Validation rules to implement
- Error paths (not found, validation failure, service error)
- Test scenarios

**STOP. Wait for explicit approval.**

---

## Step 2 — Generate

After approval, generate all selected targets for `${input:FEATURE_NAME}`:

| Target | File | Status |
|--------|------|--------|
| Command/Query | Features/.../Commands\|Queries/ | [ ] |
| Handler | Features/.../Commands\|Queries/ | [ ] |
| Validator | Features/.../Validators/ | [ ] |
| Endpoint Map | Endpoints/ | [ ] |

---

## Step 3 — Generate Tests

```
Framework: xUnit
Location: tests/{Project}.Tests/Features/{Domain}/

Test scenarios:
  - {FeatureName}Handler_ValidInput_ReturnsExpectedResponse
  - {FeatureName}Handler_NotFound_ReturnsNull
  - {FeatureName}Validator_EmptyField_FailsValidation
  - {FeatureName}Validator_ValidInput_PassesValidation

AAA pattern. One assertion per test.
Mock repositories with Moq or NSubstitute.
```

---

## Step 4 — Deliverables

- [ ] Handler generates correct response
- [ ] Validator rejects invalid input
- [ ] Endpoint calls `mediator.Send()` only
- [ ] Tests pass for critical paths
- [ ] No `any`, no TODO, no hardcoded strings
- [ ] Prompt logged: `{N}_generate-{feature-kebab}.md` in `prompts/`
