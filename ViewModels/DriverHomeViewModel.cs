using HeavyGo_Project_Identity.Models;

namespace HeavyGo_Project_Identity.ViewModels
{
    public class DriverHomeViewModel
    {
        public List<DriverOrderNearbyViewModel> AcceptedOrders { get; set; } = new();
        public List<DriverOrderNearbyViewModel> NearbyOrders { get; set; } = new();
    }
}
