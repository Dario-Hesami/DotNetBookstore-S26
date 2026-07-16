# DotNet Bookstore

A full-featured ASP.NET Core web application for managing an online bookstore catalogue. Built for the COMP2084 course at Georgian College (Summer 2026).

Badges:
- .NET 10.0
- Bootstrap 5
- EF Core 10.0
- SQL Server / Azure SQL

---

## Live repository

https://github.com/Dario-Hesami/DotNetBookstore-S26

---

## Quick overview

DotNet Bookstore is an ASP.NET Core application (MVC with Razor views) that demonstrates a complete CRUD workflow for Books and Categories, user authentication using ASP.NET Core Identity, and file uploads for book cover images. The solution targets .NET 10.

Key features:
- Book and Category management (Create / Read / Update / Delete)
- ASP.NET Core Identity for user registration and login
- EF Core data access with migrations and eager-loading (`.Include()`)
- Cover image upload (saved to `wwwroot/img/books/` with GUID-prefixed filenames)
- Bootstrap 5 based responsive UI and custom design tokens
- Educational inline comments in source files to help learners

---

## Technology stack

- .NET 10
- ASP.NET Core MVC (Razor views)
- Entity Framework Core 10 (SQL Server / Azure SQL)
- ASP.NET Core Identity
- Bootstrap 5, Bootstrap Icons

---

## Getting started (local development)

Prerequisites:
- .NET 10 SDK (https://dotnet.microsoft.com/)
- SQL Server / LocalDB or Azure SQL
- (Optional) Visual Studio 2022/2026 or VS Code

1. Clone the repository:

   git clone https://github.com/Dario-Hesami/DotNetBookstore-S26.git
   cd DotNetBookstore

2. Update configuration:

   - The default connection string is configured in `appsettings.json`. Change `DefaultConnection` to point to your SQL Server / Azure SQL instance.
   - For production deployments, store secrets (connection strings, keys) in user secrets, environment variables, or Azure Key Vault.

3. Apply EF Core migrations and seed the database:

   dotnet tool restore
   dotnet ef database update

4. Run the application:

   dotnet run --project DotNetBookstore

Open https://localhost:5001 (or the URL output by dotnet run).

---

## Database and migrations

- Project uses EF Core migrations. To add a migration:

  dotnet ef migrations add DescribeChange -s DotNetBookstore -p DotNetBookstore

- To apply migrations locally:

  dotnet ef database update -s DotNetBookstore -p DotNetBookstore

---

## File uploads (cover images)

- Cover images are uploaded from the Books UI and saved to `wwwroot/img/books/`.
- Filenames are prefixed with a GUID to avoid collisions. Old files are deleted when a book's image is replaced or the book is removed.
- If you need to change the storage location, update `BooksController` helper methods that reference `IWebHostEnvironment.WebRootPath` and the `img/books` folder.

---

## Routes (high level)

- Home: `/` (Home/Index)
- Books: `/Books` (Index, Create, Edit, Details, Delete)
- Categories: `/Categories` (Index, Create, Edit, Delete)
- Account / Identity pages (register, login, logout) use the Identity UI and are available under `/Identity/Account` when enabled.

---

## Testing and validation

- The project includes server-side validation attributes. Client-side validation uses jQuery Validate + unobtrusive adapters included in the layout.
- Manual checks:
  - Create a Category then create a Book assigned to that Category.
  - Upload a cover image and confirm it appears in `wwwroot/img/books/`.
  - Edit a book and replace its image; confirm the old file is removed.

---

## Deploying to Azure Web App (high level)

- Publish the project using Visual Studio Publish or `dotnet publish` and deploy the artifact to an Azure Web App.
- Use Azure SQL for production and update connection strings accordingly.
- Enable Application Insights for telemetry (optional).

---

## Educational guidance

This repository is annotated heavily with comments in source files. Open `Program.cs`, controllers, models and views to read teaching notes explaining why code was written that way.

---

## Contributing / Notes

- Keep changes scoped and run the application locally before opening a PR.
- When changing DB models, add and apply EF migrations.

---

## License

This project is for educational use. Check the repository for any license file if needed.

---

## Contact

Repository: https://github.com/Dario-Hesami/DotNetBookstore-S26

