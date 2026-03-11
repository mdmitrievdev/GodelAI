import { useEffect, useState } from 'react';
import type { ApiError, ReviewDetailResponse } from '../models';
import { getReviewById } from '../services/api';

interface UseReviewDetailResult {
  data: ReviewDetailResponse | null;
  loading: boolean;
  error: string | null;
}

export function useReviewDetail(id: string): UseReviewDetailResult {
  const [data, setData] = useState<ReviewDetailResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError(null);

    getReviewById(id)
      .then((result) => {
        if (!cancelled) {
          setData(result);
        }
      })
      .catch((err: unknown) => {
        if (!cancelled) {
          setError((err as ApiError).message);
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoading(false);
        }
      });

    return (): void => {
      cancelled = true;
    };
  }, [id]);

  return { data, loading, error };
}
