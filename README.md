# DotNet Bookstore

A full-featured ASP.NET Core MVC web application for managing an online bookstore catalogue. Built as part of the **COMP2084** course at **Georgian College** (Summer 2026).

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap)](https://getbootstrap.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-10.0-brightgreen)](https://docs.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Azure-CC2927?logo=microsoftsqlserver)](https://azure.microsoft.com/en-us/products/azure-sql/)

---

## Live Repository

[https://github.com/Dario-Hesami/DotNetBookstore-S26](https://github.com/Dario-Hesami/DotNetBookstore-S26)

---

## Table of Contents

- [Overview](#overview)
- [Technology Stack](#technology-stack)
- [Features](#features)
- [UI/UX Design](#uiux-design)
- [Data Models](#data-models)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Opening in VS Code](#opening-in-vs-code)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Available Routes](#available-routes)
- [Documentation](#documentation)
- [UI Enhancement History](#ui-enhancement-history)

---

## Overview

DotNet Bookstore is an ASP.NET Core MVC CRUD application that manages a bookstore's catalogue of books and categories. It uses Entity Framework Core with SQL Server (Azure) for data persistence, ASP.NET Core Identity for authentication, and a fully custom Bootstrap 5 design system for a modern, responsive UI.

The project demonstrates real-world patterns including:

- One-to-many relational data (Category → Books)
- EF Core eager loading with `.Include()`
- Scaffolded CRUD controllers with post-scaffolding fixes
- ASP.NET Core Identity (registration, login, logout)
- Bootstrap 5 responsive layout with Bootstrap Icons
- CSS custom properties for a consistent design token system
- Reactive UI elements (live cover image preview, empty states, hover effects)

---

## Technology Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Language | C# 14 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server (Azure SQL Database / LocalDB) |
| Authentication | ASP.NET Core Identity |
| Frontend | Bootstrap 5, Bootstrap Icons 1.11.3 (CDN), jQuery |
| Custom CSS | `site.css` — full design system with CSS custom properties |
| IDE | Visual Studio Enterprise 2026 / VS Code + C# Dev Kit |

---

## Features

### Books Management

- **List** all books in a **responsive card grid** (1 → 2 → 3 → 4 columns) with cover art, category badge, Mature Content badge, author, title, and price
- **Create** a new book with author, title, image URL (with live cover preview), price, mature content toggle, and category dropdown
- **Edit** an existing book — live cover preview loads from the existing URL on page open; category dropdown pre-selects the current value
- **View Details** — two-column card layout with full-height cover image on the left and metadata on the right
- **Delete** — danger-themed confirmation card with a cover thumbnail and full book summary

### Categories Management

- **List** all categories in a **responsive card grid** (1 → 2 → 3 → 4 columns) with icon, name, and action buttons
- **Create**, **Edit**, **View Details**, and **Delete** categories with consistent card-wrapped forms and danger confirmation flow

### Authentication
- User registration and login via ASP.NET Core Identity
- Email confirmation required (`RequireConfirmedAccount = true`)
- Navbar displays logged-in username with manage and logout links

### UX Enhancements

- **Active nav link** highlighting — current controller auto-detected at render time
- **TempData toast area** in the layout — controllers can set `TempData["SuccessMessage"]` or `TempData["ErrorMessage"]` for dismissible Bootstrap alerts
- **Empty states** on index pages — icon, message, and CTA when no records exist
- **Live cover preview** — image renders below the URL field as you type; disappears silently on a broken URL
- **Graceful image fallback** — gradient placeholder with a book icon is always rendered behind cover images; shows through on `onerror` with no JS required

---

## UI/UX Design

All views share a unified design language built on **Bootstrap 5** and a custom CSS design token system in `site.css`. Bootstrap Icons 1.11.3 is loaded via CDN.

### Design Tokens (`site.css`)

A set of CSS custom properties drives the entire visual theme:

```css
--brand-gradient        /* deep indigo → purple → indigo (navbar, footer) */
--hero-gradient         /* darker 3-stop indigo gradient (home hero) */
--brand-gradient-light  /* soft indigo→lavender (card covers, placeholders) */
--card-shadow           /* subtle multi-layer shadow */
--card-shadow-hover     /* lifted indigo-tinted shadow on hover */
--radius-card           /* .75rem — all cards and alerts */
--radius-btn            /* .5rem — all buttons and inputs */
--transition            /* 220ms ease — card and button transitions */
```

### Global Layout (`_Layout.cshtml`)

- **Navbar** — deep indigo gradient (`--brand-gradient`), amber brand badge icon, active link highlighted in gold, smooth background-fade on hover. Active state is detected server-side via `ViewContext.RouteData.Values["controller"]`.
- **Footer** — matching indigo gradient, responsive three-column layout (brand name / copyright / Privacy + GitHub links).
- **TempData alerts** — success and error banners rendered automatically between the navbar and main content area; dismissible via Bootstrap's `btn-close`.
- `min-vh-100` flex-column body keeps the footer pinned at the bottom on short pages.

### Home Page (`Home/Index.cshtml`)

- **Hero section** — full-width indigo/purple gradient card with an SVG dot-pattern texture overlay, large display heading with amber accent, lead text, and two CTA buttons (Browse Books / Categories).
- **Feature cards** — three equal-height cards (Books, Categories, Account) with lift-on-hover effect, coloured icon badges, and action buttons.
- **Tech strip** — a secondary info band showing Identity, EF Core, and Bootstrap with icons.

### Books Views

| View | Design |
|---|---|
| `Index` | Responsive card grid; cover art with gradient-placeholder fallback behind image; category + Mature badges; price in green; icon action buttons in card footer |
| `Create` / `Edit` | Centred card form; `input-group` with Bootstrap Icons; small uppercase muted labels; live cover image preview below URL field; Mature Content toggle in a highlighted box; `divider-gradient` separator before action buttons |
| `Details` | Two-column card — full-height cover (or gradient placeholder) on left, metadata on right; category and rating badges; price in large green text |
| `Delete` | Red-bordered card with cover thumbnail, two-column summary; danger alert banner with triangle icon; "Delete Permanently" button |

### Categories Views

| View | Design |
|---|---|
| `Index` | Responsive card grid; tag icon in a rounded success-tinted badge; lift-on-hover; icon action buttons in card footer |
| `Create` / `Edit` | Centred card form; tag icon `input-group`; `autofocus` on Create; divider before buttons |
| `Details` | Gradient header band (indigo-subtle) with tag icon + category name; Edit / Delete / Back buttons |
| `Delete` | Danger header band + danger alert warning about book assignments; "Delete Permanently" button |

### Shared Components

- **Error page** — large danger octagon icon, `alert-secondary` for Request ID, yellow-tinted card for dev-mode info
- **Privacy page** — centred card with shield icon and divider
- **Empty states** — faint indigo icon, muted heading, small descriptor text, and a CTA button; used on both Books and Categories index pages

---

## Data Models

```
Category
├── CategoryId (PK)
└── Name (required)

Book
├── BookId (PK)
├── Author (required, max 100)
├── Title (required, max 200)
├── Image (nullable URL string)
├── Price (required, 0.01–10000)
├── MatureContent (bool)
├── CategoryId (FK → Category)
└── Category (navigation property)

CartItem
├── CartItemId (PK)
├── Quantity (required, 1–1000)
├── Price (required)
├── BookId (FK → Book)
└── UserId (FK → IdentityUser)

Order
├── OrderId (PK)
├── OrderDate / OrderTotal
├── FirstName / LastName / Address
├── City / Province / PostalCode
├── Phone / Email
└── UserId (FK → IdentityUser)

OrderDetail
├── OrderDetailId (PK)
├── Quantity / Price
├── BookId (FK → Book)
└── OrderId (FK → Order)
```

---

## Project Structure

```
DotNetBookstore/
├── Controllers/
│   ├── HomeController.cs
│   ├── BooksController.cs          # Full CRUD with .Include() eager loading
│   └── CategoriesController.cs     # Full CRUD
├── Data/
│   ├── ApplicationDbContext.cs     # EF Core DbContext (Identity + 5 DbSets)
│   └── Migrations/                 # EF Core migration files
├── Models/
│   ├── Book.cs
│   ├── Category.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   ├── OrderDetail.cs
│   └── ErrorViewModel.cs
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml          # Indigo gradient navbar + footer, active nav, toast area
│   │   ├── _LoginPartial.cshtml    # Auth nav links with Bootstrap Icons
│   │   └── Error.cshtml            # Styled error page with icon and dev-mode card
│   ├── Home/
│   │   ├── Index.cshtml            # Hero section, feature cards, tech strip
│   │   └── Privacy.cshtml          # Centred card with shield icon
│   ├── Books/
│   │   ├── Index.cshtml            # Responsive card grid with cover art and empty state
│   │   ├── Create.cshtml           # Card form with live cover preview
│   │   ├── Edit.cshtml             # Card form with live cover preview (pre-loaded)
│   │   ├── Details.cshtml          # Two-column cover + metadata card
│   │   └── Delete.cshtml           # Danger-bordered confirmation card with thumbnail
│   └── Categories/
│       ├── Index.cshtml            # Responsive card grid with empty state
│       ├── Create.cshtml           # Card form with autofocus
│       ├── Edit.cshtml             # Card form
│       ├── Details.cshtml          # Gradient header band card
│       └── Delete.cshtml           # Danger header band confirmation card
├── Areas/
│   └── Identity/                   # Scaffolded Identity pages
├── wwwroot/
│   ├── css/site.css                # Custom design system (tokens, components, utilities)
│   ├── js/site.js
│   └── lib/                        # Bootstrap 5, jQuery, jQuery Validation
├── Docs/
│   ├── Post-Scaffolding-Guide-OneToMany.md
│   └── Post-Model-Change-Migrations-Guide.md
├── appsettings.json                # Connection string (gitignored — do not commit credentials)
├── appsettings.Development.json
├── Program.cs
└── DotNetBookstore.csproj          # .NET 10, EF Core 10, Identity packages
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) (or VS Code with C# Dev Kit)
- SQL Server (local instance or Azure SQL Database)

### Clone the Repository

```bash
git clone https://github.com/Dario-Hesami/DotNetBookstore-S26.git
cd DotNetBookstore-S26
```

### Restore Dependencies

```bash
dotnet restore DotNetBookstore/DotNetBookstore.csproj
```

### Run the Application

```bash
dotnet run --project DotNetBookstore/DotNetBookstore.csproj
```

Or press **F5** in Visual Studio to launch with debugging. The default launch URLs are:

- `https://localhost:7296`
- `http://localhost:5170`

---

## Opening in VS Code

This repository includes a `.vscode/` configuration folder for a full VS Code development experience. These files are completely ignored by Visual Studio 2026 and do not affect that workflow in any way.

### VS Code Configuration Files

| File | Purpose |
|---|---|
| `.vscode/launch.json` | Two debug configs: **Launch (http)** on port 5170, **Launch (https)** on ports 7296/5170, plus Attach |
| `.vscode/tasks.json` | `build` (default Ctrl+Shift+B), `publish`, and `watch` tasks via `dotnet` CLI |
| `.vscode/settings.json` | Points to `DotNetBookstore.slnx`, format-on-save for C# and Razor, hides `bin/` and `obj/` from Explorer |
| `.vscode/extensions.json` | Workspace-recommended extensions (auto-prompted on first open) |

### Required Extensions

Accept the workspace prompt on first open, or install manually from the Extensions panel:

| Extension ID | Purpose |
|---|---|
| `ms-dotnettools.csharp` | C# language support — IntelliSense, diagnostics, go-to-definition |
| `ms-dotnettools.csdevkit` | C# Dev Kit — Solution Explorer, test runner, enhanced refactoring |
| `ms-dotnettools.vscode-dotnet-runtime` | .NET runtime installer used by the above extensions |

### Steps to Open and Run

1. **Open the folder** — `File → Open Folder` → select the `DotNetBookstore-S26/` root directory.
2. **Install extensions** — accept the prompt to install recommended extensions, then reload VS Code.
3. **Create `appsettings.json`** — this file is gitignored (keeps credentials out of source control). Create it at `DotNetBookstore/appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DotNetBookstoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   ```

   > Alternatively, store the connection string in [User Secrets](#️-connection-string-security) instead of `appsettings.json`.

4. **Apply migrations** (first run only — skip if the database already exists from Visual Studio):

   ```bash
   dotnet ef database update --project DotNetBookstore/DotNetBookstore.csproj
   ```

5. **Run / Debug** — press `F5`, select **Launch (http)** or **Launch (https)** from the dropdown, and VS Code will build, start the app, and open the browser automatically.

   For hot-reload development, use the `watch` task instead (`Terminal → Run Task → watch`):

   ```bash
   dotnet watch run --project DotNetBookstore/DotNetBookstore.csproj
   ```

### Trust the HTTPS Dev Certificate (once per machine)

```bash
dotnet dev-certs https --trust
```

---

## Database Setup

The application uses **EF Core code-first** with two migrations:

| Migration | Description |
|---|---|
| `00000000000000_CreateIdentitySchema` | ASP.NET Core Identity tables |
| `20260529000439_CreateBookstoreTables` | Categories, Books, CartItems, Orders, OrderDetails |

Apply all migrations to a new database:

```bash
dotnet ef database update --project DotNetBookstore/DotNetBookstore.csproj
```

If the EF CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Add a new migration after model changes:

```bash
dotnet ef migrations add YourMigrationName --project DotNetBookstore/DotNetBookstore.csproj
dotnet ef database update --project DotNetBookstore/DotNetBookstore.csproj
```

See [`Docs/Post-Model-Change-Migrations-Guide.md`](DotNetBookstore/Docs/Post-Model-Change-Migrations-Guide.md) for detailed guidance.

---

## Configuration

### ⚠️ Connection String Security

**Never commit real credentials to source control.** Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) for local development:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string" --project DotNetBookstore/DotNetBookstore.csproj
```

### Connection String Examples

**Local SQL Server (Windows Auth):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=dotnetbookstore;Trusted_Connection=True"
  }
}
```

**Azure SQL Database:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server.database.windows.net;Database=dotnetbookstore-azure;User Id=dba;Password=YOUR_PASSWORD;TrustServerCertificate=true"
  }
}
```

---

## Available Routes

| Route | Description |
|---|---|
| `/` | Home page (hero + feature cards + tech strip) |
| `/Home/Privacy` | Privacy policy page |
| `/Books` | Books card grid |
| `/Books/Create` | Add a new book (with live cover preview) |
| `/Books/Edit/{id}` | Edit a book (cover preview pre-loaded) |
| `/Books/Details/{id}` | Two-column book detail view |
| `/Books/Delete/{id}` | Delete confirmation with thumbnail |
| `/Categories` | Categories card grid |
| `/Categories/Create` | Add a new category |
| `/Categories/Edit/{id}` | Edit a category |
| `/Categories/Details/{id}` | Category detail view |
| `/Categories/Delete/{id}` | Delete confirmation |
| `/Identity/Account/Register` | User registration |
| `/Identity/Account/Login` | User login |

Default route pattern: `{controller=Home}/{action=Index}/{id?}`

---

## Documentation

The `Docs/` folder contains detailed developer guides:

| Document | Description |
|---|---|
| [`Post-Scaffolding-Guide-OneToMany.md`](DotNetBookstore/Docs/Post-Scaffolding-Guide-OneToMany.md) | Step-by-step fixes for scaffolded CRUD after adding one-to-many relationships — FK dropdowns, eager loading, Bind list trimming, dropdown repopulation |
| [`Post-Model-Change-Migrations-Guide.md`](DotNetBookstore/Docs/Post-Model-Change-Migrations-Guide.md) | Guide for safely adding EF Core migrations after model changes |

---

## UI Enhancement History

| Phase | Changes |
|---|---|
| Initial scaffold | Basic scaffolded CRUD views with default Bootstrap styling |
| Post-scaffolding fixes | Fixed FK dropdowns (`CategoryId` → `<select>`), added `.Include()` eager loading, replaced raw FK integers with human-readable category names, removed reverse navigation collection columns |
| Bootstrap 5 UI pass | Full overhaul of all 14 views: dark navbar with Bootstrap Icons 1.11.3 (CDN), hero home page with feature cards, hover tables with image thumbnails and colour badges, card-wrapped forms with `input-group` icons, `form-switch` for boolean fields, danger confirmation cards, styled error page, sticky dark footer |
| **Modern Design System** | Complete redesign of all 15 views and `site.css`: indigo/purple brand gradient replacing plain dark; CSS custom property token system (`--brand-gradient`, `--card-shadow`, `--radius-card`, etc.); Books and Categories index pages converted from tables to responsive card grids; live cover image preview (JS) in Create/Edit forms; layered gradient placeholder + real image on all cover slots with silent `onerror` fallback; two-column Details view (cover + metadata); active nav link via server-side controller detection; TempData toast area in layout; empty states with icon + CTA; page-header component with left accent border; `divider-gradient` separator; lift-on-hover transitions on all interactive cards and buttons |

---

Built with Georgian College · COMP2084 Summer 2026
