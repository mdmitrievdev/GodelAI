import { render, screen } from '@testing-library/react';
import { ReviewSummary } from './ReviewSummary';
import type { ReviewSummary as ReviewSummaryModel } from '../../models';

const mockSummary: ReviewSummaryModel = {
  totalFindings: 10,
  criticalCount: 3,
  warningCount: 5,
  infoCount: 2,
  averageConfidence: 75.5,
};

describe('ReviewSummary', () => {
  it('ReviewSummary_WithValidSummary_RendersTotalFindings', () => {
    // Arrange + Act
    render(<ReviewSummary summary={mockSummary} />);

    // Assert
    expect(screen.getByText('10')).toBeInTheDocument();
  });

  it('ReviewSummary_WithValidSummary_RendersCriticalCount', () => {
    // Arrange + Act
    render(<ReviewSummary summary={mockSummary} />);

    // Assert
    expect(screen.getByText('3')).toBeInTheDocument();
  });

  it('ReviewSummary_WithValidSummary_RendersWarningCount', () => {
    // Arrange + Act
    render(<ReviewSummary summary={mockSummary} />);

    // Assert
    expect(screen.getByText('5')).toBeInTheDocument();
  });

  it('ReviewSummary_WithValidSummary_RendersInfoCount', () => {
    // Arrange + Act
    render(<ReviewSummary summary={mockSummary} />);

    // Assert
    expect(screen.getByText('2')).toBeInTheDocument();
  });

  it('ReviewSummary_WithValidSummary_RendersAverageConfidenceRounded', () => {
    // Arrange + Act
    render(<ReviewSummary summary={mockSummary} />);

    // Assert
    expect(screen.getByText('76%')).toBeInTheDocument();
  });
});
