# PR Review Assistant — Software Requirements Specification (SRS)

---

## 1. Introduction

### 1.1 Purpose
This document defines the complete functional and non-functional requirements for the **PR Review Assistant** application. It serves as the primary reference for AI-assisted code generation and human review.

### 1.2 Scope
PR Review Assistant is a web application consisting of a React frontend and .NET 9 Web API backend. It accepts code diffs (pasted or fetched from GitHub), sends them through an AI analysis pipeline, and returns structured, categorized review findings.

### 1.3 Definitions

| Term | Definition |
|------|-----------|
| Diff | A text representation of changes between two versions of code |
| Review | A single code analysis session producing a set of findings |
| Finding | An individual issue, suggestion, or observation from AI analysis |
| Severity | Importance level: Critical, Warning, Info |
| Category | Classification: Bug, Naming, Performance, Security, CodeStyle, BestPractice |
| Confidence | AI's self-assessed reliability of a finding (0–100%) |

### 1.4 References
- [1-project-overview.md](1-project-overview.md) — Architecture and stack
- [3-backlog-tasks.md](3-backlog-tasks.md) — Task breakdown
- [4-ai-generation-rules.md](4-ai-generation-rules.md) — Copilot generation rules
- `.github/copilot-instructions.md` — Copilot behavioral instructions
- `docs/ai-workflow.md` — Deterministic development workflow

---

## 2. Overall Description

### 2.1 Product Perspective
PR Review Assistant is a standalone web application. It does not replace human code review — it augments it by catching common issues before a PR reaches a reviewer.

### 2.2 User Classes

| User Class | Description | Access |
|-----------|-------------|--------|
| Developer | Submits code for review, views results | Main pages |
| Admin | Manages review history and settings | Admin pages |

Note: No authentication for MVP. User class distinction is layout/routing-based only.

### 2.3 Operating Environment
- Frontend: Modern browsers (Chrome, Firefox, Edge)
- Backend: .NET 9 runtime
- Database: PostgreSQL 16 (Dockerized)
- Development: Docker Compose for full-stack local setup

### 2.4 Assumptions
- Users have access to code diffs in text format
- Mock AI service is sufficient for MVP demonstration
- No concurrent multi-user scenarios need handling for MVP
- PostgreSQL runs locally via Docker

### 2.5 Constraints
- Development time: ~4 hours total (2 days × 2 hrs)
- 90%+ code must be AI-generated
- Stack: React (Vite + TS) + .NET 9 + PostgreSQL + Docker
- Must follow conventions in `.github/copilot-instructions.md`

---

## 3. Functional Requirements

### 3.1 Code Review Input

| ID | Requirement | Priority |
|----|------------|----------|
| FR-RI-01 | User shall paste a code diff as plain text into a multiline input | P0 |
| FR-RI-02 | User shall optionally enter a GitHub PR URL | P2 |
| FR-RI-03 | System shall validate that code diff is not empty before submission | P0 |
| FR-RI-04 | User shall select the programming language from a dropdown | P0 |
| FR-RI-05 | System shall display a loading indicator during AI analysis | P0 |
| FR-RI-06 | Supported languages: C#, TypeScript, JavaScript, Python, Java, Go, Other | P0 |

### 3.2 AI Code Analysis

| ID | Requirement | Priority |
|----|------------|----------|
| FR-AI-01 | System shall send code diff + language to IAiAnalysisService | P0 |
| FR-AI-02 | Service shall return a list of findings | P0 |
| FR-AI-03 | Each finding shall include: category, severity, title, description, lineReference (nullable), suggestion, confidence (0–100), suggestedFix (nullable) | P0 |
| FR-AI-04 | Finding categories: Bug, Naming, Performance, Security, CodeStyle, BestPractice | P0 |
| FR-AI-05 | Finding severities: Critical, Warning, Info | P0 |
| FR-AI-06 | System shall handle AI service errors and return ProblemDetails | P0 |
| FR-AI-07 | Mock AI service shall return realistic, varied findings based on input length and language | P0 |
| FR-AI-08 | If GitHub PR URL is provided, system shall fetch diff via GitHub API before analysis | P2 |

