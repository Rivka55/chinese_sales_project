using System.ComponentModel.DataAnnotations;

namespace project1.Models
{
    public class Gift
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Picture { get; set; } = null!;

        [Required]
        public int Price { get; set; }

        public int DonorId { get; set; }
        public Donor Donor { get; set; } = null!;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? WinnerId { get; set; }
        public User? Winner { get; set; }

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }}