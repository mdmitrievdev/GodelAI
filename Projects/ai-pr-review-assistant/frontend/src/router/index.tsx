import { createBrowserRouter, Navigate } from 'react-router-dom'

import MainLayout from '../layouts/MainLayout'
import AdminLayout from '../layouts/AdminLayout'
import HomePage from '../pages/HomePage'
import ReviewPage from '../pages/ReviewPage'
import ReviewResultPage from '../pages/ReviewResultPage'
import HistoryPage from '../pages/HistoryPage'
import AdminSettingsPage from '../pages/AdminSettingsPage'
import { RouteErrorBoundary } from '../components/common/ErrorBoundary'

const router = createBrowserRouter([
  {
    element: <MainLayout />,
    children: [
      { path: '/', element: <RouteErrorBoundary><HomePage /></RouteErrorBoundary> },
      { path: '/review', element: <RouteErrorBoundary><ReviewPage /></RouteErrorBoundary> },
      { path: '/review/:id', element: <RouteErrorBoundary><ReviewResultPage /></RouteErrorBoundary> },
    ],
  },
  {
    element: <AdminLayout />,
    children: [
      { path: '/admin/history', element: <RouteErrorBoundary><HistoryPage /></RouteErrorBoundary> },
      { path: '/admin/settings', element: <RouteErrorBoundary><AdminSettingsPage /></RouteErrorBoundary> },
    ],
  },
  {
    path: '*',
    element: <Navigate to="/" replace />,
  },
])

export default router