### 3.3 Review Results Display

| ID | Requirement | Priority |
|----|------------|----------|
| FR-RD-01 | System shall display findings grouped by category | P0 |
| FR-RD-02 | Each finding shall be color-coded by severity (red=Critical, orange=Warning, blue=Info) | P0 |
| FR-RD-03 | User shall filter findings by severity | P0 |
| FR-RD-04 | User shall filter findings by category | P0 |
| FR-RD-05 | System shall display summary: total findings, count per severity, average confidence | P0 |
| FR-RD-06 | System shall display the original diff in a code block alongside findings | P1 |
| FR-RD-07 | Each finding with a suggestedFix shall show a collapsible "Suggested Fix" panel | P1 |
| FR-RD-08 | Confidence score shall be displayed as a percentage badge on each finding | P1 |

### 3.4 Review History

| ID | Requirement | Priority |
|----|------------|----------|
| FR-RH-01 | System shall persist each review: id, codeDiff, language, prUrl, createdAt, findings | P0 |
| FR-RH-02 | User shall view a paginated list of past reviews | P0 |
| FR-RH-03 | Each history item shows: date, language, snippet preview, finding counts by severity | P0 |
| FR-RH-04 | User shall click a review to navigate to its full result page | P0 |
| FR-RH-05 | Reviews sorted by date (newest first) by default | P0 |

### 3.5 Admin Features

| ID | Requirement | Priority |
|----|------------|----------|
| FR-AD-01 | Admin view shall list all reviews with management options | P0 |
| FR-AD-02 | Admin shall delete individual reviews | P0 |
| FR-AD-03 | Admin settings page shall allow toggling mock AI mode on/off | P1 |
| FR-AD-04 | Admin settings page shall display application stats (total reviews, total findings) | P1 |

### 3.6 Navigation & Routing

| ID | Requirement | Priority |
|----|------------|----------|
| FR-NR-01 | Application shall use client-side routing (React Router v6) | P0 |
| FR-NR-02 | Route `/` → HomePage (MainLayout) | P0 |
| FR-NR-03 | Route `/review` → ReviewPage (MainLayout) | P0 |
| FR-NR-04 | Route `/review/:id` → ReviewResultPage (MainLayout) | P0 |
| FR-NR-05 | Route `/admin/history` → HistoryPage (AdminLayout) | P0 |
| FR-NR-06 | Route `/admin/settings` → AdminSettingsPage (AdminLayout) | P1 |
| FR-NR-07 | Unknown routes → redirect to `/` | P0 |
| FR-NR-08 | Active navigation link shall be visually highlighted | P1 |

---

## 4. Non-Functional Requirements

### 4.1 Performance

| ID | Requirement |
|----|------------|
| NFR-P-01 | Frontend initial load: under 3 seconds |
| NFR-P-02 | API responses (non-AI): under 500ms |
| NFR-P-03 | Mock AI analysis: under 2 seconds |
| NFR-P-04 | History page pagination: 20 items per page |

### 4.2 Usability

| ID | Requirement |
|----|------------|
| NFR-U-01 | Desktop-first responsive design |
| NFR-U-02 | Navigation with visible active state |
| NFR-U-03 | Clear, actionable error messages (no raw exceptions) |
| NFR-U-04 | Loading states for all async operations |

### 4.3 Reliability

| ID | Requirement |
|----|------------|
| NFR-R-01 | Application shall handle API failures with user-friendly error display |
| NFR-R-02 | Application shall not crash on invalid or malicious input |
| NFR-R-03 | Error boundaries on every top-level route |

### 4.4 Maintainability

| ID | Requirement |
|----|------------|
| NFR-M-01 | Layered architecture with clear separation of concerns |
| NFR-M-02 | MediatR for all business logic (no logic in endpoints) |
| NFR-M-03 | Repository pattern for data access |
| NFR-M-04 | Dependency Injection throughout |
| NFR-M-05 | Self-documenting code (no unnecessary comments) |
| NFR-M-06 | Files under 150 lines where possible |

### 4.5 Security

