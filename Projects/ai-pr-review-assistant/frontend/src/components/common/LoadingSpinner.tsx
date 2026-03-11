import styles from './LoadingSpinner.module.css';

interface LoadingSpinnerProps {
  message?: string;
}

export function LoadingSpinner({ message }: LoadingSpinnerProps): React.JSX.Element {
  return (
    <div className={styles.container} role="status" aria-live="polite">
      <div className={styles.spinner} aria-hidden="true" />
      {message && <span className={styles.message}>{message}</span>}
      <span className={styles.srOnly}>Loading{message ? `: ${message}` : '...'}</span>
    </div>
  );
}
