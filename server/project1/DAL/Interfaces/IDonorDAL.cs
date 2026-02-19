using project1.DTOs.Donor;
using project1.Models;

namespace project1.DAL.Interfaces
{
    public interface IDonorDAL
    {
        Task<List<Donor>> GetAllAsync();
        Task<Donor?> GetByIdAsync(int id);

        Task<bool> ExistsByIdentityNumberAsync(string identityNumber, int? id = null);
        Task<bool> ExistsByNameAsync(string name, int? id = null);
        Task<bool> ExistsByEmailAsync(string email, int? id = null);

        Task AddAsync(Donor donor);
        Task UpdateAsync(Donor donor);
        Task DeleteAsync(int id);

        IQueryable<Donor> GetSearchQuery();
    }
}