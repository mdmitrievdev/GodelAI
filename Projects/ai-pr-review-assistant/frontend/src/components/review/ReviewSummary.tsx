import type { ReviewSummary as ReviewSummaryModel } from '../../models';
import styles from './ReviewSummary.module.css';

interface ReviewSummaryProps {
  summary: ReviewSummaryModel;
}

export function ReviewSummary({ summary }: ReviewSummaryProps): React.JSX.Element {
  return (
    <div className={styles.container}>
      <h3 className={styles.heading}>Review Summary</h3>
      <div className={styles.stats}>
        <div className={styles.stat}>
          <span className={styles.statValue}>{summary.totalFindings}</span>
          <span className={styles.statLabel}>Total Findings</span>
        </div>
        <div className={[styles.stat, styles.critical].join(' ')}>
          <span className={styles.statValue}>{summary.criticalCount}</span>
          <span className={styles.statLabel}>Critical</span>
        </div>
        <div className={[styles.stat, styles.warning].join(' ')}>
          <span className={styles.statValue}>{summary.warningCount}</span>
          <span className={styles.statLabel}>Warning</span>
        </div>
        <div className={[styles.stat, styles.info].join(' ')}>
          <span className={styles.statValue}>{summary.infoCount}</span>
          <span className={styles.statLabel}>Info</span>
        </div>
        <div className={styles.stat}>
          <span className={styles.statValue}>{Math.round(summary.averageConfidence)}%</span>
          <span className={styles.statLabel}>Avg Confidence</span>
        </div>
      </div>
    </div>
  );
}
