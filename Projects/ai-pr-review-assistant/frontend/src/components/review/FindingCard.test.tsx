import { render, screen } from '@testing-library/react';
import { FindingCard } from './FindingCard';
import type { Finding } from '../../models';

const baseFinding: Finding = {
  id: '01HXYTEST001',
  category: 'Bug',
  severity: 'Critical',
  title: 'Null reference exception',
  description: 'Variable may be null at this point.',
  lineReference: null,
  suggestion: 'Add null check before accessing the property.',
  confidence: 85,
  suggestedFix: null,
};

describe('FindingCard', () => {
  it('FindingCard_WithValidFinding_RendersTitle', () => {
    // Arrange + Act
    render(<FindingCard finding={baseFinding} />);

    // Assert
    expect(screen.getByText('Null reference exception')).toBeInTheDocument();
  });

  it('FindingCard_WithValidFinding_RendersSeverityBadge', () => {
    // Arrange + Act
    render(<FindingCard finding={baseFinding} />);

    // Assert
    expect(screen.getByText('Critical')).toBeInTheDocument();
  });

  it('FindingCard_WithValidFinding_RendersDescription', () => {
    // Arrange + Act
    render(<FindingCard finding={baseFinding} />);

    // Assert
    expect(screen.getByText('Variable may be null at this point.')).toBeInTheDocument();
  });
});
