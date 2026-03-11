import styles from './ConfidenceBadge.module.css';

interface ConfidenceBadgeProps {
  confidence: number;
}

function getLevel(confidence: number): 'high' | 'medium' | 'low' {
  if (confidence >= 75) return 'high';
  if (confidence >= 50) return 'medium';
  return 'low';
}

export function ConfidenceBadge({ confidence }: ConfidenceBadgeProps): React.JSX.Element {
  const level = getLevel(confidence);
  return (
    <span
      className={`${styles.badge} ${styles[level]}`}
      title={`AI confidence: ${confidence}%`}
    >
      {confidence}%
    </span>
  );
}
