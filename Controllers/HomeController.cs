using HeavyGo_Project_Identity.Data;
using HeavyGo_Project_Identity.Models;
using HeavyGo_Project_Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HeavyGo_Project_Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // ---------------------------------------------------------------
        // CLIENT HOME PAGE
        // ---------------------------------------------------------------
        [Authorize(Roles = "Client")]
        public IActionResult ClientHome()
        {
            return View();
        }

        // ---------------------------------------------------------------
        // DRIVER HOME PAGE — SHOW NEARBY ORDERS
        // ---------------------------------------------------------------
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> DriverHome()
        {
            var driver = await _userManager.GetUserAsync(User);

            if (driver == null)
            {
                ViewBag.Error = "Please update your location first.";
                return View(new DriverHomeViewModel());
            }

            // Load all orders
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.DriverRequests)
                .ToListAsync();

            // Accepted Orders (by this driver)
            var acceptedOrders = orders
                .Where(o => o.DriverRequests.Any(r =>
                    r.DriverId == driver.Id &&
                    r.Status == "Accepted"
                ))
                .Select(o => new DriverOrderNearbyViewModel
                {
                    OrderId = o.OrderId,
                    ClientName = o.User.UserName,
                    From = o.PickupLocation,
                    To = o.DropoffLocation,
                    TotalPrice = o.TotalPrice,
                    Status = "Accepted"
                })
                .ToList();

            // All orders that are NOT accepted by this driver
            var nearbyOrders = orders
                .Where(o => !o.DriverRequests.Any(r => r.DriverId == driver.Id && r.Status == "Accepted"))
                .Select(o => new DriverOrderNearbyViewModel
                {
                    OrderId = o.OrderId,
                    ClientName = o.User.UserName,
                    From = o.PickupLocation,
                    To = o.DropoffLocation,
                    TotalPrice=o.TotalPrice,

                    Status = "Available"
                })
                .ToList();

            return View(new DriverHomeViewModel
            {
                AcceptedOrders = acceptedOrders,
                NearbyOrders = nearbyOrders
            });
        }

        // Driver accepts an order
        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId, double? proposedPrice = null)
        {
            var driver = await _userManager.GetUserAsync(User);
            if (driver == null) return Unauthorized();

            var order = await _context.Orders
                .Include(o => o.DriverRequests)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return NotFound();

            // Prevent multiple accepted requests
            if (order.DriverRequests.Any(r => r.Status == "Accepted"))
                return BadRequest("This order is already accepted by another driver.");

            var request = new DriverOrderRequest
            {
                DriverId = driver.Id,
                OrderId = orderId,
                Status = "Accepted",
                ProposedPrice = proposedPrice
            };

            _context.DriverOrderRequests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("DriverHome");
        }

        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // km
            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        // ---------------------------------------------------------------
        // HAVERSINE DISTANCE
        // ---------------------------------------------------------------
        

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
