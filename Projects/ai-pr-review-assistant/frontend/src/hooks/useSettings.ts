import { useCallback, useEffect, useState } from 'react';
import type {
  ApiError,
  AppSettingsResponse,
  UpdateSettingsRequest,
} from '../models';
import {
  getSettings,
  updateSettings as updateSettingsApi,
} from '../services/api';

interface UseSettingsResult {
  data: AppSettingsResponse | null;
  loading: boolean;
  error: string | null;
  updateSettings: (request: UpdateSettingsRequest) => Promise<void>;
}

export function useSettings(): UseSettingsResult {
  const [data, setData] = useState<AppSettingsResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError(null);

    getSettings()
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
  }, []);

  const updateSettings = useCallback(
    async (request: UpdateSettingsRequest): Promise<void> => {
      setLoading(true);
      setError(null);
      try {
        const result = await updateSettingsApi(request);
        setData(result);
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

  return { data, loading, error, updateSettings };
}
