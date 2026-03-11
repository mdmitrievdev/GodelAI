import type { Finding } from '../../models';
import { SeverityBadge } from '../common/SeverityBadge';
import { ConfidenceBadge } from '../common/ConfidenceBadge';
import { SuggestedFixPanel } from './SuggestedFixPanel';
import styles from './FindingCard.module.css';

interface FindingCardProps {
  finding: Finding;
}

export function FindingCard({ finding }: FindingCardProps): React.JSX.Element {
  const severityClass = styles[finding.severity.toLowerCase()] ?? '';

  return (
    <div className={[styles.card, severityClass].join(' ')}>
      <div className={styles.header}>
        <div className={styles.titleRow}>
          <h4 className={styles.title}>{finding.title}</h4>
          <div className={styles.badges}>
            <SeverityBadge severity={finding.severity} />
            <ConfidenceBadge confidence={finding.confidence} />
          </div>
        </div>
        <div className={styles.meta}>
          <span className={styles.category}>{finding.category}</span>
          {finding.lineReference && (
            <span className={styles.lineRef}>Line {finding.lineReference}</span>
          )}
        </div>
      </div>
      <p className={styles.description}>{finding.description}</p>
      <div className={styles.suggestion}>
        <span className={styles.suggestionLabel}>Suggestion:</span>
        <p className={styles.suggestionText}>{finding.suggestion}</p>
      </div>
      {finding.suggestedFix && (
        <SuggestedFixPanel suggestedFix={finding.suggestedFix} />
      )}
    </div>
  );
}
