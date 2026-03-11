// --- Enums (string unions) ---

export type FindingCategory =
  | 'Bug'
  | 'Naming'
  | 'Performance'
  | 'Security'
  | 'CodeStyle'
  | 'BestPractice';

export type FindingSeverity = 'Critical' | 'Warning' | 'Info';

// --- Response interfaces ---

export interface ReviewDetailResponse {
  id: string;
  createdAt: string;
  language: string;
  codeDiff: string;
  prUrl: string | null;
  summary: ReviewSummary;
  findings: Finding[];
}

export interface ReviewListItem {
  id: string;
  createdAt: string;
  language: string;
  codeSnippet: string;
  totalFindings: number;
  criticalCount: number;
  warningCount: number;
  infoCount: number;
}

export interface Finding {
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

export interface ReviewSummary {
  totalFindings: number;
  criticalCount: number;
  warningCount: number;
  infoCount: number;
  averageConfidence: number;
}

export interface AppSettingsResponse {
  useMockAi: boolean;
  aiModel: string;
}

export interface PaginatedList<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// --- Request interfaces ---

export interface CreateReviewRequest {
  codeDiff: string;
  language: string;
  prUrl?: string | null;
}

export interface UpdateSettingsRequest {
  useMockAi: boolean;
  aiModel: string;
}

// --- API wrapper ---

export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  errors?: Record<string, string[]>;
}

export interface ApiResponse<T> {
  data: T | null;
  error: ProblemDetails | null;
}

// --- Normalized error ---

export interface ApiError {
  message: string;
  code?: string;
}
