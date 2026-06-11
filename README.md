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
- [Educational Comments](#educational-comments)
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
- Reactive UI elements (live cover image preview via FileReader API, empty states, hover effects)
- **File upload** — cover images uploaded via `multipart/form-data`, saved to `wwwroot/img/books/` with GUID-prefixed filenames; old files deleted on replace or book removal

---

## Educational Comments

> **Students and learners — please read this section before diving into the code.**

Every source file in this repository has been annotated with two layers of purposeful, curriculum-aligned comments. They are not boilerplate — they are a structured learning resource written specifically for this course. Treat them as an integrated textbook that lives right next to the code it explains.

### What was added and why

| Layer | Where you find it | What it teaches |
| --- | --- | --- |
| **File header block** | Top of every `.cs`, `.cshtml`, `.css`, and `.js` file | States the file's purpose in plain English and lists the specific ASP.NET Core / C# concepts demonstrated inside |
| **Inline comments** | Alongside individual lines, blocks, and decisions throughout each file | Explains *why* the code is written the way it is — the reasoning, the trade-offs, and the common mistakes it avoids |

### Commented files and their key concepts

| File | Key concepts covered in the comments |
|---|---|
| `Program.cs` | `WebApplicationBuilder`, Dependency Injection, EF Core registration, Identity setup, middleware pipeline order, convention-based routing |
| `Data/ApplicationDbContext.cs` | `DbContext` vs `IdentityDbContext`, `DbSet<T>` as a table, constructor injection of `DbContextOptions`, migrations workflow, optional Fluent API |
| `Models/Category.cs` | POCO model, primary key by convention, `[Required]` / `[DisplayName]` data annotations, navigation properties |
| `Models/Book.cs` | Foreign key property, navigation property, `[ValidateNever]`, `null!` null-forgiving operator, `[Range]`, `[DisplayFormat]` |
| `Models/CartItem.cs` | Database-persisted cart pattern, price snapshot, `CustomerId` as identity key, FK + navigation property pair |
| `Models/Order.cs` | Order header / detail split pattern, `decimal` for currency, capturing shipping data at checkout time |
| `Models/OrderDetail.cs` | Junction/bridge table concept, two FKs in one model, price capture, `[ValidateNever]` on navigation properties |
| `Models/ErrorViewModel.cs` | ViewModel vs. entity, expression-bodied property (`=>`), nullable reference types |
| `Controllers/HomeController.cs` | MVC controller anatomy, action methods, `IActionResult`, `return View()`, `[ResponseCache]` |
| `Controllers/CategoriesController.cs` | Full CRUD HTTP mapping, `async`/`await`, `[HttpPost]`, `[ValidateAntiForgeryToken]`, `[Bind]`, `ModelState.IsValid`, `DbUpdateConcurrencyException`, PRG pattern |
| `Controllers/BooksController.cs` | All of the above **plus**: eager loading with `.Include()`, `SelectList` + `ViewBag`, repopulating dropdowns on validation failure, the 11 numbered post-scaffolding fixes; `IFormFile` file upload, `IWebHostEnvironment.WebRootPath`, GUID filename strategy, `UploadImage()` / `DeleteImage()` helpers, preserving existing image on edit, deleting orphaned files on delete, `bool deleteImage` parameter for explicit cover removal |
| `Views/Shared/_Layout.cshtml` | Master layout template, `@RenderBody()`, `@RenderSectionAsync`, TempData flash messages, active nav link detection, `asp-append-version` cache busting |
| `Views/Shared/_LoginPartial.cshtml` | Partial views, `@inject` in Razor, `SignInManager`, logout as a POST request (CSRF prevention) |
| `Views/Shared/_ValidationScriptsPartial.cshtml` | Client-side vs. server-side validation, jQuery Validate, unobtrusive validation bridge |
| `Views/Shared/Error.cshtml` | `@model` directive, ViewModel in a view, production vs. development error pages |
| `Views/_ViewImports.cshtml` | Global `@using` namespaces, `@addTagHelper`, Tag Helper overview |
| `Views/_ViewStart.cshtml` | `_ViewStart` implicit hook, setting the default layout, per-view overrides |
| `Views/Home/Index.cshtml` | Razor code blocks, `@*...*@` server-side comments, `asp-controller`/`asp-action`/`asp-route-id`, Bootstrap grid, responsive utilities; anonymous-type mock arrays (`new[] { new { … } }`), `@foreach` rendering, custom card classes for bestsellers and featured books |
| `Views/Books/Index.cshtml` | Strongly-typed `@model`, `.ToList()` to avoid double-enumeration, `@foreach`, conditional rendering, `asp-route-id`, empty state UX |
| `Views/Books/Create.cshtml` | `asp-action` on `<form>`, `enctype="multipart/form-data"` for file upload, `<input type="file">` with `accept`, `asp-for` on labels and text inputs, `asp-validation-for`, `asp-items` + `ViewBag` dropdown, FileReader API live preview IIFE, `@section Scripts` |
| `Views/Books/Edit.cshtml` | Hidden `BookId` field, `enctype="multipart/form-data"`, existing cover thumbnail with `~/img/books/` path, file input to replace image, **"Remove current cover image" checkbox** (`name="deleteImage" value="true"`), FileReader live preview for new selection, JS that dims thumbnail on checkbox check and auto-unchecks on new file selection, pre-populated inputs, category pre-selection |
| `Views/Books/Details.cshtml` | Read-only view, `@Html.DisplayFor`, `<dl>` definition list, null-conditional `?.`, two-column Bootstrap layout |
| `Views/Books/Delete.cshtml` | Two-step GET→POST delete, hidden PK field, danger styling conventions, Cancel as a safe link |
| `Views/Categories/*.cshtml` | All of the above patterns applied to a simpler, one-field entity — ideal for seeing the concepts in their most direct form |
| `wwwroot/css/site.css` | CSS custom properties (design tokens), Bootstrap override strategy, transitions, responsive `@media` queries, `object-fit: cover`, accessibility focus rings; new sections 18–20 cover bestseller amber cards, featured indigo cards with `backdrop-filter` glass badge, and the `.home-section-divider` gradient rule |
| `wwwroot/js/site.js` | `wwwroot` as the web root, script load order, `DOMContentLoaded`, separation of concerns, commented auto-dismiss example |

### How to get the most out of the comments

1. **Read the header block first.** Before studying any method or template, read the `KEY CONCEPTS` list at the top of the file. It tells you exactly what to watch for.
2. **Trace a full request.** Pick one action — for example, creating a book — and follow it from `BooksController.Create (GET)` → `Views/Books/Create.cshtml` → `BooksController.Create (POST)`. Read every inline comment along that path.
3. **Compare Create and Edit.** The `Create` and `Edit` views are nearly identical. The inline comments highlight the exact lines that differ and explain *why* (hidden PK field, pre-selected dropdown, image pre-load). This comparison is a fast way to understand model binding.
4. **Read the controller before the view.** The controller comment explains what data it prepares (`ViewBag`, `SelectList`, `.Include()`). Once you understand what arrives at the view, the view's markup makes much more sense.
5. **Don't skip the CSS.** `site.css` comments explain every design decision — from CSS custom properties to the book cover `object-fit` trick. If you plan to customise the UI, read Section 1 (Design Tokens) first.
6. **Ask "why", not just "what".** Most inline comments are written to answer *why* the code is written a particular way — a security reason (`[Bind]`, `[ValidateAntiForgeryToken]`), a convention (`null!`, `string.Empty`), or a fix to a scaffolding gap (`.Include()`, `SelectList`). Look for those explanations.

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
- **Create** a new book with author, title, cover image file upload (PNG/JPEG; live FileReader preview), price, mature content toggle, and category dropdown
- **Edit** an existing book — existing cover thumbnail shown; upload a new file to replace it, or check **"Remove current cover image"** to delete it (leave both empty to keep current); JS dims the thumbnail while the delete checkbox is active and auto-unchecks it if a new file is selected; category dropdown pre-selects the current value
- **View Details** — two-column card layout with full-height cover image on the left and metadata on the right
- **Delete** — danger-themed confirmation card with a cover thumbnail and full book summary; the associated image file is removed from disk on confirmation

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
- **Live cover preview (FileReader API)** — on Create, when a file is selected in the picker the FileReader API reads the bytes locally and shows a preview before the form is submitted; on Edit, the existing cover thumbnail is shown server-side and a new preview appears only when a replacement file is chosen
- **Delete cover image** — a "Remove current cover image" checkbox in the Edit form lets users permanently delete the current cover; the controller deletes the file from disk and sets `Image = null`; JavaScript dims the thumbnail as feedback while the checkbox is active
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
- **Best Sellers section** — three amber/gold-themed cards with circular rank badges (#1, #2, #3) overlaid on the cover area, amber top-border accent, genre pill, title/author, price, and a "View Book" CTA. Mock data (Book IDs 1–3) replaces a real DB query; inline comments explain the production replacement path.
- **Featured Books section** — three indigo-themed cards with frosted-glass curator badges ("Editor's Choice", "Course Pick", "Staff Pick") pinned to the top-right of the cover, indigo left-border accent, and the same metadata layout. Mock data (Book IDs 4–6).
- **Section dividers** — `.home-section-divider` — a decorative 1 px rule with opacity fade at both edges, separating the three content sections.
- **Feature cards** — three equal-height navigation cards (Books, Categories, Account) with lift-on-hover effect, coloured icon badges, and action buttons; heading now includes a compass Bootstrap Icon.
- **Tech strip** — expanded to four items: Identity, EF Core, Bootstrap 5, and **File Upload** (GUID-based cover images), using a 2 × 2 grid on mobile and 4-column row on desktop.

### Books Views

| View | Design |
|---|---|
| `Index` | Responsive card grid; cover art with gradient-placeholder fallback behind image; category + Mature badges; price in green; icon action buttons in card footer |
| `Create` / `Edit` | Centred card form; `input-group` with Bootstrap Icons; small uppercase muted labels; `type="file"` cover image picker with FileReader live preview; Edit shows existing cover thumbnail + **"Remove current cover image" checkbox** (dims thumbnail via JS while active, auto-unchecks on new file selection); Mature Content toggle in a highlighted box; `divider-gradient` separator before action buttons |
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
├── Image (nullable; stores GUID-prefixed filename saved to wwwroot/img/books/)
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
│   ├── img/
│   │   └── books/                  # Uploaded book cover images (GUID-prefixed filenames)
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
| `/` | Home page (hero + best sellers + featured books + feature cards + tech strip) |
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
| **File Upload** | Cover image upload via `multipart/form-data` `<input type="file">` replacing the previous URL text field. Files saved to `wwwroot/img/books/` with GUID-prefixed names to prevent collisions. `IWebHostEnvironment` injected into `BooksController` for `WebRootPath` resolution. `UploadImage()` helper writes the file; `DeleteImage()` helper removes orphaned files on cover replace or book delete. Edit form shows the existing thumbnail and preserves it when no new file is chosen. FileReader API drives the in-browser live preview on Create and Edit without a round-trip. `app.UseStaticFiles()` added to `Program.cs` so uploaded runtime files are served from `wwwroot/`. |
| **Delete Cover Image + Home Page Enhancements** | **Edit form:** "Remove current cover image" checkbox (`name="deleteImage" value="true"`) added to Edit.cshtml. Controller (Step 11 fix) accepts `bool deleteImage = false`; when true and no replacement file is uploaded, the old cover is deleted from disk and `Image` set to `null`. JS feedback: thumbnail dims (`opacity:0.3` + `grayscale(100%)`) when the checkbox is checked; auto-unchecks and restores the thumbnail when the user selects a new file (upload wins). **Home page:** two new sections added — *Best Sellers* (amber/gold theme, rank-badge overlay, `Book IDs 1–3` mock data) and *Featured Books* (indigo theme, glass-morphism curator badge, `Book IDs 4–6` mock data). Inline comments guide students on replacing mock data with a real DB query. `site.css` extended with sections 18–20: `.bestseller-card`, `.bestseller-rank-badge`, `.featured-card`, `.featured-label-badge`, `.home-section-divider`. Tech strip expanded from 3 to 4 icons (added File Upload). |

---

Built with Georgian College · COMP2084 Summer 2026