| ID | Requirement |
|----|------------|
| NFR-S-01 | No secrets in source code |
| NFR-S-02 | ProblemDetails responses never expose stack traces |
| NFR-S-03 | Input validation on all API endpoints |
| NFR-S-04 | CORS configured for frontend origin only |

### 4.6 AI Generation

| ID | Requirement |
|----|------------|
| NFR-AI-01 | At least 90% of code AI-generated |
| NFR-AI-02 | All prompts documented in `prompts/` folder using `{N}_{short-key}.md` per-file naming convention |
| NFR-AI-03 | Manual edits marked with `// MANUAL EDIT: <reason>` |

---

## 5. System Features (Detailed)

### 5.1 Feature: Submit Code for Review

**Description:** User submits a code diff for AI analysis and receives structured findings.

**Stimulus:** User clicks "Analyze" on the Review page.

**Flow:**
1. Frontend validates input (non-empty diff, language selected)
2. Frontend sends `POST /api/v1/reviews` with `CreateReviewRequest`
3. Backend validates via FluentValidation
4. Backend calls `IAiAnalysisService.AnalyzeAsync()`
5. Backend persists Review + Findings
6. Backend returns `ReviewDetailResponse`
7. Frontend navigates to `/review/:id` and displays results

**Error Handling:**
- Empty input → frontend validation error
- Validation failure → `400 Bad Request` with field-level ProblemDetails
- AI service failure → `500 Internal Server Error` with generic ProblemDetails
- Network error → frontend ErrorMessage component

### 5.2 Feature: Browse Review History

**Description:** User browses past reviews with pagination and summary info.

**Stimulus:** User navigates to History page.

**Flow:**
1. Frontend sends `GET /api/v1/reviews?page=1&pageSize=20`
2. Backend queries repository with pagination
3. Returns paginated `ReviewListResponse`
4. User clicks a review → navigates to `/review/:id`

### 5.3 Feature: Admin Delete Review

**Description:** Admin deletes a review and all associated findings.

**Stimulus:** Admin clicks "Delete" on a review in the History page.

**Flow:**
1. Frontend shows confirmation dialog
2. Frontend sends `DELETE /api/v1/reviews/:id`
3. Backend deletes Review + cascade deletes Findings
4. Returns `204 No Content`
5. Frontend refreshes list

---

## 6. External Interface Requirements

### 6.1 API Specification

**Base URL:** `/api/v1`

| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|-------------|----------|
| POST | `/reviews` | Submit code for review | `CreateReviewRequest` | `ApiResponse<ReviewDetailResponse>` |
| GET | `/reviews` | List reviews (paginated) | Query: `page`, `pageSize` | `ApiResponse<PaginatedList<ReviewListItem>>` |
| GET | `/reviews/{id}` | Get review with findings | — | `ApiResponse<ReviewDetailResponse>` |
| DELETE | `/reviews/{id}` | Delete a review | — | `204 No Content` |
| GET | `/settings` | Get app settings | — | `ApiResponse<AppSettingsResponse>` |
| PUT | `/settings` | Update app settings | `UpdateSettingsRequest` | `ApiResponse<AppSettingsResponse>` |

### 6.2 Response Envelope

```json
{
  "data": { ... } | null,
  "error": {
    "type": "string",
    "title": "string",
    "status": 400,
    "detail": "string",
    "errors": { "fieldName": ["error message"] }
  } | null
}
```

### 6.3 Pagination Response

```json
{
  "items": [...],
  "totalCount": 42,
  "page": 1,
  "pageSize": 20
}
```

---

## 7. Data Requirements

### 7.1 Entities

#### Review

| Field | Type | Constraints | Description |
|-------|------|------------|-------------|
| Id | string (ULID) | PK | Unique identifier |
| CodeDiff | string | Required, max 50000 chars | Full diff text |
| Language | string | Required | Programming language |
| PrUrl | string | Nullable | Optional GitHub PR URL |
| CreatedAt | DateTime | Required, UTC | Timestamp |
| Findings | List\<Finding\> | Navigation | Related findings |

#### Finding

