import { useState, type ReactElement } from 'react'
import { useParams, Link } from 'react-router-dom'

import { ErrorMessage } from '../components/common/ErrorMessage'
import { LoadingSpinner } from '../components/common/LoadingSpinner'
import { FindingsList } from '../components/review/FindingsList'
import { ReviewSummary } from '../components/review/ReviewSummary'
import { useReviewDetail } from '../hooks/useReviewDetail'
import type { FindingCategory, FindingSeverity } from '../models'
import styles from './ReviewResultPage.module.css'

const CATEGORIES: FindingCategory[] = [
  'Bug',
  'Security',
  'Performance',
  'CodeStyle',
  'Naming',
  'BestPractice',
]

const CATEGORY_LABELS: Record<FindingCategory, string> = {
  Bug: 'Bug',
  Security: 'Security',
  Performance: 'Performance',
  CodeStyle: 'Code Style',
  Naming: 'Naming',
  BestPractice: 'Best Practice',
}

const SEVERITIES: FindingSeverity[] = ['Critical', 'Warning', 'Info']

export default function ReviewResultPage(): ReactElement {
  const { id } = useParams<{ id: string }>()
  const { data, loading, error } = useReviewDetail(id ?? '')

  const [severityFilter, setSeverityFilter] = useState<FindingSeverity | 'All'>('All')
  const [categoryFilter, setCategoryFilter] = useState<FindingCategory | 'All'>('All')
  const [diffExpanded, setDiffExpanded] = useState<boolean>(false)

  if (loading) {
    return (
      <div className={styles.centered}>
        <LoadingSpinner message="Loading review results…" />
      </div>
    )
  }

  if (error) {
    return (
      <div className={styles.page}>
        <ErrorMessage message={error} />
        <Link to="/review" className={styles.backLink}>
          ← New Review
        </Link>
      </div>
    )
  }

  if (!data) {
    return (
      <div className={styles.page}>
        <ErrorMessage message="Review not found." />
        <Link to="/review" className={styles.backLink}>
          ← New Review
        </Link>
      </div>
    )
  }

  const formattedDate = new Date(data.createdAt).toLocaleString()

  return (
    <div className={styles.page}>
      <div className={styles.header}>
        <div className={styles.headerMeta}>
          <h1 className={styles.title}>Review Result</h1>
          <div className={styles.meta}>
            <span className={styles.metaItem}>
              <span className={styles.metaLabel}>Language:</span>
              <span className={styles.metaValue}>{data.language}</span>
            </span>
            <span className={styles.metaItem}>
              <span className={styles.metaLabel}>Analyzed:</span>
              <span className={styles.metaValue}>{formattedDate}</span>
            </span>
            {data.prUrl && (
              <span className={styles.metaItem}>
                <span className={styles.metaLabel}>PR:</span>
                <a
                  href={data.prUrl}
                  className={styles.prLink}
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  {data.prUrl}
                </a>
              </span>
            )}
          </div>
        </div>
        <Link to="/review" className={styles.backLink}>
          ← New Review
        </Link>
      </div>

      <ReviewSummary summary={data.summary} />

      <section className={styles.section}>
        <div className={styles.filterBar}>
          <div className={styles.filterGroup}>
            <span className={styles.filterLabel}>Severity:</span>
            <button
              className={[
                styles.filterBtn,
                severityFilter === 'All' ? styles.active : '',
              ].join(' ')}
              onClick={() => { setSeverityFilter('All') }}
            >
              All
            </button>
            {SEVERITIES.map((s) => (
              <button
                key={s}
                className={[
                  styles.filterBtn,
                  styles[s.toLowerCase()],
                  severityFilter === s ? styles.active : '',
                ].join(' ')}
                onClick={() => { setSeverityFilter(s) }}
              >
                {s}
              </button>
            ))}
          </div>

          <div className={styles.filterGroup}>
            <label htmlFor="categoryFilter" className={styles.filterLabel}>
              Category:
            </label>
            <select
              id="categoryFilter"
              className={styles.select}
              value={categoryFilter}
              onChange={(e) => {
                setCategoryFilter(e.target.value as FindingCategory | 'All')
              }}
            >
              <option value="All">All Categories</option>
              {CATEGORIES.map((c) => (
                <option key={c} value={c}>
                  {CATEGORY_LABELS[c]}
                </option>
              ))}
            </select>
          </div>
        </div>

        <FindingsList
          findings={data.findings}
          filters={{ severity: severityFilter, category: categoryFilter }}
        />
      </section>

      <section className={styles.section}>
        <button
          className={styles.diffToggle}
          onClick={() => { setDiffExpanded((prev) => !prev) }}
          aria-expanded={diffExpanded}
        >
          {diffExpanded ? '▲ Hide' : '▼ Show'} Original Diff
        </button>
        {diffExpanded && (
          <pre className={styles.diffBlock}>
            <code>{data.codeDiff}</code>
          </pre>
        )}
      </section>
    </div>
  )
}
