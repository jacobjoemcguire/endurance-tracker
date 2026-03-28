# TrainingPulse — Design Document

## Overview

A personal training analysis and planning platform that syncs activity data from Strava, supports manual entry for non-tracked activities (strength work), provides configurable dashboards for analysing training patterns, and generates AI-assisted training plans built around target events.

---

## 1. Architecture

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

### Tech Stack

| Layer          | Technology              | Notes                              |
|----------------|-------------------------|------------------------------------|
| Frontend       | Next.js (React)         | Responsive web, mobile-friendly    |
| Backend API    | .NET 8 (Web API)        | RESTful, Clean Architecture        |
| Database       | Azure SQL               | Free tier (32GB)                   |
| Auth           | Microsoft Entra ID      | OIDC/OAuth2, single user + future  |
| Hosting        | Azure App Service       | Free/Basic tier for both apps      |
| IaC            | Terraform               | All Azure resources managed        |
| AI             | Hybrid — manual (Claude.ai) now, API integration later | Training plan generation |
| Charts         | Tremor                  | Dashboard components + charts      |

---

## 2. Functional Requirements

### 2.1 Data Ingestion

| Requirement | Detail |
|-------------|--------|
| **Strava Sync** | OAuth2 connection to Strava API. Initial bulk import of ~5 years of history. |
| **Webhook Listener** | Strava webhook subscription for real-time activity sync (create, update, delete events). |
| **Manual Entry** | Form-based entry for strength/gym sessions with fields: date, type, duration, exercises, sets, reps, weight, notes. |
| **Activity Types** | Cycling, Running, Hiking, Strength (manual only). |

### 2.2 Analysis & Dashboard

| Requirement | Detail |
|-------------|--------|
| **Metrics** | Volume (hours/week), distance, elevation gain, TSS/load estimate, pace/speed trends. |
| **Activity Split** | All views filterable by activity type. |
| **Time Ranges** | Predefined (week, month, quarter, YTD, year) + custom date range. |
| **Saved Views** | User can create, name, and save dashboard filter configurations. |
| **Default View** | Recent 12 weeks, all activity types, summary cards + trend charts. |

**Recommended Dashboard Components:**

1. **Weekly Summary Cards** — hours, distance, elevation, activity count for current vs previous week
2. **Training Load Chart** — rolling weekly volume over time (stacked by activity type)
3. **Activity Distribution** — pie/donut chart showing split across activity types
4. **Elevation Profile** — weekly/monthly elevation gain trends
5. **Pace/Speed Trends** — rolling averages for cycling (speed) and running (pace)
6. **Calendar Heatmap** — training density/consistency view
7. **Event Countdown** — days to next event, with plan compliance %
8. **Personal Records** — distance, elevation, longest ride/run, power bests (if available)

### 2.3 Event Planning

| Requirement | Detail |
|-------------|--------|
| **Multiple Events** | Support concurrent events with different disciplines. |
| **Event Fields** | Name, date, discipline (cycling/running), distance, elevation, target time (optional), priority, notes. |
| **Event Types** | Trail running events, endurance/ultra-endurance cycling. |
| **Timeline View** | Visual timeline showing upcoming events and plan phases. |

### 2.4 Training Plans

| Requirement | Detail |
|-------------|--------|
| **Generation** | AI-generated plans working backwards from event date. |
| **Input** | Event details + current fitness level (derived from history) + user preferences. |
| **Historical Awareness** | AI considers what user enjoys (activity patterns, preferred ride types, volume tolerance) and adjusts. |
| **Periodisation** | Base → Build → Peak → Taper phases, appropriate to discipline. |
| **Strength Integration** | Include strength/gym sessions in plan structure. |
| **Compliance Tracking** | Planned vs actual — weekly adherence %, deviation alerts. |
| **Adjustability** | User can modify/override individual sessions. AI can re-plan remaining weeks if user falls behind or ahead. |

