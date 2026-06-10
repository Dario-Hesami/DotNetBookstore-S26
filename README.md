# 📚 DotNet Bookstore

A full-featured ASP.NET Core MVC web application for managing an online bookstore catalogue. Built as part of the **COMP2084** course at **Georgian College** (Summer 2026).

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap)](https://getbootstrap.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-10.0-brightgreen)](https://docs.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Azure-CC2927?logo=microsoftsqlserver)](https://azure.microsoft.com/en-us/products/azure-sql/)

---

## 🌐 Live Repository

[https://github.com/Dario-Hesami/DotNetBookstore-S26](https://github.com/Dario-Hesami/DotNetBookstore-S26)

---

## 📋 Table of Contents

- [Overview](#overview)
- [Technology Stack](#technology-stack)
- [Features](#features)
- [UI/UX Design](#uiux-design)
- [Data Models](#data-models)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Available Routes](#available-routes)
- [Documentation](#documentation)
- [UI Enhancement History](#ui-enhancement-history)

---

## Overview

DotNet Bookstore is an ASP.NET Core MVC CRUD application that manages a bookstore's catalogue of books and categories. It uses Entity Framework Core with SQL Server (Azure) for data persistence, ASP.NET Core Identity for authentication, and Bootstrap 5 for a polished, responsive UI.

The project demonstrates real-world patterns including:

- One-to-many relational data (Category → Books)
- EF Core eager loading with `.Include()`
- Scaffolded CRUD controllers with post-scaffolding fixes
- ASP.NET Core Identity (registration, login, logout)
- Bootstrap 5 responsive layout with Bootstrap Icons

---

## Technology Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Language | C# 14 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server (Azure SQL Database) |
| Authentication | ASP.NET Core Identity |
| Frontend | Bootstrap 5, Bootstrap Icons 1.11.3, jQuery |
| IDE | Visual Studio Enterprise 2026 |

---

## Features

### Books Management
- **List** all books in a hover-table with cover image thumbnails, coloured Mature Content badges, category badges, and icon action buttons
- **Create** a new book with author, title, image URL, price, mature content toggle, and category dropdown
- **Edit** an existing book — category dropdown pre-selects the current value
- **View Details** — card layout with cover image, all fields, and colour-coded badges
- **Delete** — danger-themed confirmation card with full book summary

### Categories Management
- **List** all categories in a clean hover-table
- **Create**, **Edit**, **View Details**, and **Delete** categories with consistent card-wrapped forms and danger confirmation flow

### Authentication
- User registration and login via ASP.NET Core Identity
- Email confirmation required (`RequireConfirmedAccount = true`)
- Navbar displays logged-in username with manage and logout links

---

## UI/UX Design

The entire frontend was built and enhanced using **Bootstrap 5** with **Bootstrap Icons 1.11.3** (loaded via CDN). A consistent design language is applied across all 14 views.

### Global Layout (`_Layout.cshtml`)
- **Dark navbar** (`bg-dark`) with Bootstrap Icons in all nav links and the brand logo
- Sticky **flex footer** (`bg-dark`) with Privacy and GitHub source links
- `min-vh-100` flex-column body keeps the footer at the bottom on short pages

### Home Page
- **Hero section** — dark jumbotron with a large book icon, headline, description, and two CTA buttons (Browse Books / View Categories)
- **Feature cards** — three equal-height cards for Discover Books, Browse Categories, and Manage Your Account

### Books Views

| View | Enhancement |
|---|---|
| `Index` | `table-dark` header, `table-hover`, cover `<img>` thumbnail, colour badge for Mature Content, category badge, icon-only action buttons, empty-state message |
| `Create` / `Edit` | Centred card form, `input-group` with Bootstrap Icons, `form-switch` for Mature Content, Bootstrap 5 `mb-3` spacing, Save/Cancel buttons |
| `Details` | Card with cover image at top, `dl` info grid, colour-coded badges for Mature Content and Category |
| `Delete` | Danger-bordered card, `alert-danger` warning banner, full book summary, Yes/Cancel buttons |

### Categories Views

| View | Enhancement |
|---|---|
| `Index` | Dark table header, `table-hover`, icon-only action buttons, empty-state message |
| `Create` / `Edit` | Centred card form with tag icon input-group |
| `Details` | Card with dark header showing category name |
| `Delete` | Danger-bordered card with `alert-danger` warning, confirm/cancel buttons |

### Shared
- **Error page** — large danger icon, styled `alert-secondary` for Request ID, `card` for dev-mode info, Return Home button
- **Privacy page** — card with dark header, back navigation link
- **Login Partial** — Bootstrap Icons on all auth links (Register, Login, Logout, Manage)

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
│   │   ├── _Layout.cshtml          # Dark navbar, Bootstrap Icons CDN, sticky footer
│   │   ├── _LoginPartial.cshtml    # Auth nav links with icons
│   │   └── Error.cshtml            # Styled error card
│   ├── Home/
│   │   ├── Index.cshtml            # Hero section + feature cards
│   │   └── Privacy.cshtml          # Card layout
│   ├── Books/
│   │   ├── Index.cshtml            # Hover table, image thumbnails, badges
│   │   ├── Create.cshtml           # Card form with input-group icons
│   │   ├── Edit.cshtml             # Card form, pre-selected category dropdown
│   │   ├── Details.cshtml          # Cover image + info card with badges
│   │   └── Delete.cshtml           # Danger confirmation card
│   └── Categories/
│       ├── Index.cshtml            # Hover table with icon action buttons
│       ├── Create.cshtml           # Card form
│       ├── Edit.cshtml             # Card form
│       ├── Details.cshtml          # Card with dark header
│       └── Delete.cshtml           # Danger confirmation card
├── Areas/
│   └── Identity/                   # Scaffolded Identity pages
├── wwwroot/
│   ├── css/site.css
│   ├── js/site.js
│   └── lib/                        # Bootstrap 5, jQuery, jQuery Validation
├── Docs/
│   ├── Post-Scaffolding-Guide-OneToMany.md
│   └── Post-Model-Change-Migrations-Guide.md
├── appsettings.json                # Connection string (do not commit credentials)
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
| `/` | Home page (hero + feature cards) |
| `/Home/Privacy` | Privacy policy page |
| `/Books` | Books list |
| `/Books/Create` | Add a new book |
| `/Books/Edit/{id}` | Edit a book |
| `/Books/Details/{id}` | Book detail view |
| `/Books/Delete/{id}` | Delete confirmation |
| `/Categories` | Categories list |
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
| **UI/UX Enhancement (Bootstrap 5)** | Full overhaul of all 14 views: dark navbar with Bootstrap Icons 1.11.3 (CDN), hero home page with feature cards, hover tables with image thumbnails and colour badges, card-wrapped forms with `input-group` icons, `form-switch` for boolean fields, danger confirmation cards, styled error page, sticky dark footer |

---

*Built with ❤️ at Georgian College · COMP2084 Summer 2026*
