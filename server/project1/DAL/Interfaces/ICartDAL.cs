using project1.Models;
using project1.DTOs.Cart;

namespace project1.DAL.Interfaces
{
    public interface ICartDAL
    {
        Task<List<Cart>> GetUserCartAsync(int userId);
        Task<Cart?> GetByIdAsync(int id);

        Task<Cart?> GetOpenCartItemAsync(int userId, int giftId);
        Task<List<Cart>> GetPurchasedByGiftAsync(int giftId);
        Task<List<Cart>> GetPurchasesByUserIdAsync(int userId);
        Task<List<Cart>> GetAllPurchasedItemsAsync();

        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Cart cart);

        Task ExecutePurchaseAsync(int userId);
        Task ClearUserCartAsync(int userId);

        IQueryable<Cart> GetSearchQuery();

        Task<TopGiftStatsDTO?> FindTopGiftAsync(string criteria);
    }
}