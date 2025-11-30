using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeavyGo_Project_Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NationalId { get; set; }
        public string LicenseId { get; set; }
        public string VehicleNumberPlate { get; set; }
        public string FullName { get; set; }
        public string profileImageUrl { get; set; }
        [NotMapped]
        public IFormFile ProfileImage { get; set; }

        public string VehicleType { get; set; }
        public bool? IsAvailable { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<DriverOrderRequest> DriverOrderRequests { get; set; } 
            = new List<DriverOrderRequest>();
    }
}
