using project1.DTOs.Gift;
using project1.Models;

namespace project1.BLL.Interfaces
{
    public interface IGiftService
    {
        Task<List<GiftDTO>> GetAllAsync();
        Task<GiftDTO?> GetByIdAsync(int id);
        Task AddAsync(CreateGiftDTO dto);
        Task UpdateAsync(int id, GiftUpdateDTO dto);
        Task DeleteAsync(int id);

        Task<List<GiftDTO>> ManagerSearchAsync(string? giftName, string? donorName, int? buyersCount);
        Task<List<GiftDTO>> UserSearchAsync(string? categoryName, int? price);

        Task<(User Winner, bool EmailSent)> DrawWinnerAsync(int giftId);
    }
}