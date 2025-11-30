using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeavyGo_Project_Identity.Data;
using HeavyGo_Project_Identity.Models;
using Microsoft.AspNetCore.Identity;

public class ReviewsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // -------------------------------------------------------
    // GET: /Reviews/Create?orderId=5
    // -------------------------------------------------------
    public IActionResult Create(int orderId)
    {
        var review = new Review
        {
            OrderId = orderId
        };

        return View(review);
    }

    // -------------------------------------------------------
    // POST: /Reviews/Create
    // -------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Review review)
    {
        if (!ModelState.IsValid)
            return View(review);

        review.ApplicationUserId = _userManager.GetUserId(User);
        review.CreatedAt = DateTime.Now;

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("UserReviews");
    }

    // -------------------------------------------------------
    // GET: /Reviews/UserReviews
    // shows all reviews written BY the logged-in user
    // -------------------------------------------------------
    public async Task<IActionResult> UserReviews()
    {
        var userId = _userManager.GetUserId(User);

        var reviews = await _context.Reviews
            .Include(r => r.Order)
            .Where(r => r.ApplicationUserId == userId)
            .ToListAsync();

        return View(reviews);
    }

    // -------------------------------------------------------
    // GET: /Reviews/Details/5
    // -------------------------------------------------------
    public async Task<IActionResult> Details(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.Order)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.ReviewId == id);

        if (review == null) return NotFound();

        return View(review);
    }
}
