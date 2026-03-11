import { useCallback, useEffect, useRef, useState } from 'react';
import type { ApiError, PaginatedList, ReviewListItem } from '../models';
import { getReviews } from '../services/api';

interface UseReviewsResult {
  data: PaginatedList<ReviewListItem> | null;
  loading: boolean;
  error: string | null;
  refetch: () => void;
}

export function useReviews(page: number, pageSize: number): UseReviewsResult {
  const [data, setData] = useState<PaginatedList<ReviewListItem> | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const cancelledRef = useRef<boolean>(false);

  const fetchData = useCallback((): void => {
    cancelledRef.current = false;
    setLoading(true);
    setError(null);

    getReviews(page, pageSize)
      .then((result) => {
        if (!cancelledRef.current) {
          setData(result);
        }
      })
      .catch((err: unknown) => {
        if (!cancelledRef.current) {
          setError((err as ApiError).message);
        }
      })
      .finally(() => {
        if (!cancelledRef.current) {
          setLoading(false);
        }
      });
  }, [page, pageSize]);

  useEffect(() => {
    fetchData();
    return (): void => {
      cancelledRef.current = true;
    };
  }, [fetchData]);

  const refetch = useCallback((): void => {
    fetchData();
  }, [fetchData]);

  return { data, loading, error, refetch };
}
