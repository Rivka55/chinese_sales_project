using project1.DTOs.Gift;

namespace project1.DTOs.Donor
{
    public class DonorDTO
    {
        public int Id { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // רשימת מתנות לתורם
        public IEnumerable<GiftDTO> Gifts { get; set; } = Enumerable.Empty<GiftDTO>();
    }
}
