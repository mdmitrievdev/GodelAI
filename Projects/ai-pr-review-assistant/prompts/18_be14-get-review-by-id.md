# Prompt Log — 18_be14-get-review-by-id

| Field | Value |
|-------|-------|
| **Number** | 18 |
| **Key** | be14-get-review-by-id |
| **Date** | 2026-03-10 |
| **Mode** | Agent |
| **Model** | Claude Sonnet 4.6 |
| **Task Ref** | BE-14 |

---

## Context Provided

- `.github/plan-pr-review-assistant.md` — TASK BE-14 acceptance criteria
- `Features/Reviews/Queries/GetReviewsQuery.cs` — existing query pattern
- `Features/Reviews/Queries/GetReviewsQueryHandler.cs` — existing handler pattern
- `Features/Reviews/Commands/CreateReviewCommandHandler.cs` — `MapToResponse` logic
- `Domain/Interfaces/IReviewRepository.cs` — `GetByIdAsync` signature
- `Shared/DTOs/ReviewDetailResponse.cs`, `ReviewSummaryDto.cs`, `FindingDto.cs`

---

## Prompt

Implement TASK BE-14: GetReviewById Query + Handler from the plan file.

---

## Response Summary

- Created `GetReviewByIdQuery.cs` — record implementing `IRequest<ReviewDetailResponse?>` with a single `string Id` parameter.
- Created `GetReviewByIdQueryHandler.cs` — handler that calls `IReviewRepository.GetByIdAsync`, returns `null` for missing reviews, and maps the entity to `ReviewDetailResponse` (including `ReviewSummaryDto` with counts and average confidence) using the same pattern as `CreateReviewCommandHandler.MapToResponse`.
- `dotnet build` passes with 0 errors.

---

## Files Created / Modified

| File | Action |
|------|--------|
| `backend/src/PRReviewAssistant.API/Features/Reviews/Queries/GetReviewByIdQuery.cs` | Created |
| `backend/src/PRReviewAssistant.API/Features/Reviews/Queries/GetReviewByIdQueryHandler.cs` | Created |
| `.github/plan-pr-review-assistant.md` | Updated — BE-14 checkboxes marked `[x]` |

---

## Outcome

Accepted — all acceptance criteria met, build passes.

---

## Notes

None.
