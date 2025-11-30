using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeavyGo_Project_Identity.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required, StringLength(100)]
        public string Model { get; set; }

        [Required, StringLength(20)]
        public string PlateNumber { get; set; }

        [StringLength(50)]
        public string Color { get; set; }

        [StringLength(50)]
        public string VehicleType { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; }
    }
}