| Field | Type | Constraints | Description |
|-------|------|------------|-------------|
| Id | string (ULID) | PK | Unique identifier |
| ReviewId | string | FK → Review | Parent review |
| Category | FindingCategory | Required | Bug, Naming, Performance, Security, CodeStyle, BestPractice |
| Severity | FindingSeverity | Required | Critical, Warning, Info |
| Title | string | Required, max 200 chars | Short title |
| Description | string | Required, max 2000 chars | Detailed description |
| LineReference | string | Nullable | Line number or range |
| Suggestion | string | Required, max 2000 chars | Recommended action |
| Confidence | int | 0–100 | AI confidence score |
| SuggestedFix | string | Nullable, max 5000 chars | Inline code fix suggestion |

#### AppSettings

| Field | Type | Constraints | Description |
|-------|------|------------|-------------|
| Id | string | PK | Singleton key |
| UseMockAi | bool | Default: true | Toggle mock/real AI |
| AiModel | string | Default: "mock" | Model identifier |

### 7.2 Enums

```
FindingCategory: Bug, Naming, Performance, Security, CodeStyle, BestPractice
FindingSeverity: Critical, Warning, Info
```

### 7.3 DTOs (C# Records)

```csharp
// Requests
record CreateReviewRequest(string CodeDiff, string Language, string? PrUrl);
record UpdateSettingsRequest(bool UseMockAi, string AiModel);

// Responses
record ReviewDetailResponse(
    string Id, DateTime CreatedAt, string Language, string CodeDiff,
    string? PrUrl, ReviewSummaryDto Summary, IReadOnlyList<FindingDto> Findings);

record ReviewListItem(
    string Id, DateTime CreatedAt, string Language, string CodeSnippet,
    int TotalFindings, int CriticalCount, int WarningCount, int InfoCount);

record FindingDto(
    string Id, string Category, string Severity, string Title,
    string Description, string? LineReference, string Suggestion,
    int Confidence, string? SuggestedFix);

record ReviewSummaryDto(
    int TotalFindings, int CriticalCount, int WarningCount,
    int InfoCount, double AverageConfidence);

record AppSettingsResponse(bool UseMockAi, string AiModel);

// Pagination
record PaginatedList<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
```

### 7.4 TypeScript Interfaces

```typescript
interface ReviewDetailResponse {
  id: string;
  createdAt: string;
  language: string;
  codeDiff: string;
  prUrl: string | null;
  summary: ReviewSummary;
  findings: Finding[];
}

interface ReviewListItem {
  id: string;
  createdAt: string;
  language: string;
  codeSnippet: string;
  totalFindings: number;
  criticalCount: number;
  warningCount: number;
  infoCount: number;
}

interface Finding {
  id: string;
  category: FindingCategory;
  severity: FindingSeverity;
  title: string;
  description: string;
  lineReference: string | null;
  suggestion: string;
  confidence: number;
  suggestedFix: string | null;
}

interface ReviewSummary {
  totalFindings: number;
  criticalCount: number;
  warningCount: number;
  infoCount: number;
  averageConfidence: number;
}

interface PaginatedList<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

type FindingCategory = 'Bug' | 'Naming' | 'Performance' | 'Security' | 'CodeStyle' | 'BestPractice';
type FindingSeverity = 'Critical' | 'Warning' | 'Info';
```

---

## 8. Assumptions & Constraints

### Assumptions
1. Developer has Node.js 18+, .NET 9 SDK, and Docker installed
2. No real OpenAI API key is required for MVP (mock service)
3. Single-user scenario (no concurrent review submissions)
4. PostgreSQL data is not backed up (development only)
5. Code diffs are pasted as plain text (unified diff format preferred but not enforced)

### Constraints
1. Total development time: ~4 hours
2. 90%+ code must be AI-generated and documented
3. Must follow `.github/copilot-instructions.md` conventions
4. Must follow MediatR + FluentValidation + Repository pattern
5. No `any` in TypeScript, strict mode enforced
6. ProblemDetails (RFC 7807) for all API errors
7. IDs as strings (ULID) — never expose raw integer database IDs
8. Timestamps: ISO 8601 UTC
9. JSON field names: camelCase
