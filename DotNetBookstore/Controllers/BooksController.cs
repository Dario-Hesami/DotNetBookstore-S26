/*
 * ============================================================================
 * DotNet Bookstore — In-Class ASP.NET Core MVC Project
 * Course: COMP2084 | Georgian College | Summer 2026
 * Instructor: Dario Hesami
 * ============================================================================
 *
 * FILE:    Controllers/BooksController.cs
 * PURPOSE: Implements full CRUD (Create, Read, Update, Delete) for the Book
 *          entity. Because a Book belongs to a Category (foreign key), this
 *          controller is more complex than CategoriesController — it must:
 *            • Eagerly load the related Category for display (.Include())
 *            • Populate a dropdown list for the Category selector (SelectList)
 *            • Re-populate the dropdown on validation failure
 *
 * WHAT STUDENTS LEARN HERE (beyond CategoriesController basics):
 *  1. Eager loading (.Include)  — explicitly load a related entity in ONE query
 *                                 using a SQL JOIN, rather than issuing a separate
 *                                 SELECT for each book's category.
 *  2. SelectList + ViewBag      — how to pass a dropdown data source from the
 *                                 controller to the view for a <select> element.
 *  3. [Bind] without navigation — always exclude navigation properties (Category,
 *                                 CartItems, OrderDetails) from [Bind] to prevent
 *                                 over-posting attacks.
 *  4. Repopulate ViewBag        — if form validation fails, we must rebuild the
 *                                 SelectList and put it back in ViewBag, otherwise
 *                                 the dropdown disappears when the form re-renders.
 *  5. OrderBy in queries        — sort the book list and category dropdowns with
 *                                 LINQ OrderBy / ThenBy to improve usability.
 *
 * POST-SCAFFOLDING FIXES APPLIED (documented inline below):
 *  Step 1 → Added "using Microsoft.AspNetCore.Mvc.Rendering" for SelectList
 *  Step 2 → Added .Include(b => b.Category) to Index, Details, and Delete GET
 *  Step 3 → Added ViewBag.CategoryId SelectList to Create GET and Edit GET
 *  Step 4 → Removed navigation properties from [Bind] lists (Create & Edit POST)
 *  Step 5 → Repopulated ViewBag.CategoryId when ModelState is invalid (Create & Edit POST)
 * ============================================================================
 */

// FIX (Step 1): Microsoft.AspNetCore.Mvc.Rendering provides SelectList, which
// is needed to build the Category dropdown for the Create and Edit forms.
// The scaffolder does NOT add this using statement automatically.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetBookstore.Models;
using DotNetBookstore.Data;

public class BooksController : Controller
{
    // Injected via constructor by the DI container (see Program.cs).
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ── GET: /Books ───────────────────────────────────────────────────────────
    // Lists all books, ordered by Author then Title, with their Category loaded.
    //
    // FIX (Step 2): Without .Include(b => b.Category), EF Core returns books
    // where book.Category is null. The view would then show an empty category
    // column or throw a NullReferenceException.
    //
    // .Include(b => b.Category) tells EF Core to perform a JOIN:
    //   SELECT b.*, c.* FROM Books b INNER JOIN Categories c ON b.CategoryId = c.CategoryId
    // OrderBy / ThenBy add ORDER BY Author, Title to the query for alphabetical listing.
    public async Task<IActionResult> Index()
    {
        return View(await _context.Books
            .Include(b => b.Category)         // eagerly load the related Category
            .OrderBy(b => b.Author)           // primary sort: alphabetical by author
            .ThenBy(b => b.Title)             // secondary sort: alphabetical by title
            .ToListAsync());
    }

