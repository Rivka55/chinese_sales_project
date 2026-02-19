# Copilot Instructions

## Project Overview
- Angular 21 standalone app (no NgModules). Bootstrap in src/main.ts; app-wide providers in src/app/app.config.ts.
- Routing is defined in src/app/app.routes.ts and uses functional guards (authGuard, managerGuard).
- HTTP calls go through services in src/app/services/* with base URLs built from environment.apiUrl.
- Auth uses JWT stored in localStorage under "token"; authInterceptor attaches Bearer token to all requests.

## Key Patterns and Conventions
- Standalone components declare `imports` locally and use templateUrl/styleUrl (see src/app/components/Gift/gift-list/gift-list.ts).
- New Angular control flow syntax (`@if`) is used in templates (see src/app/app.html).
- Services typically import environment.development for apiUrl (see src/app/services/Gift/gift-service.ts).
- Gift list state uses a BehaviorSubject + refresh method for live updates (see src/app/services/Gift/gift-service.ts).
- UI strings and alerts are largely Hebrew; preserve existing language style when editing templates.

## Integration Points
- API base URL comes from src/environments/environment.development.ts (dev) and environment.ts (prod).
- Auth endpoints: `${apiUrl}/Auth` with login returning a raw token string; client sanitizes quotes before storage.
- Manager-only endpoints are grouped under /admin or /manager paths (e.g., CartService and GiftService).

## Dev Workflows
- Start dev server: `npm run start` (Angular CLI `ng serve`).
- Run unit tests: `npm run test` (Angular CLI test runner configured for Vitest).
- Build: `npm run build` (outputs to dist/).

## Where To Look
- App shell/nav and auth-driven UI: src/app/app.html and src/app/app.ts.
- Route protection logic: src/app/guards/auth.guard.ts and src/app/guards/manager.guard.ts.
- Example data flows (search, add to cart, refresh list): src/app/components/Gift/gift-list/gift-list.ts.
