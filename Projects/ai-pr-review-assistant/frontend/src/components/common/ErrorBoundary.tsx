import { Component, type ReactNode } from 'react'
import { Link, useLocation } from 'react-router-dom'

import styles from './ErrorBoundary.module.css'

interface ErrorBoundaryProps {
  children: ReactNode
}

interface ErrorBoundaryState {
  hasError: boolean
  errorMessage: string
}

export class ErrorBoundary extends Component<ErrorBoundaryProps, ErrorBoundaryState> {
  constructor(props: ErrorBoundaryProps) {
    super(props)
    this.state = { hasError: false, errorMessage: '' }
  }

  static getDerivedStateFromError(error: unknown): ErrorBoundaryState {
    const message = error instanceof Error ? error.message : 'An unexpected error occurred.'
    return { hasError: true, errorMessage: message }
  }

  componentDidCatch(_error: unknown, _info: { componentStack: string }): void {
    // Rendering error captured by boundary — no action needed here
  }

  render(): ReactNode {
    if (this.state.hasError) {
      return (
        <div className={styles.container}>
          <div className={styles.card}>
            <span className={styles.icon} aria-hidden="true">💥</span>
            <h2 className={styles.heading}>Something went wrong</h2>
            <p className={styles.message}>{this.state.errorMessage}</p>
            <Link to="/" className={styles.homeButton}>
              Go Home
            </Link>
          </div>
        </div>
      )
    }

    return this.props.children
  }
}

/**
 * Wraps `ErrorBoundary` with a `key` derived from the current route location.
 * Remounting on every navigation resets error state automatically.
 */
export function RouteErrorBoundary({ children }: { children: ReactNode }): React.JSX.Element {
  const location = useLocation()
  return <ErrorBoundary key={location.key}>{children}</ErrorBoundary>
}
