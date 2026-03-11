import { useCallback, useState } from 'react';
import type {
  ApiError,
  CreateReviewRequest,
  ReviewDetailResponse,
} from '../models';
import { createReview } from '../services/api';

interface UseCreateReviewResult {
  submit: (request: CreateReviewRequest) => Promise<ReviewDetailResponse>;
  loading: boolean;
  error: string | null;
}

export function useCreateReview(): UseCreateReviewResult {
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const submit = useCallback(
    async (request: CreateReviewRequest): Promise<ReviewDetailResponse> => {
      setLoading(true);
      setError(null);
      try {
        const result = await createReview(request);
        return result;
      } catch (err: unknown) {
        const apiErr = err as ApiError;
        setError(apiErr.message);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [],
  );

  return { submit, loading, error };
}
