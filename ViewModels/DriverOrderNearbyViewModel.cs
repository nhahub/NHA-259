namespace HeavyGo_Project_Identity.Models
{
    public class DriverOrderNearbyViewModel
    {
        public int OrderId { get; set; }
        public string ClientName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double DistanceKm { get; set; }
        public string Status { get; set; } // "Accepted" or "Available"
        public double TotalPrice { get; set; }
    }
}
