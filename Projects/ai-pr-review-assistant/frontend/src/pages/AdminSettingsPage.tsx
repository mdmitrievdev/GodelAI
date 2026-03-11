import { useEffect, useState, type ReactElement } from 'react';

import { ErrorMessage } from '../components/common/ErrorMessage';
import { LoadingSpinner } from '../components/common/LoadingSpinner';
import { useSettings } from '../hooks/useSettings';
import styles from './AdminSettingsPage.module.css';

export default function AdminSettingsPage(): ReactElement {
  const { data, loading, error, updateSettings } = useSettings();

  const [useMockAi, setUseMockAi] = useState<boolean>(true);
  const [aiModel, setAiModel] = useState<string>('mock');
  const [saving, setSaving] = useState<boolean>(false);
  const [saveError, setSaveError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  useEffect(() => {
    if (data) {
      setUseMockAi(data.useMockAi);
      setAiModel(data.aiModel);
    }
  }, [data]);

  async function handleSave(): Promise<void> {
    setSaving(true);
    setSaveError(null);
    setSuccessMessage(null);
    try {
      await updateSettings({ useMockAi, aiModel });
      setSuccessMessage('Settings saved successfully.');
    } catch {
      setSaveError('Failed to save settings. Please try again.');
    } finally {
      setSaving(false);
    }
  }

  const isDirty =
    data !== null &&
    (useMockAi !== data.useMockAi || aiModel !== data.aiModel);

  if (loading && !data) {
    return <LoadingSpinner message="Loading settings…" />;
  }

  return (
    <div className={styles.page}>
      <header className={styles.header}>
        <h1 className={styles.title}>Admin Settings</h1>
        <p className={styles.subtitle}>Configure application behaviour.</p>
      </header>

      {error && <ErrorMessage message={error} />}

      {!error && (
        <div className={styles.card}>
          {/* ── AI Mode ── */}
          <section className={styles.section}>
            <h2 className={styles.sectionTitle}>AI Analysis Mode</h2>

            <div className={styles.field}>
              <div className={styles.toggleRow}>
                <div className={styles.toggleInfo}>
                  <span className={styles.label}>Use Mock AI</span>
                  <span className={styles.hint}>
                    When enabled, analysis is handled by the built-in mock
                    service. No external API key required.
                  </span>
                </div>

                <button
                  type="button"
                  role="switch"
                  aria-checked={useMockAi}
                  className={[
                    styles.toggle,
                    useMockAi ? styles.toggleOn : styles.toggleOff,
                  ].join(' ')}
                  onClick={(): void => {
                    setUseMockAi((v) => !v);
                    setSuccessMessage(null);
                  }}
                >
                  <span className={styles.toggleThumb} />
                </button>
              </div>

              <div className={styles.statusBadgeRow}>
                <span
                  className={[
                    styles.statusBadge,
                    useMockAi ? styles.badgeMock : styles.badgeLive,
                  ].join(' ')}
                >
                  {useMockAi ? 'Mock mode active' : 'Live AI active'}
                </span>
              </div>
            </div>

            <div className={styles.field}>
              <label htmlFor="aiModel" className={styles.label}>
                AI Model
              </label>
              <span className={styles.hint}>
                Identifier of the model currently in use.
              </span>
              <input
                id="aiModel"
                type="text"
                className={styles.input}
                value={aiModel}
                maxLength={100}
                onChange={(e): void => {
                  setAiModel(e.target.value);
                  setSuccessMessage(null);
                }}
              />
            </div>
          </section>

          {/* ── Current state read-out ── */}
          {data && (
            <section className={styles.section}>
              <h2 className={styles.sectionTitle}>Saved State</h2>
              <dl className={styles.dl}>
                <dt className={styles.dt}>Mock AI</dt>
                <dd className={styles.dd}>{data.useMockAi ? 'Enabled' : 'Disabled'}</dd>
                <dt className={styles.dt}>AI Model</dt>
                <dd className={styles.dd}>{data.aiModel}</dd>
              </dl>
            </section>
          )}

          {/* ── Feedback ── */}
          {saveError && <ErrorMessage message={saveError} />}
          {successMessage && (
            <div className={styles.successMessage}>{successMessage}</div>
          )}

          {/* ── Actions ── */}
          <div className={styles.actions}>
            <button
              type="button"
              className={styles.saveButton}
              onClick={(): void => void handleSave()}
              disabled={saving || !isDirty}
            >
              {saving ? 'Saving…' : 'Save Settings'}
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
