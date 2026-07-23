using DotNetBookstore.Data;
using DotNetBookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotNetBookstore.Controllers
{
    /// <summary>
    /// Public shop surface where users can browse categories and begin shopping.
    /// </summary>
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Shop or /Shop/Index
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        // GET: /Shop/ShopByCategory/5
        public async Task<IActionResult> ShopByCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return BadRequest();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Where(b => b.CategoryId == categoryId)
                .Include(b => b.Category)
                .OrderBy(b => b.Author)
                .ThenBy(b => b.Title)
                .ToListAsync();

            ViewData["Title"] = $"Shop - {category.Name}";
            ViewData["CategoryName"] = category.Name;

            return View("ShopByCategory", books);
        }

        // POST: /Shop/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int bookId, int quantity = 1)
        {
            // Keep logic simple and clear for demonstration purposes
            if (quantity < 1) quantity = 1;
            if (quantity > 1000) quantity = 1000;

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            var customerId = GetCustomerId();

            // Check for existing cart item for this customer
            var cartItem = await _context.CartItems
                .SingleOrDefaultAsync(c => c.BookId == bookId && c.CustomerId == customerId);

            if (cartItem == null)
            {
                // Create new cart item
                cartItem = new CartItem
                {
                    BookId = bookId,
                    Quantity = quantity,
                    Price = book.Price,
                    CustomerId = customerId
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                // Update existing quantity (simple additive merge)
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();

            // TempData notification for UI toast
            try
            {
                TempData["CartMessage"] = $"Added {quantity} × \"{(book?.Title ?? "item")}\" to your cart.";
                TempData["CartMessageType"] = "success";
            }
            catch { }

            // Update session ItemCount so navbar or layout can show updated count
            var items = await _context.CartItems.Where(c => c.CustomerId == customerId).ToListAsync();
            var itemCount = items.Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("ItemCount", itemCount);

            // Redirect user to Cart page to view their cart
            return RedirectToAction("Cart");
        }

        // identify customer for each shopping cart using session
        private string GetCustomerId()
        {
            var sid = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(sid))
            {
                if (User?.Identity?.IsAuthenticated == true)
                {
                    // use authenticated user's name/email as id
                    sid = User.Identity.Name ?? Guid.NewGuid().ToString();
                }
                else
                {
                    // generate simple GUID for anonymous session
                    sid = Guid.NewGuid().ToString();
                }

                HttpContext.Session.SetString("CustomerId", sid);
            }

            return sid;
        }

        // Small helper to produce a safe, user-friendly title for UI messages
        private static string DisplayTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "item";
            // Remove common placeholder prefixes used in sample data (e.g., "X-") and trim
            var t = title.Trim();
            if (t.StartsWith("X-", StringComparison.OrdinalIgnoreCase)) t = t.Substring(2).Trim();
            return t;
        }

        // GET: /Shop/Cart
        public async Task<IActionResult> Cart()
        {
            var customerId = GetCustomerId();

            var cartItems = await _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.CustomerId == customerId)
                .OrderBy(c => c.Book.Title)
                .ToListAsync();

            var itemCount = cartItems.Sum(c => c.Quantity);
            HttpContext.Session.SetInt32("ItemCount", itemCount);

            return View(cartItems);
        }

        // POST: /Shop/RemoveFromCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            // Load cart item with Book navigation so we have the title for notifications
            var cartItem = await _context.CartItems
                .Include(c => c.Book)
                .FirstOrDefaultAsync(c => c.CartItemId == id);

            if (cartItem != null)
            {
                var title = cartItem.Book?.Title ?? "item";
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                // update session count
                var customerId = GetCustomerId();
                var items = await _context.CartItems.Where(c => c.CustomerId == customerId).ToListAsync();
                HttpContext.Session.SetInt32("ItemCount", items.Sum(c => c.Quantity));
                try
                {
                    TempData["CartMessage"] = $"Removed \"{title}\" from your cart.";
                    TempData["CartMessageType"] = "warning";
                }
                catch { }
            }

            return RedirectToAction("Cart");
        }

        // POST: /Shop/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 1000) quantity = 1000;

            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            // update session count
            var customerId = GetCustomerId();
            var items = await _context.CartItems.Where(c => c.CustomerId == customerId).ToListAsync();
            HttpContext.Session.SetInt32("ItemCount", items.Sum(c => c.Quantity));

            // load book title for notification
            try
            {
                var book = await _context.Books.FindAsync(cartItem.BookId);
                // Polished, user-friendly toast message
                TempData["CartMessage"] = $"Updated quantity for \"{DisplayTitle(book?.Title)}\" — now {quantity} in your cart.";
                TempData["CartMessageType"] = "info";
            }
            catch { }

            return RedirectToAction("Cart");
        }
    }
}