    // ── GET: /Books/Details/5 ─────────────────────────────────────────────────
    // Shows the full details of a single book, including its category name.
    //
    // FIX (Step 2): Same .Include() fix — the Details view displays
    // book.Category.Name, so we must load the related Category or it will be null.
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Category)    // JOIN with Categories table
            .FirstOrDefaultAsync(m => m.BookId == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // ── GET: /Books/Create ────────────────────────────────────────────────────
    // Returns an empty form for adding a new book.
    //
    // FIX (Step 3): The scaffolded version had no SelectList — it generated a
    // plain text box for CategoryId (an integer FK field), which is poor UX.
    // We replace that with a <select> dropdown in the view, and this SelectList
    // feeds it with all available categories.
    //
    // new SelectList(source, valueField, displayField):
    //   source      → IQueryable from the Categories table
    //   valueField  → "CategoryId" — the integer stored in the Books.CategoryId FK
    //   displayField→ "Name"       — the human-readable string shown in the dropdown
    public IActionResult Create()
    {
        // Pass the SelectList to the view via ViewBag.
        // The key "CategoryId" matches the asp-items="ViewBag.CategoryId" attribute
        // in the Create.cshtml view's <select> element.
        ViewBag.CategoryId = new SelectList(
            _context.Categories.OrderBy(c => c.Name), // source: all categories, A-Z
            "CategoryId",                              // value stored when selected
            "Name");                                   // text shown to the user
        return View();
    }

    // ── POST: /Books/Create ───────────────────────────────────────────────────
    // Receives the form submission, validates it, and inserts the new book.
    //
    // FIX (Step 4): The scaffolded [Bind] list included navigation properties
    // (Category, CartItems, OrderDetails). Navigation properties must NEVER be
    // bound from raw POST data — doing so opens an over-posting vulnerability
    // where a malicious user could submit spoofed related-object data.
    // Solution: only bind the scalar, user-editable fields.
    //
    // FIX (Step 5): When validation fails, we return the form view — but the
    // ViewBag.CategoryId we set in the GET action is gone (ViewBag is per-request).
    // We must rebuild it here and include book.CategoryId as the 4th argument
    // so the dropdown pre-selects the category the user had already chosen.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("BookId,Author,Title,Image,Price,MatureContent,CategoryId")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // FIX (Step 5): Repopulate the category dropdown with the previously
        // selected value pre-selected, so the user doesn't lose their choice.
        ViewBag.CategoryId = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name",
            book.CategoryId);  // 4th arg = currently selected value
        return View(book);
    }

    // ── GET: /Books/Edit/5 ────────────────────────────────────────────────────
    // Fetches the book and returns a pre-filled edit form.
    //
    // FIX (Step 3): Same SelectList setup as Create GET, with the addition of
    // book.CategoryId as the 4th argument — this pre-selects the book's current
    // category in the dropdown when the Edit form opens.
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // FindAsync is efficient for PK lookups — checks the EF identity cache first.
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        // Build the SelectList with the book's current CategoryId pre-selected
        // so the dropdown shows the correct category when the Edit form opens.
        ViewBag.CategoryId = new SelectList(
            _context.Categories.OrderBy(c => c.Name),
            "CategoryId",
            "Name",
            book.CategoryId);  // pre-select the book's existing category
        return View(book);
    }

    // ── POST: /Books/Edit/5 ───────────────────────────────────────────────────
    // Saves the updated book fields to the database.
    //
    // FIX (Step 4): Same [Bind] fix as Create POST.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id,
        [Bind("BookId,Author,Title,Image,Price,MatureContent,CategoryId")] Book book)
    {
        // Route ID must match the hidden BookId field in the form.
        if (id != book.BookId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Update() attaches the detached entity and marks all fields as modified.
                // SaveChangesAsync() generates: UPDATE Books SET ... WHERE BookId = @id
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the book still exists; if not, it was deleted by someone else.
                if (!BookExists(book.BookId))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Unknown concurrency problem — propagate to error handler
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // FIX (Step 5): Repopulate the dropdown on validation failure.
        ViewBag.CategoryId = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name",
            book.CategoryId);
        return View(book);
    }

    // ── GET: /Books/Delete/5 ─────────────────────────────────────────────────
    // Shows a confirmation page with the book's details before deletion.
    //
    // FIX (Step 2): .Include(b => b.Category) so the confirmation page can
    // show the category name alongside the book title, author, and price.
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Category)  // load category so the view can display its name
            .FirstOrDefaultAsync(m => m.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // ── POST: /Books/Delete/5 ────────────────────────────────────────────────
    // Permanently removes the book after the user confirms on the delete page.
    //
    // ActionName("Delete") allows both the GET and POST Delete actions to share
    // the same URL (/Books/Delete/5) while having different C# method names.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // ── Private helper ────────────────────────────────────────────────────────
    // Checks if a book with the given ID exists. Used in the Edit POST action
    // to handle DbUpdateConcurrencyException gracefully.
    private bool BookExists(int? id)
    {
        return _context.Books.Any(e => e.BookId == id);
    }
}
