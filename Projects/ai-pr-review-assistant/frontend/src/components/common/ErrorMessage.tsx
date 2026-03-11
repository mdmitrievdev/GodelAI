import styles from './ErrorMessage.module.css';

interface ErrorMessageProps {
  message: string;
}

export function ErrorMessage({ message }: ErrorMessageProps): React.JSX.Element {
  return (
    <div className={styles.container} role="alert">
      <span className={styles.icon} aria-hidden="true">⚠</span>
      <span className={styles.text}>{message}</span>
    </div>
  );
}
