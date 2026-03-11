import { useMemo } from 'react';
import type { Finding, FindingCategory, FindingSeverity } from '../../models';
import { FindingCard } from './FindingCard';
import styles from './FindingsList.module.css';

interface FindingsFilters {
  severity: FindingSeverity | 'All';
  category: FindingCategory | 'All';
}

interface FindingsListProps {
  findings: Finding[];
  filters: FindingsFilters;
}

const CATEGORY_ORDER: FindingCategory[] = [
  'Bug',
  'Security',
  'Performance',
  'CodeStyle',
  'Naming',
  'BestPractice',
];

const CATEGORY_LABELS: Record<FindingCategory, string> = {
  Bug: 'Bugs',
  Security: 'Security',
  Performance: 'Performance',
  CodeStyle: 'Code Style',
  Naming: 'Naming',
  BestPractice: 'Best Practice',
};

function groupByCategory(findings: Finding[]): Map<FindingCategory, Finding[]> {
  const grouped = new Map<FindingCategory, Finding[]>();
  for (const category of CATEGORY_ORDER) {
    const items = findings.filter((f) => f.category === category);
    if (items.length > 0) {
      grouped.set(category, items);
    }
  }
  return grouped;
}

export function FindingsList({ findings, filters }: FindingsListProps): React.JSX.Element {
  const filtered = useMemo(() => {
    let result = findings;
    if (filters.severity !== 'All') {
      result = result.filter((f) => f.severity === filters.severity);
    }
    if (filters.category !== 'All') {
      result = result.filter((f) => f.category === filters.category);
    }
    return result;
  }, [findings, filters]);

  const grouped = useMemo(() => groupByCategory(filtered), [filtered]);

  if (filtered.length === 0) {
    return (
      <div className={styles.empty}>
        No findings match the selected filters.
      </div>
    );
  }

  return (
    <div className={styles.container}>
      {Array.from(grouped.entries()).map(([category, items]) => (
        <section key={category} className={styles.group}>
          <h3 className={styles.groupHeading}>
            {CATEGORY_LABELS[category]}
            <span className={styles.groupCount}>{items.length}</span>
          </h3>
          <div className={styles.cards}>
            {items.map((finding) => (
              <FindingCard key={finding.id} finding={finding} />
            ))}
          </div>
        </section>
      ))}
    </div>
  );
}

export type { FindingsFilters };
