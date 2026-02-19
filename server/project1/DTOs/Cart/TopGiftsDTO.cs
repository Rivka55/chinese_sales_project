namespace project1.DTOs.Cart
{
    public class TopGiftsDTO
    {
        public int GiftId { get; set; }
        public string GiftName { get; set; } = string.Empty;
        public int TotalTicketsPurchased { get; set; }
        public decimal TotalEarned { get; set; }
        public int Price { get; set; }
    }
}
