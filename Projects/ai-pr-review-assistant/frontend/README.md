# Frontend — PR Review Assistant

React 18 + Vite + TypeScript (strict) frontend for the PR Review Assistant application.

## Requirements

- Node.js 20 LTS or higher
- npm 10+

## Getting Started

```bash
npm install
npm run dev        # Start dev server at http://localhost:5173
npm run build      # Production build
npm run lint       # ESLint check
npm run format     # Prettier format
npm test           # Jest unit tests
```

## Environment

Dev server proxies `/api` requests to `http://localhost:5000` (backend).

## Stack

| Tool | Version | Purpose |
|------|---------|---------|
| React | 18 | UI framework |
| Vite | 6 | Build tool |
| TypeScript | 5 (strict) | Type safety |
| React Router | v6 | Client-side routing |
| Axios | 1 | HTTP client |
| Jest + RTL | 29 | Unit tests |
| ESLint + Prettier | 9 / 3 | Code quality |
