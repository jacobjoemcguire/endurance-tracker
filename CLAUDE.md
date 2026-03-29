# EnduranceTracker

Personal training analysis and planning platform. Syncs Strava data, supports manual strength entry, configurable dashboards, and AI-assisted event-based training plans.

## Project Structure

```
api/                          .NET 8 Web API (Clean Architecture)
├── EnduranceTracker.Api/     Controllers, middleware, Program.cs
├── EnduranceTracker.Core/    Domain entities, enums, interfaces (no dependencies)
├── EnduranceTracker.Infrastructure/  EF Core, DbContext, configurations
└── EnduranceTracker.Tests/   xUnit tests

web/                          Next.js 16 frontend (App Router, TypeScript, Tailwind)
├── src/app/                  Pages and layouts
├── src/components/           Reusable UI components
└── src/lib/                  Auth (MSAL), API client, utilities

infra/                        Terraform (Azure)
└── modules/                  app-service, sql, keyvault, monitoring

docs/                         Setup guides and design documentation
├── design.md                 Full data model, API design, phased plan, decisions log
└── entra-setup.md            Entra ID app registration walkthrough
```

## Build & Run

```bash
# .NET API (requires dotnet 8 SDK)
export DOTNET_ROOT="/opt/homebrew/opt/dotnet@8/libexec"
export PATH="$DOTNET_ROOT:$PATH"
cd api && dotnet build
cd api && dotnet test
cd api && dotnet run --project EnduranceTracker.Api

# Next.js frontend
cd web && npm install --legacy-peer-deps
cd web && npm run build
cd web && npm run dev

# Terraform
cd infra && terraform init && terraform validate
```

## Key Conventions

- **Clean Architecture**: Core has zero dependencies on Infrastructure or Api. Domain entities live in Core. EF Core stays in Infrastructure. Api is the composition root.
- **Guid primary keys** on all entities — no sequential IDs.
- **EF Core migrations** live in Infrastructure. Generate with: `dotnet ef migrations add <Name> --project EnduranceTracker.Infrastructure --startup-project EnduranceTracker.Api`
- **Entra ID auth** on the API via Microsoft.Identity.Web. CurrentUserMiddleware auto-provisions User records from JWT `oid` claim.
- **MSAL redirect flow** on the frontend (not popup) — works on mobile browsers.
- **Tremor** for dashboard charts and components. Installed with `--legacy-peer-deps` due to React 19 peer dep mismatch.
- **Next.js 16 has breaking changes** — always check `web/node_modules/next/dist/docs/` before writing frontend code.
- **Terraform modules** are modular per Azure resource type. All secrets via Key Vault, managed identity for access.
- **No secrets in committed files.** Use `.env.local` (frontend), `appsettings.Development.json` (API), `terraform.tfvars` (infra) — all gitignored.

## Git Workflow

- `main` — stable releases
- `develop` — active development
- Feature branches from `develop`: `feature/<name>`

## Current Status

- **Phase 1 (Foundation)**: Complete — solution scaffolded, domain entities, EF Core migration, Entra ID auth, Next.js with MSAL, Terraform modules
- **Phase 2 (Strava Integration)**: Not started
- **Phase 3 (Dashboard & Analytics)**: Not started
- **Phase 4 (Manual Entry & Strength)**: Not started
- **Phase 5 (Event Planning & Training Plans)**: Not started
