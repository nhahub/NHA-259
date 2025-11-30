using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HeavyGo_Project_Identity.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required, StringLength(50)]
        public string PaymentMethod { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.Now;
    }
}