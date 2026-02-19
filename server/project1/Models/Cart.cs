using System.ComponentModel.DataAnnotations;

namespace project1.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public int GiftID { get; set; }
        public Gift Gift { get; set; } = null!;

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;

        public bool IsPurchased { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now; // ??? Migration להוריד את זה ולעשות
    }
}
