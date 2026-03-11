import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { ErrorBoundary } from './ErrorBoundary';

function ThrowOnRender(): never {
  throw new Error('Test render error');
}

// Suppress React's console.error output during error boundary tests
const originalConsoleError = console.error;
beforeEach(() => {
  console.error = jest.fn();
});
afterEach(() => {
  console.error = originalConsoleError;
});

describe('ErrorBoundary', () => {
  it('ErrorBoundary_WhenChildThrows_RendersFallbackHeading', () => {
    // Arrange + Act
    render(
      <MemoryRouter>
        <ErrorBoundary>
          <ThrowOnRender />
        </ErrorBoundary>
      </MemoryRouter>,
    );

    // Assert
    expect(screen.getByText('Something went wrong')).toBeInTheDocument();
  });

  it('ErrorBoundary_WhenChildThrows_RendersErrorMessage', () => {
    // Arrange + Act
    render(
      <MemoryRouter>
        <ErrorBoundary>
          <ThrowOnRender />
        </ErrorBoundary>
      </MemoryRouter>,
    );

    // Assert
    expect(screen.getByText('Test render error')).toBeInTheDocument();
  });

  it('ErrorBoundary_WhenChildThrows_RendersGoHomeLink', () => {
    // Arrange + Act
    render(
      <MemoryRouter>
        <ErrorBoundary>
          <ThrowOnRender />
        </ErrorBoundary>
      </MemoryRouter>,
    );

    // Assert
    expect(screen.getByText('Go Home')).toBeInTheDocument();
  });

  it('ErrorBoundary_WithHealthyChildren_RendersChildren', () => {
    // Arrange + Act
    render(
      <MemoryRouter>
        <ErrorBoundary>
          <p>Child content</p>
        </ErrorBoundary>
      </MemoryRouter>,
    );

    // Assert
    expect(screen.getByText('Child content')).toBeInTheDocument();
  });
});
