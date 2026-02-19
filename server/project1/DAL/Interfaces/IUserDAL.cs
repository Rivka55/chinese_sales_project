using project1.Models;

namespace project1.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<User?> GetByNameAsync(string name);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
    }
}
