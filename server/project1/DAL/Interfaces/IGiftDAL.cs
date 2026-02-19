using project1.Models;

namespace project1.DAL.Interfaces
{
    public interface IGiftDAL
    {
        Task<List<Gift>> GetAllAsync();

        //Task<List<Gift>> GetAllWithCartsAsync();
        Task<Gift?> GetByIdAsync(int id);

        Task<bool> ExistsByNameAsync(string name, int? currentId = null);

        Task AddAsync(Gift gift);
        Task UpdateAsync(Gift gift);
        Task DeleteAsync(int id);

        IQueryable<Gift> GetSearchQuery();
    }
}
