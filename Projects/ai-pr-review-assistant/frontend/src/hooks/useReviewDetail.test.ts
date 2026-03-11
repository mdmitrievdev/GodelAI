import { renderHook, waitFor } from '@testing-library/react';
import { useReviewDetail } from './useReviewDetail';
import { getReviewById } from '../services/api';
import type { ReviewDetailResponse } from '../models';

// Factory form prevents Jest from ever loading/executing api.ts (which uses import.meta.env)
jest.mock('../services/api', () => ({
  getReviewById: jest.fn(),
}));

const mockGetReviewById = jest.mocked(getReviewById);

const mockReviewDetail: ReviewDetailResponse = {
  id: '01HXYTEST001',
  createdAt: '2026-03-10T12:00:00Z',
  language: 'TypeScript',
  codeDiff: 'const x = null',
  prUrl: null,
  summary: {
    totalFindings: 1,
    criticalCount: 0,
    warningCount: 1,
    infoCount: 0,
    averageConfidence: 80,
  },
  findings: [],
};

describe('useReviewDetail', () => {
  afterEach(() => {
    jest.resetAllMocks();
  });

  it('useReviewDetail_OnSuccess_ReturnsData', async () => {
    // Arrange
    mockGetReviewById.mockResolvedValue(mockReviewDetail);

    // Act
    const { result } = renderHook(() => useReviewDetail('01HXYTEST001'));
    await waitFor(() => { expect(result.current.loading).toBe(false); });

    // Assert
    expect(result.current.data).toEqual(mockReviewDetail);
  });

  it('useReviewDetail_OnSuccess_SetsErrorToNull', async () => {
    // Arrange
    mockGetReviewById.mockResolvedValue(mockReviewDetail);

    // Act
    const { result } = renderHook(() => useReviewDetail('01HXYTEST001'));
    await waitFor(() => { expect(result.current.loading).toBe(false); });

    // Assert
    expect(result.current.error).toBeNull();
  });

  it('useReviewDetail_OnFailure_SetsErrorMessage', async () => {
    // Arrange
    mockGetReviewById.mockRejectedValue({ message: 'Review not found', code: '404' });

    // Act
    const { result } = renderHook(() => useReviewDetail('01HXYTEST001'));
    await waitFor(() => { expect(result.current.loading).toBe(false); });

    // Assert
    expect(result.current.error).toBe('Review not found');
  });

  it('useReviewDetail_OnFailure_DataRemainsNull', async () => {
    // Arrange
    mockGetReviewById.mockRejectedValue({ message: 'Review not found' });

    // Act
    const { result } = renderHook(() => useReviewDetail('01HXYTEST001'));
    await waitFor(() => { expect(result.current.loading).toBe(false); });

    // Assert
    expect(result.current.data).toBeNull();
  });
});
