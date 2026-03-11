---
applyTo: "**"
name: my-GenerateBackendContract
description: Generate backend domain entity, enum, DTO, or service interface for the .NET 9 backend.
agent: agent
---

# Generate Backend Contract — AI-ED-FTG16

**Role:** Senior .NET Engineer  
**Target:** `${input:TARGET_TYPE:Choose type — Entity | Enum | DTO | Interface | Repository | Validator}`  
**Name:** `${input:NAME:Enter the class/record/interface name}`

---

## Pre-Conditions

1. Read `.github/copilot-instructions.md` for naming and architecture conventions.
2. Locate existing related files (entities, interfaces, DTOs) to ensure consistency.
3. Check that `${input:NAME}` does not already exist — if it does, stop and report.

---

## Generation Rules by Type

### Entity

```
Location: Domain/Entities/{Name}.cs
- Use string (ULID) for Id property.
- Use DateTime (UTC) for all timestamp fields.
- Navigation properties where appropriate (no EF attributes in domain — use Fluent API config).
- No framework dependencies in domain layer.
```

### Enum

```
Location: Domain/Enums/{Name}.cs
- Simple C# enum.
- One value per line.
- Add XML doc comment on the enum and each value.
```

### DTO (C# Record)

```
Location: Shared/DTOs/{Name}.cs
- Use record syntax.
- All fields immutable.
- Use IReadOnlyList<T> for collections.
- Nullable reference types where value is optional.
- API contract: camelCase JSON serialization, string IDs, ISO 8601 timestamps.
```

### Service Interface

```
Location: Domain/Interfaces/{IName}.cs  (or Infrastructure/Services/)
- Use IPascalCase naming.
- All methods async with CancellationToken parameter.
- Return Task<IReadOnlyList<T>> for collections.
- XML doc comments on interface and each method.
```

### Repository Interface + Implementation

```
Interface location: Domain/Interfaces/{INameRepository}.cs
Implementation location: Infrastructure/Repositories/{NameRepository}.cs
- Interface: async methods with CancellationToken.
- Implementation: inject AppDbContext, use EF Core AsNoTracking for queries.
- No raw SQL — use LINQ only.
- Return IReadOnlyList<T> from query methods.
```

### FluentValidation Validator

```
Location: Features/{Domain}/Validators/{RequestType}Validator.cs
- Inherit AbstractValidator<{RequestType}>.
- One rule chain per property.
- No business logic — shape/range/format validation only.
- Use WithMessage() for every rule.
```

---

## Step 1 — Output Plan (No Code)

List:
- File path
- Public API (properties, methods, signatures)
- Any dependencies on existing types
- Risk: naming clash, missing enum value, etc.

**STOP. Wait for explicit approval.**

---

## Step 2 — Generate

After approval:

- Implement `${input:NAME}` according to the rules above.
- Follow all conventions from `.github/copilot-instructions.md`.
- Add XML doc comments on public members.
- Keep file under 150 lines where possible.

---

## Step 3 — Deliverables

- [ ] Generated file in correct location
- [ ] Compiles without errors
- [ ] No `any`, no hardcoded values, no TODO
- [ ] Prompt logged: `{N}_generate-{name-kebab}.md` in `prompts/`
