import type { ReactElement } from 'react'
import { NavLink, Outlet, Link } from 'react-router-dom'

import styles from './AdminLayout.module.css'

export default function AdminLayout(): ReactElement {
  return (
    <div className={styles.layout}>
      <div className={styles.topBar}>
        <div className={styles.topBarLeft}>
          <span className={styles.adminBadge}>Admin</span>
        </div>
        <Link to="/" className={styles.backLink}>
          ← Back to Main
        </Link>
      </div>

      <div className={styles.body}>
        <aside className={styles.sidebar}>
          <nav className={styles.sidebarNav}>
            <NavLink
              to="/admin/history"
              className={({ isActive }): string =>
                [styles.sidebarLink, isActive ? styles.sidebarLinkActive : ''].filter(Boolean).join(' ')
              }
            >
              <span className={styles.sidebarLinkIcon}>🕑</span>
              History
            </NavLink>
            <NavLink
              to="/admin/settings"
              className={({ isActive }): string =>
                [styles.sidebarLink, isActive ? styles.sidebarLinkActive : ''].filter(Boolean).join(' ')
              }
            >
              <span className={styles.sidebarLinkIcon}>⚙️</span>
              Settings
            </NavLink>
          </nav>
        </aside>

        <main className={styles.main}>
          <Outlet />
        </main>
      </div>
    </div>
  )
}
