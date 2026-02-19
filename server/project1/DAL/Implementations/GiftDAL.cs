using Microsoft.EntityFrameworkCore;
using project1.Models;
using project1.DAL.Interfaces;

namespace project1.DAL.Implementations
{
    public class GiftDAL : IGiftDAL
    {
        private readonly ProjectContext _context;

        public GiftDAL(ProjectContext context)
        {
            _context = context;
        }

        public async Task<List<Gift>> GetAllAsync()
            => await _context.Gifts
                    .Include(g => g.Donor)
                    .Include(g => g.Category)
                    .Include(g => g.Winner)
                    .ToListAsync();

        public async Task<Gift?> GetByIdAsync(int id)
            => await _context.Gifts
                    .Include(g => g.Donor)
                    .Include(g => g.Category)
                    .Include(g => g.Winner)
                    .FirstOrDefaultAsync(g => g.Id == id);

        public async Task<bool> ExistsByNameAsync(string name, int? currentId = null)
            => await _context.Gifts.AnyAsync(g => g.Name == name && (!currentId.HasValue || g.Id != currentId));

        public async Task AddAsync(Gift gift)
        {
            await _context.Gifts.AddAsync(gift);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Gift gift)
        {
            _context.Gifts.Update(gift);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var gift = await _context.Gifts
                .Include(g => g.Carts)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gift == null) throw new KeyNotFoundException("מתנה לא נמצאה");

            if (gift.Carts.Any(c => c.IsPurchased))
                throw new InvalidOperationException("לא ניתן למחוק מתנה שכבר רכשו עבורה כרטיסים");

            if (gift.WinnerId != null)
                throw new InvalidOperationException("לא ניתן למחוק מתנה שכבר הוגרלה");

            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Gift> GetSearchQuery()
            => _context.Gifts.AsQueryable();
    }
}