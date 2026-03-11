import axios, { AxiosError, AxiosInstance } from 'axios';
import type {
  ApiError,
  ApiResponse,
  AppSettingsResponse,
  CreateReviewRequest,
  PaginatedList,
  ReviewDetailResponse,
  ReviewListItem,
  UpdateSettingsRequest,
} from '../models';

const BASE_URL: string =
  (import.meta.env['VITE_API_BASE_URL'] as string | undefined) ??
  'http://localhost:8080';

const client: AxiosInstance = axios.create({
  baseURL: `${BASE_URL}/api/v1`,
  headers: { 'Content-Type': 'application/json' },
});

function normalizeError(error: unknown): ApiError {
  if (error instanceof AxiosError) {
    const data = error.response?.data as
      | { error?: { detail?: string; title?: string } }
      | undefined;
    const detail = data?.error?.detail ?? data?.error?.title;
    return {
      message: detail ?? error.message ?? 'An unexpected error occurred.',
      code: String(error.response?.status ?? ''),
    };
  }
  if (error instanceof Error) {
    return { message: error.message };
  }
  return { message: 'An unexpected error occurred.' };
}

// --- Reviews ---

export async function createReview(
  request: CreateReviewRequest,
): Promise<ReviewDetailResponse> {
  try {
    const response =
      await client.post<ApiResponse<ReviewDetailResponse>>('/reviews', request);
    if (response.data.data === null || response.data.data === undefined) {
      throw new Error(
        response.data.error?.detail ?? 'Failed to create review.',
      );
    }
    return response.data.data;
  } catch (error) {
    throw normalizeError(error);
  }
}

export async function getReviews(
  page = 1,
  pageSize = 20,
): Promise<PaginatedList<ReviewListItem>> {
  try {
    const response = await client.get<
      ApiResponse<PaginatedList<ReviewListItem>>
    >('/reviews', { params: { page, pageSize } });
    if (response.data.data === null || response.data.data === undefined) {
      throw new Error(
        response.data.error?.detail ?? 'Failed to fetch reviews.',
      );
    }
    return response.data.data;
  } catch (error) {
    throw normalizeError(error);
  }
}

export async function getReviewById(
  id: string,
): Promise<ReviewDetailResponse> {
  try {
    const response = await client.get<ApiResponse<ReviewDetailResponse>>(
      `/reviews/${encodeURIComponent(id)}`,
    );
    if (response.data.data === null || response.data.data === undefined) {
      throw new Error(
        response.data.error?.detail ?? 'Review not found.',
      );
    }
    return response.data.data;
  } catch (error) {
    throw normalizeError(error);
  }
}

export async function deleteReview(id: string): Promise<void> {
  try {
    await client.delete(`/reviews/${encodeURIComponent(id)}`);
  } catch (error) {
    throw normalizeError(error);
  }
}

// --- Settings ---

export async function getSettings(): Promise<AppSettingsResponse> {
  try {
    const response =
      await client.get<ApiResponse<AppSettingsResponse>>('/settings');
    if (response.data.data === null || response.data.data === undefined) {
      throw new Error(
        response.data.error?.detail ?? 'Failed to fetch settings.',
      );
    }
    return response.data.data;
  } catch (error) {
    throw normalizeError(error);
  }
}

export async function updateSettings(
  request: UpdateSettingsRequest,
): Promise<AppSettingsResponse> {
  try {
    const response = await client.put<ApiResponse<AppSettingsResponse>>(
      '/settings',
      request,
    );
    if (response.data.data === null || response.data.data === undefined) {
      throw new Error(
        response.data.error?.detail ?? 'Failed to update settings.',
      );
    }
    return response.data.data;
  } catch (error) {
    throw normalizeError(error);
  }
}
