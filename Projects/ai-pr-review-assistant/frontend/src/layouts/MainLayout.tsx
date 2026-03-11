import type { ReactElement } from 'react'
import { NavLink, Outlet } from 'react-router-dom'

import styles from './MainLayout.module.css'

export default function MainLayout(): ReactElement {
  return (
    <div className={styles.layout}>
      <header className={styles.header}>
        <NavLink to="/" className={styles.logo}>
          <span className={styles.logoIcon}>🔍</span>
          PR Review Assistant
        </NavLink>

        <nav className={styles.nav}>
          <NavLink
            to="/"
            end
            className={({ isActive }): string =>
              `${styles.navLink} ${isActive ? styles.navLinkActive : ''}`
            }
          >
            Home
          </NavLink>
          <NavLink
            to="/review"
            className={({ isActive }): string =>
              `${styles.navLink} ${isActive ? styles.navLinkActive : ''}`
            }
          >
            New Review
          </NavLink>
        </nav>
      </header>

      <main className={styles.main}>
        <Outlet />
      </main>

      <footer className={styles.footer}>
        PR Review Assistant v1.0.0
      </footer>
    </div>
  )
}
