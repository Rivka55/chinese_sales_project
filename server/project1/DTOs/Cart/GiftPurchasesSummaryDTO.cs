using System.Collections.Generic;

namespace project1.DTOs.Cart
{
    public class GiftPurchasesSummaryDTO
    {
        public int GiftId { get; set; }
        public string GiftName { get; set; } = string.Empty;
        // list of purchasers with their details and quantities
        public List<GiftPurchaseDTO> Purchasers { get; set; } = new List<GiftPurchaseDTO>();
        // total number of tickets purchased for this gift
        public int TotalTicketsPurchased { get; set; }
        // total money earned for this gift (sum of quantity * per-card price recorded at purchase time)
        public decimal TotalEarned { get; set; }
    }
}