### 2.5 Auth & User

| Requirement | Detail |
|-------------|--------|
| **Provider** | Microsoft Entra ID (OIDC) |
| **Scope** | Single user now, multi-user capable schema |
| **Session** | JWT-based, secure cookies |
| **Strava OAuth** | Separate OAuth flow to link Strava account |

---

## 3. Non-Functional Requirements

| Category | Requirement |
|----------|-------------|
| **Performance** | Dashboard charts render < 1s. API responses < 500ms for dashboard queries. Paginate/lazy-load historical data. |
| **Security** | HTTPS/SSL enforced. Entra ID auth on all routes. Secrets in Azure Key Vault. Strava tokens encrypted at rest. |
| **Availability** | Azure App Service SLA. Acceptable maintenance windows for personal use. |
| **Cost** | Favour free tier: Azure SQL Free (32GB), App Service Free/Basic. Monitor costs. |
| **Scalability** | Single user now. UserId on all tables for future multi-tenancy. |
| **Observability** | Application Insights for API. Structured logging. |
| **Mobile** | Responsive design — usable on phone for quick checks. Full experience on desktop. |

---

## 4. Data Model (Core Entities)

```
User
├── Id, EntraObjectId, DisplayName, StravaAthleteId, StravaTokens
│
├── Activity
│   ├── Id, UserId, Source (Strava|Manual), StravaActivityId
│   ├── Type (Ride|Run|Hike|Strength), Name, Date
│   ├── Duration, Distance, ElevationGain, AvgSpeed, AvgPace
│   ├── AvgHeartRate, MaxHeartRate, Calories
│   └── RawJson (Strava payload for future re-processing)
│
├── StrengthDetail (for manual strength entries)
│   ├── ActivityId, Exercise, Sets, Reps, Weight, Notes
│
├── Event
│   ├── Id, UserId, Name, Date, Discipline, Distance
│   ├── ElevationGain, TargetTime, Priority, Notes
│
├── TrainingPlan
│   ├── Id, UserId, EventId, GeneratedDate, Status
│   ├── PlanJson (full structured plan)
│   │
│   └── PlannedSession
│       ├── Id, PlanId, Date, Type, Description
│       ├── TargetDuration, TargetDistance, TargetIntensity
│       └── CompletedActivityId (nullable FK → Activity)
│
└── SavedView
    ├── Id, UserId, Name, FilterJson, IsDefault
```

---

## 5. API Design (Key Endpoints)

```
# Auth
GET  /api/auth/login          → Entra ID redirect
GET  /api/auth/callback       → Token exchange
POST /api/auth/logout

# Strava
GET  /api/strava/connect      → Strava OAuth redirect
GET  /api/strava/callback     → Token exchange + initial sync trigger
POST /api/strava/webhook      → Webhook receiver (activity events)

# Activities
GET  /api/activities           → List (filterable, paginated)
GET  /api/activities/:id       → Detail
POST /api/activities           → Manual entry (strength etc.)
PUT  /api/activities/:id       → Update manual entry
DELETE /api/activities/:id     → Delete manual entry

# Analytics
GET  /api/analytics/summary    → Summary stats (date range, activity type filters)
GET  /api/analytics/trends     → Time-series data for charts
GET  /api/analytics/records    → Personal bests

# Events
GET  /api/events               → List upcoming events
POST /api/events               → Create event
PUT  /api/events/:id           → Update
DELETE /api/events/:id         → Delete

# Training Plans
POST /api/plans/generate       → AI generate plan for event
GET  /api/plans/:id            → Plan detail with sessions
PUT  /api/plans/:id/sessions/:sid → Mark session complete / modify
POST /api/plans/:id/replan     → AI re-plan remaining weeks

# Dashboard
GET  /api/views                → List saved views
POST /api/views                → Save view config
PUT  /api/views/:id            → Update
DELETE /api/views/:id          → Delete
```

---

## 6. Infrastructure (Terraform)

