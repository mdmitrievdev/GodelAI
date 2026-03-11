import type { ReactElement } from 'react'
import { Link } from 'react-router-dom'

import styles from './HomePage.module.css'

export default function HomePage(): ReactElement {
  return (
    <div className={styles.page}>
      <section className={styles.hero}>
        <h1 className={styles.title}>PR Review Assistant</h1>
        <p className={styles.subtitle}>
          AI-powered code review that catches bugs, style issues, and security
          vulnerabilities — in seconds.
        </p>
        <Link to="/review" className={styles.cta}>
          Start Review
        </Link>
      </section>

      <section className={styles.features}>
        <div className={styles.feature}>
          <span className={styles.featureIcon}>🐛</span>
          <h3 className={styles.featureTitle}>Bug Detection</h3>
          <p className={styles.featureText}>
            Finds potential bugs, logic errors, and edge cases before they reach
            production.
          </p>
        </div>

        <div className={styles.feature}>
          <span className={styles.featureIcon}>🔒</span>
          <h3 className={styles.featureTitle}>Security Analysis</h3>
          <p className={styles.featureText}>
            Scans for security vulnerabilities such as injection flaws and unsafe
            patterns.
          </p>
        </div>

        <div className={styles.feature}>
          <span className={styles.featureIcon}>⚡</span>
          <h3 className={styles.featureTitle}>Performance Tips</h3>
          <p className={styles.featureText}>
            Surfaces inefficient code paths and suggests faster alternatives.
          </p>
        </div>

        <div className={styles.feature}>
          <span className={styles.featureIcon}>🏷️</span>
          <h3 className={styles.featureTitle}>Severity Levels</h3>
          <p className={styles.featureText}>
            Findings are rated Critical, Warning, or Info so you can prioritize
            what matters.
          </p>
        </div>

        <div className={styles.feature}>
          <span className={styles.featureIcon}>📂</span>
          <h3 className={styles.featureTitle}>Category Grouping</h3>
          <p className={styles.featureText}>
            Results grouped by Bug, Naming, Performance, Security, CodeStyle,
            and BestPractice.
          </p>
        </div>

        <div className={styles.feature}>
          <span className={styles.featureIcon}>🕑</span>
          <h3 className={styles.featureTitle}>Review History</h3>
          <p className={styles.featureText}>
            Every review is persisted so you can revisit past analyses anytime.
          </p>
        </div>
      </section>

      <section className={styles.adminSection}>
        <Link to="/admin/history" className={styles.adminLink}>
          Go to Admin Panel →
        </Link>
      </section>
    </div>
  )
}
