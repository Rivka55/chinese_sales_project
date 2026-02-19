using project1.DTOs.Cart;
using project1.Models;
using System.ComponentModel.DataAnnotations;

namespace project1.DTOs.Gift
{
    public class GiftDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public int Price { get; set; }

        public string DonorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;  
        public string? WinnerName { get; set; }
        public string? WinnerEmail { get; set; }
    }
}