```
Azure Resources:
├── Resource Group
├── App Service Plan (Free/Basic)
│   ├── App Service — Next.js frontend
│   └── App Service — .NET API
├── Azure SQL Server
│   └── Azure SQL Database (Free tier)
├── Key Vault (secrets, Strava tokens)
├── Application Insights
├── Entra ID App Registration
│   ├── Frontend redirect URIs
│   └── API permissions
└── DNS / Custom Domain + Managed SSL Certificate (optional)
```

---

## 7. Project Structure

```
training-analysis-plan/
├── infra/                    # Terraform
│   ├── main.tf
│   ├── variables.tf
│   ├── outputs.tf
│   └── modules/
│       ├── app-service/
│       ├── sql/
│       ├── keyvault/
│       └── monitoring/
├── api/                      # .NET Web API
│   ├── TrainingPulse.Api/
│   ├── TrainingPulse.Core/       # Domain models, interfaces
│   ├── TrainingPulse.Infrastructure/ # DB, Strava client, AI service
│   └── TrainingPulse.Tests/
├── web/                      # Next.js frontend
│   ├── src/
│   │   ├── app/              # App router pages
│   │   ├── components/       # UI components
│   │   ├── lib/              # API client, auth, utils
│   │   └── hooks/            # Custom React hooks
│   └── public/
├── DESIGN.md
└── README.md
```

---

## 8. Implementation Phases

### Phase 1 — Foundation
- Terraform infrastructure (App Services, SQL, Key Vault, App Insights)
- .NET API scaffolding with Entra ID auth
- Next.js app with Entra ID login
- Database schema + EF Core migrations
- CI stub

### Phase 2 — Strava Integration
- Strava OAuth flow (connect/disconnect)
- Bulk historical import (~5 years)
- Webhook subscription for real-time sync
- Activity list + detail views

### Phase 3 — Dashboard & Analytics
- Summary cards, training load chart, activity split
- Calendar heatmap, pace/speed trends, elevation trends
- Saved views (create, load, delete)
- Time range filters + activity type filters

### Phase 4 — Manual Entry & Strength
- Strength session entry form (exercises, sets, reps, weight)
- Strength activities in dashboard views
- Activity type = Strength in all filters/charts

### Phase 5 — Event Planning & Training Plans
- Event CRUD + timeline view
- Plan data model: import/editor UI for manually created plans
- Plan creation workflow: export fitness data → use Claude.ai to generate plan → import into app
- Planned vs actual compliance tracking
- Re-planning: manually re-generate via Claude.ai and re-import

### Phase 5b — AI API Integration (future)
- Abstract AI behind interface (ITrainingPlanGenerator)
- Integrate Azure OpenAI or Claude API for in-app plan generation
- Historical fitness analysis for automated plan personalisation
- One-click re-planning

### Phase 6 — Polish
- Mobile responsive refinements
- Personal records page
- Performance tuning (query optimisation, caching)

---

## 9. Decisions Log

| Decision | Choice | Rationale |
|----------|--------|-----------|
| AI Approach | Hybrid (Option C) — manual via Claude.ai first, API integration later | Get event planning + compliance tracking without API cost. Plug in automation when ready. |
| Chart Library | Tremor | Built for Next.js dashboards, includes cards + charts + layout primitives. Fastest to ship. |
| Custom Domain | Deferred | App Service provides *.azurewebsites.net with SSL. Add custom domain later if needed. |

## 10. Open Decisions

| Decision | Options | Notes |
|----------|---------|-------|
| AI Provider (Phase 5b) | Azure OpenAI vs Claude API | Decide when ready to automate. Abstract behind interface so it's swappable. |
| Strava Rate Limits | 100 req/15min, 1000/day | Bulk import needs throttling. Webhook handles real-time. |
| Plan Import Format | JSON vs structured form vs paste | How plans from Claude.ai get into the app. TBD in Phase 5. |
