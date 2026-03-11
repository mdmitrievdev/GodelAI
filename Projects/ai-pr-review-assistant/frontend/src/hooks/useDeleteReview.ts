import { useCallback, useState } from 'react';
import type { ApiError } from '../models';
import { deleteReview } from '../services/api';

interface UseDeleteReviewResult {
  deleteReview: (id: string) => Promise<void>;
  loading: boolean;
  error: string | null;
}

export function useDeleteReview(): UseDeleteReviewResult {
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const remove = useCallback(async (id: string): Promise<void> => {
    setLoading(true);
    setError(null);
    try {
      await deleteReview(id);
    } catch (err: unknown) {
      const apiErr = err as ApiError;
      setError(apiErr.message);
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  return { deleteReview: remove, loading, error };
}
