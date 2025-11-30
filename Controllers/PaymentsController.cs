using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeavyGo_Project_Identity.Data;
using HeavyGo_Project_Identity.Models;
using Microsoft.AspNetCore.Identity;

public class PaymentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // -------------------------------------------------------
    // GET: /Payments/Create?orderId=1
    // -------------------------------------------------------
    public async Task<IActionResult> Create(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        if (order.ApplicationUserId != user.Id)
            return Forbid();

        return View(order);
    }

    // -------------------------------------------------------
    // POST: /Payments/Create
    // -------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int orderId, string paymentMethod)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return NotFound();

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = order.TotalPrice,
            PaidAt = DateTime.Now,
            PaymentMethod = paymentMethod
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = payment.PaymentId });
    }

    // -------------------------------------------------------
    // GET: /Payments/Details/5
    // View ONE payment
    // -------------------------------------------------------
    public async Task<IActionResult> Details(int id)
    {
        var payment = await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.PaymentId == id);

        if (payment == null) return NotFound();
        return View(payment);
    }

    // -------------------------------------------------------
    // GET: /Payments/UserPayments
    // List all payments for logged-in user
    // -------------------------------------------------------
    public async Task<IActionResult> UserPayment()
    {
        var userId = _userManager.GetUserId(User);

        var payments = await _context.Payments
            .Include(p => p.Order)
            .Where(p => p.Order.ApplicationUserId == userId)
            .OrderByDescending(p => p.PaidAt)
            .ToListAsync();

        return View(payments);
    }
}
