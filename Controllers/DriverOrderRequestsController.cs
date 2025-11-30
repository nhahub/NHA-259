using HeavyGo_Project_Identity.Data;
using HeavyGo_Project_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DriverOrderRequestsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DriverOrderRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: DriverOrderRequests
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var requests = await _context.DriverOrderRequests
                            .Include(r => r.Order)
                            .Where(r => r.DriverId == user.Id && r.Status == "Pending")
                            .ToListAsync();

        return View(requests);
    }

    // Accept request
    public async Task<IActionResult> Accept(int id)
    {
        var request = await _context.DriverOrderRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = "Accepted";
            request.RespondedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Reject request
    public async Task<IActionResult> Reject(int id)
    {
        var request = await _context.DriverOrderRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = "Rejected";
            request.RespondedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
