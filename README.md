# EnduranceTracker

A personal training analysis and planning platform that syncs activity data from Strava, supports manual entry for non-tracked activities (strength work), provides configurable dashboards for analysing training patterns, and generates AI-assisted training plans built around target events.

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│                        Azure                             │
│                                                          │
│  ┌──────────────┐    ┌──────────────┐   ┌────────────┐  │
│  │  Azure App   │    │  Azure App   │   │  Azure SQL │  │
│  │  Service     │◄──►│  Service     │◄─►│  Database  │  │
│  │  (Next.js)   │    │  (.NET API)  │   │            │  │
│  └──────┬───────┘    └──────┬───────┘   └────────────┘  │
│         │                   │                            │
│         │            ┌──────┴───────┐                    │
│         │            │   Strava     │                    │
│  ┌──────┴───────┐    │   Webhooks   │                    │
│  │  Entra ID    │    └──────────────┘                    │
│  │  (Auth)      │                                        │
│  └──────────────┘                                        │
└─────────────────────────────────────────────────────────┘
```

## Tech Stack

| Layer | Technology | Notes |
|-------|-----------|-------|
| Frontend | Next.js 16 (React, TypeScript, Tailwind) | Responsive web, mobile-friendly |
| Charts | Tremor | Dashboard components + charts |
| Backend API | .NET 8 (Web API) | RESTful, Clean Architecture |
| Database | Azure SQL | Free tier (32GB) |
| Auth | Microsoft Entra ID | OIDC/OAuth2 |
| Hosting | Azure App Service | Free tier |
| IaC | Terraform | All Azure resources managed |
| AI | Hybrid — Claude.ai manually, API integration later | Training plan generation |

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [Terraform 1.5+](https://www.terraform.io/downloads)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- An Azure subscription with Microsoft Entra ID

## Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/jacobjoemcguire/endurance-tracker.git
cd endurance-tracker
```

### 2. Set up Entra ID

Follow the guide in [docs/entra-setup.md](docs/entra-setup.md) to create the app registration and note your Client ID and Tenant ID.

### 3. Run the API

```bash
cd api

# Create local settings (gitignored)
cp EnduranceTracker.Api/appsettings.json EnduranceTracker.Api/appsettings.Development.json
# Edit appsettings.Development.json with your Entra ID and SQL connection string

dotnet build
dotnet run --project EnduranceTracker.Api
```

The API will be available at `http://localhost:5000`. Health check: `GET /api/health`.

### 4. Run the frontend

```bash
cd web

# Create local env (gitignored)
cp .env.example .env.local
# Edit .env.local with your Entra ID details

npm install --legacy-peer-deps
npm run dev
```

The frontend will be available at `http://localhost:3000`.

### 5. Infrastructure (optional — for Azure deployment)

```bash
cd infra
cp terraform.tfvars.example terraform.tfvars
# Edit terraform.tfvars with your values

terraform init
terraform plan
terraform apply
```

## Project Structure

```
endurance-tracker/
├── api/                          .NET 8 Web API
│   ├── EnduranceTracker.Api/     Controllers, middleware, composition root
│   ├── EnduranceTracker.Core/    Domain entities, enums, interfaces
│   ├── EnduranceTracker.Infrastructure/  EF Core, DbContext, migrations
│   └── EnduranceTracker.Tests/   xUnit tests
├── web/                          Next.js 16 frontend
│   └── src/
│       ├── app/                  App Router pages
│       ├── components/           UI components
│       └── lib/                  Auth, API client, utilities
├── infra/                        Terraform
│   └── modules/                  app-service, sql, keyvault, monitoring
└── docs/                         Setup guides and design docs
```

## Documentation

- [Design Document](docs/design.md) — data model, API design, phased implementation plan
- [Entra ID Setup](docs/entra-setup.md) — app registration walkthrough
