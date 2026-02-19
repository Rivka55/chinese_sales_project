using Microsoft.EntityFrameworkCore;
using project1.DAL.Interfaces;
using project1.Models;


namespace project1.DAL.Implementations
{
    public class CategoryDAL : ICategoryDAL
    {
        private readonly ProjectContext _context;


        public CategoryDAL(ProjectContext context)
        {
            _context = context;
        }


        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Id)
                .ToListAsync();
        }


        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories.FindAsync(id);


        public async Task<Category?> GetByNameAsync(string name)
         => await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);


        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}