using project1.DTOs.Cart;

namespace project1.BLL.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItemDTO>> GetMyCartAsync(int userId);
        Task AddAsync(int userId, AddToCartDTO dto);
        Task UpdateQuantityAsync(int cartId, int userId, int newQuantity);
        Task RemoveAsync(int cartId, int userId);
        Task PurchaseAsync(int userId);
        Task ClearCartAsync(int userId);

        Task<List<PurchaserDetailsDTO>> GetAllPurchasersAsync();
        Task<PurchaserDetailsDTO> GetPurchaserDetailsAsync(int userId);
        Task<GiftPurchasesSummaryDTO> GetPurchasesByGiftIdAsync(int giftId);

        Task<TopGiftsDTO?> FindTopGiftAsync(string criteria);
    }
}

