import { useState, type ReactElement } from 'react'
import { useNavigate } from 'react-router-dom'

import { ErrorMessage } from '../components/common/ErrorMessage'
import { LoadingSpinner } from '../components/common/LoadingSpinner'
import { useDeleteReview } from '../hooks/useDeleteReview'
import { useReviews } from '../hooks/useReviews'
import styles from './HistoryPage.module.css'

const PAGE_SIZE = 20

function formatDate(iso: string): string {
  return new Date(iso).toLocaleString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

export default function HistoryPage(): ReactElement {
  const navigate = useNavigate()
  const [page, setPage] = useState<number>(1)
  const { data, loading, error, refetch } = useReviews(page, PAGE_SIZE)
  const { deleteReview, loading: deleting, error: deleteError } = useDeleteReview()

  async function handleDelete(e: React.MouseEvent, id: string): Promise<void> {
    e.stopPropagation()
    if (!window.confirm('Delete this review? This cannot be undone.')) return
    try {
      await deleteReview(id)
      refetch()
    } catch {
      // deleteError state managed by useDeleteReview
    }
  }

  const totalPages = data ? Math.ceil(data.totalCount / PAGE_SIZE) : 1

  return (
    <div className={styles.page}>
      <header className={styles.header}>
        <h1 className={styles.title}>Review History</h1>
        {data && (
          <span className={styles.totalCount}>
            {data.totalCount} review{data.totalCount !== 1 ? 's' : ''}
          </span>
        )}
      </header>

      {deleteError && <ErrorMessage message={deleteError} />}

      {loading && <LoadingSpinner message="Loading reviews…" />}

      {error && !loading && <ErrorMessage message={error} />}

      {!loading && !error && data && data.items.length === 0 && (
        <div className={styles.emptyState}>
          <p className={styles.emptyStateText}>No reviews yet.</p>
          <button
            className={styles.ctaButton}
            onClick={(): void => { navigate('/review') }}
          >
            Start your first review
          </button>
        </div>
      )}

      {!loading && !error && data && data.items.length > 0 && (
        <>
          <table className={styles.table}>
            <thead>
              <tr>
                <th className={styles.th}>Date</th>
                <th className={styles.th}>Language</th>
                <th className={styles.th}>Snippet</th>
                <th className={styles.th}>Findings</th>
                <th className={[styles.th, styles.thActions].filter(Boolean).join(' ')}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((review) => (
                <tr
                  key={review.id}
                  className={styles.row}
                  onClick={(): void => { navigate(`/review/${review.id}`) }}
                >
                  <td className={styles.td}>{formatDate(review.createdAt)}</td>
                  <td className={styles.td}>
                    <span className={styles.languageBadge}>{review.language}</span>
                  </td>
                  <td className={[styles.td, styles.snippetCell].filter(Boolean).join(' ')}>
                    <code className={styles.snippet}>{review.codeSnippet}</code>
                  </td>
                  <td className={styles.td}>
                    <span className={styles.findingCounts}>
                      <span className={styles.total}>{review.totalFindings} total</span>
                      {review.criticalCount > 0 && (
                        <span className={styles.critical}>{review.criticalCount} Critical</span>
                      )}
                      {review.warningCount > 0 && (
                        <span className={styles.warning}>{review.warningCount} Warning</span>
                      )}
                      {review.infoCount > 0 && (
                        <span className={styles.info}>{review.infoCount} Info</span>
                      )}
                    </span>
                  </td>
                  <td className={[styles.td, styles.tdActions].filter(Boolean).join(' ')}>
                    <button
                      className={styles.deleteButton}
                      onClick={(e): void => void handleDelete(e, review.id)}
                      disabled={deleting}
                      aria-label="Delete review"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          {totalPages > 1 && (
            <div className={styles.pagination}>
              <button
                className={styles.pageButton}
                onClick={(): void => { setPage((p) => Math.max(1, p - 1)) }}
                disabled={page === 1}
              >
                ← Previous
              </button>
              <span className={styles.pageInfo}>
                Page {page} of {totalPages}
              </span>
              <button
                className={styles.pageButton}
                onClick={(): void => { setPage((p) => Math.min(totalPages, p + 1)) }}
                disabled={page === totalPages}
              >
                Next →
              </button>
            </div>
          )}
        </>
      )}
    </div>
  )
}
