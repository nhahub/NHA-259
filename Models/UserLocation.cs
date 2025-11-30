using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeavyGo_Project_Identity.Models
{
    public class UserLocation
    {
        [Key]
        public int UserLocationId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // ---------- FOREIGN KEY TO USER ----------
        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; }
    }
}
