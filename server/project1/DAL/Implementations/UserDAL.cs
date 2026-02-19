using Microsoft.EntityFrameworkCore;
using project1.DAL.Interfaces;
using project1.Models;

namespace project1.DAL.Implementations
{
    public class UserDAL : IUserDAL
    {
        private readonly ProjectContext _context;
        public UserDAL(ProjectContext context) 
        {
            _context = context; 
        }


        public async Task<User?> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task<User?> GetByNameAsync(string name)
                    => await _context.Users.FirstOrDefaultAsync(u => u.Name == name);

        public async Task<User?> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());        

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}