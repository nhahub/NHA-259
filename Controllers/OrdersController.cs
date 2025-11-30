using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HeavyGo_Project_Identity.Data;
using HeavyGo_Project_Identity.Models;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    // GET: Orders
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var orders = await _context.Orders
            .Where(o => o.ApplicationUserId == userId)
            .Include(o => o.DriverRequests)
            .Include(o => o.Reviews) // plural property
            .ToListAsync();


        return View(orders);
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var order = await _context.Orders
            .Include(o => o.DriverRequests)
                .ThenInclude(d => d.Driver)
            .Include(o => o.Payment)
            .Include(o => o.Reviews)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) return NotFound();

        return View(order);
    }

    // GET: Orders/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string PickupLocation,string DropoffLocation,int TotalPrice)
    {
        Order order = new Order
        {
            PickupLocation = PickupLocation,
            DropoffLocation = DropoffLocation,
            TotalPrice = TotalPrice // Example fixed price, replace with actual logic
        };
        if (ModelState.IsValid)
        {
            
            var user = await _userManager.GetUserAsync(User);
            order.ApplicationUserId = user.Id;
            order.CreatedAt = DateTime.Now;

            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(order);
    }
}
