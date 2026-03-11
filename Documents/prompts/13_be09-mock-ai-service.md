# Prompt Log — 13_be09-mock-ai-service

> **File naming convention:** `{IDENTITY_NUMBER}_{SHORT_KEY}.md`

---

## Metadata

| Field | Value |
|-------|-------|
| **Number** | 13 |
| **Key** | be09-mock-ai-service |
| **Date** | 2026-03-10 |
| **Mode** | Agent |
| **Model** | GitHub Copilot (Claude Sonnet 4.6) |
| **Task ref** | BE-09 |

---

## Context Provided

- `.github/plan-pr-review-assistant.md` — TASK BE-09 acceptance criteria
- `.github/copilot-instructions.md` — domain entities, enums, conventions
- `Domain/Interfaces/IAiAnalysisService.cs` — interface definition
- `Domain/Entities/Finding.cs` — entity shape
- `Domain/Enums/FindingCategory.cs`, `FindingSeverity.cs` — enum values
- `PRReviewAssistant.API.csproj` — NUlid v1.7.3 available

---

## Prompt

```
#file:plan-pr-review-assistant.md Implement #### TASK BE-09: MockAiAnalysisService
```

---

## Response Summary

- Created `Infrastructure/Services/MockAiAnalysisService.cs` implementing `IAiAnalysisService`
- Static readonly template lists defined as private `record FindingTemplate(...)` — one generic set (12 templates) + language-specific sets for C#, TypeScript, JavaScript, Python, Java, Go
- `AnalyzeAsync` computes finding count (3–8) proportional to diff line count; shuffles pool; uses `SelectDiverseFindings` to guarantee all 3 severity levels appear
- Delay range exposed as `private const int MinDelayMs = 200; MaxDelayMs = 800;` — easily adjustable
- Changed line numbers extracted from diff (`+`/`-` lines) for realistic `LineReference` values
- `Confidence` randomized in 40–98 range via `Random.Shared.Next(40, 99)`
- ULID IDs generated with `Ulid.NewUlid().ToString()`
- Build verified: `dotnet build` → Build succeeded (0 errors, 0 warnings)

---

## Files Created / Modified

| File | Action |
|------|--------|
| `backend/src/PRReviewAssistant.API/Infrastructure/Services/MockAiAnalysisService.cs` | created |
| `.github/plan-pr-review-assistant.md` | modified — all BE-09 acceptance criteria marked `[x]` |

---

## Outcome

- [x] Accepted as-is
- [ ] Accepted with manual edits (`// MANUAL EDIT:` comments added)
- [ ] Rejected — reason: ___

---

## Notes

- `ReviewId` is intentionally left as `string.Empty` in returned findings — the `CreateReview` command handler will set the FK after saving the Review entity.
- `Review` navigation property uses `null!` suppression in the entity; safe to omit in mock output before EF tracking begins.
- `Random.Shared` used for thread safety (available since .NET 6).
