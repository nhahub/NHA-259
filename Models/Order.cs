using HeavyGo_Project_Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HeavyGo_Project_Identity.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required, Range(1, double.MaxValue)]
        public double TotalPrice { get; set; }

        [Required, StringLength(200)]
        public string PickupLocation { get; set; }

        [Required, StringLength(200)]
        public string DropoffLocation { get; set; }

        // User who made the order
        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; }

        // Navigations
        public ICollection<DriverOrderRequest> DriverRequests { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Payment Payment { get; set; }
    }
}