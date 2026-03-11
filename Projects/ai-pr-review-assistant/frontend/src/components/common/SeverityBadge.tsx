import type { FindingSeverity } from '../../models';
import styles from './SeverityBadge.module.css';

interface SeverityBadgeProps {
  severity: FindingSeverity;
}

export function SeverityBadge({ severity }: SeverityBadgeProps): React.JSX.Element {
  return (
    <span className={`${styles.badge} ${styles[severity.toLowerCase() as Lowercase<FindingSeverity>]}`}>
      {severity}
    </span>
  );
}
