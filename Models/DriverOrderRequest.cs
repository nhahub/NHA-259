using HeavyGo_Project_Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HeavyGo_Project_Identity.Models
{
    public class DriverOrderRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public string DriverId { get; set; }

        [ForeignKey("DriverId")]
        public ApplicationUser Driver { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime RequestedAt { get; set; } = DateTime.Now;
        public DateTime? RespondedAt { get; set; }
        public double? EstimatedArrivalMinutes { get; set; }
        public double? ProposedPrice { get; set; }
    }
}