namespace project1.DTOs.Cart
{
    public class PurchaserDetailsDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public int TotalTicketsPurchased { get; set; } // סה"כ כרטיסים שקנה בכללי
        public int GrandTotalSpent { get; set; } // כמה כסף הוא הכניס למערכת

        public List<PurchaserItemDTO> PurchaseHistory { get; set; } = new(); // ???
    }
}