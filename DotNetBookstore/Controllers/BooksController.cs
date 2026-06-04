
// FIX AFTER SCAFFOLDING (Step 1): Add 'using Microsoft.AspNetCore.Mvc.Rendering' so that SelectList
// (used to build the Category dropdown for Create/Edit forms) compiles correctly.
// The scaffolder does not add this automatically when it generates the raw CategoryId textbox.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetBookstore.Models;
using DotNetBookstore.Data;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: BOOKS
    // FIX AFTER SCAFFOLDING (Step 2): Add .Include(b => b.Category) so that EF Core eagerly loads
    // the related Category object for each Book in a single SQL JOIN query.
    // Without this, book.Category is null in the view and the Category Name column shows nothing.
    public async Task<IActionResult> Index()
    {
        return View(await _context.Books.Include(b => b.Category).OrderBy(b => b.Author).ThenBy(b => b.Title).ToListAsync());
    }

    // GET: BOOKS/Details/5
    // FIX AFTER SCAFFOLDING (Step 2): Add .Include(b => b.Category) so EF Core loads the related
    // Category for this Book. Without it, book.Category is null and the Category Name row in the
    // Details view is blank.
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(m => m.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: BOOKS/Create
    // FIX AFTER SCAFFOLDING (Step 3): Pass a SelectList of all Categories to the view via ViewBag.
    // The scaffolder only generates a plain textbox for CategoryId (a raw integer FK field).
    // We replace that textbox with a <select> dropdown in the view, and this SelectList feeds it.
    // Arguments: source table, value field (stored as FK), display field (shown to user).
    public IActionResult Create()
    {
        ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Name), "CategoryId", "Name");
        return View();
    }

    // POST: BOOKS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // FIX AFTER SCAFFOLDING (Step 4): Removed navigation properties (Category, CartItems, OrderDetails)
    // from the [Bind] list. The scaffolder added them automatically, but they must never be bound
    // from raw POST data — EF Core manages those relationships through the FK (CategoryId) alone.
    // Binding navigation objects from user input opens an overposting/mass-assignment security hole.
    // FIX AFTER SCAFFOLDING (Step 5): Repopulate ViewBag.CategoryId if validation fails so the
    // Category dropdown re-renders correctly when the form is returned to the user with errors.
    // The 4th argument (book.CategoryId) pre-selects the value the user had already chosen.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BookId,Author,Title,Image,Price,MatureContent,CategoryId")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", book.CategoryId);
        return View(book);
    }

    // GET: BOOKS/Edit/5
    // FIX AFTER SCAFFOLDING (Step 3): Pass a SelectList to ViewBag.CategoryId for the Category dropdown.
    // The 4th argument (book.CategoryId) tells the SelectList which option to pre-select,
    // so the Edit form opens with the book's current category already chosen in the dropdown.
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Name), "CategoryId", "Name", book.CategoryId);
        return View(book);
    }

    // POST: BOOKS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // FIX AFTER SCAFFOLDING (Step 4): Same [Bind] fix as Create POST — removed Category, CartItems,
    // and OrderDetails. Only scalar, user-editable fields are allowed through model binding.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("BookId,Author,Title,Image,Price,MatureContent,CategoryId")] Book book)
    {
        if (id != book.BookId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.BookId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // FIX AFTER SCAFFOLDING (Step 5): Repopulate ViewBag.CategoryId on validation failure
        // so the Category dropdown re-renders with the user's previously selected value intact.
        ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", book.CategoryId);
        return View(book);
    }

    // GET: BOOKS/Delete/5
    // FIX AFTER SCAFFOLDING (Step 2): Add .Include(b => b.Category) so EF Core loads the related
    // Category for this Book. Without it, book.Category is null and the Category Name row on the
    // Delete confirmation view is blank.
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(m => m.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: BOOKS/Delete/5
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

    private bool BookExists(int? id)
    {
        return _context.Books.Any(e => e.BookId == id);
    }
}