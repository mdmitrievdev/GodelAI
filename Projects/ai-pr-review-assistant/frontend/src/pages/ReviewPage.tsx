import { type FormEvent, type ReactElement, useState } from 'react'
import { useNavigate } from 'react-router-dom'

import { ErrorMessage } from '../components/common/ErrorMessage'
import { LoadingSpinner } from '../components/common/LoadingSpinner'
import { useCreateReview } from '../hooks/useCreateReview'
import styles from './ReviewPage.module.css'

const SUPPORTED_LANGUAGES = [
  'C#',
  'TypeScript',
  'JavaScript',
  'Python',
  'Java',
  'Go',
  'Other',
] as const

export default function ReviewPage(): ReactElement {
  const navigate = useNavigate()
  const { submit, loading, error } = useCreateReview()

  const [codeDiff, setCodeDiff] = useState<string>('')
  const [language, setLanguage] = useState<string>('')
  const [prUrl, setPrUrl] = useState<string>('')
  const [validationError, setValidationError] = useState<string | null>(null)

  function validate(): boolean {
    if (codeDiff.trim().length === 0) {
      setValidationError('Code diff cannot be empty.')
      return false
    }
    if (language === '') {
      setValidationError('Please select a programming language.')
      return false
    }
    setValidationError(null)
    return true
  }

  async function handleSubmit(e: FormEvent): Promise<void> {
    e.preventDefault()
    if (!validate()) return

    try {
      const result = await submit({
        codeDiff,
        language,
        prUrl: prUrl.trim() || null,
      })
      navigate(`/review/${result.id}`)
    } catch {
      // error state is managed by useCreateReview
    }
  }

  const displayError = validationError ?? error

  return (
    <div className={styles.page}>
      <h1 className={styles.title}>New Code Review</h1>
      <p className={styles.subtitle}>
        Paste your code diff below, select the language, and submit for
        AI-powered analysis.
      </p>

      {displayError && <ErrorMessage message={displayError} />}

      <form className={styles.form} onSubmit={(e) => { void handleSubmit(e) }}>
        <div className={styles.field}>
          <label htmlFor="codeDiff" className={styles.label}>
            Code Diff <span className={styles.required}>*</span>
          </label>
          <textarea
            id="codeDiff"
            className={styles.textarea}
            placeholder="Paste your code diff here..."
            value={codeDiff}
            onChange={(e) => { setCodeDiff(e.target.value) }}
            rows={16}
            maxLength={50000}
            disabled={loading}
          />
          <span className={styles.charCount}>
            {codeDiff.length.toLocaleString()} / 50,000
          </span>
        </div>

        <div className={styles.row}>
          <div className={styles.field}>
            <label htmlFor="language" className={styles.label}>
              Language <span className={styles.required}>*</span>
            </label>
            <select
              id="language"
              className={styles.select}
              value={language}
              onChange={(e) => { setLanguage(e.target.value) }}
              disabled={loading}
            >
              <option value="">— Select language —</option>
              {SUPPORTED_LANGUAGES.map((lang) => (
                <option key={lang} value={lang}>
                  {lang}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.field}>
            <label htmlFor="prUrl" className={styles.label}>
              GitHub PR URL <span className={styles.optional}>(optional)</span>
            </label>
            <input
              id="prUrl"
              className={styles.input}
              type="url"
              placeholder="https://github.com/owner/repo/pull/123"
              value={prUrl}
              onChange={(e) => { setPrUrl(e.target.value) }}
              disabled={loading}
            />
          </div>
        </div>

        <div className={styles.actions}>
          {loading ? (
            <LoadingSpinner message="Analyzing code diff…" />
          ) : (
            <button type="submit" className={styles.submitBtn}>
              Submit for Review
            </button>
          )}
        </div>
      </form>
    </div>
  )
}
