import { useState } from 'react';
import styles from './SuggestedFixPanel.module.css';

interface SuggestedFixPanelProps {
  suggestedFix: string;
}

export function SuggestedFixPanel({ suggestedFix }: SuggestedFixPanelProps): React.JSX.Element {
  const [expanded, setExpanded] = useState(false);

  return (
    <div className={styles.panel}>
      <button
        type="button"
        className={styles.toggle}
        onClick={() => { setExpanded((prev) => !prev); }}
        aria-expanded={expanded}
      >
        <span className={styles.toggleIcon}>{expanded ? '▾' : '▸'}</span>
        Suggested Fix
      </button>
      {expanded && (
        <pre className={styles.code}>
          <code>{suggestedFix}</code>
        </pre>
      )}
    </div>
  );
}
