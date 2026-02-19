using Microsoft.EntityFrameworkCore;
using project1.DAL.Interfaces;
using project1.DTOs.Donor;
using project1.Models;
using AutoMapper;
using System.Text.RegularExpressions;

namespace project1.DAL.Implementations
{
    public class DonorDAL : IDonorDAL
    {
        private readonly ProjectContext _context;

        public DonorDAL(ProjectContext context)
        {
            _context = context;
        }

        public async Task<List<Donor>> GetAllAsync()
            => await _context.Donors
                .Include(d => d.Gifts)
                    .ThenInclude(g => g.Category)
                .ToListAsync();

        public async Task<Donor?> GetByIdAsync(int id)
            => await _context.Donors
                .Include(d => d.Gifts)
                    .ThenInclude(g => g.Category)
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<bool> ExistsByIdentityNumberAsync(string identityNumber, int? currentId = null)
            => await _context.Donors.AnyAsync(d => d.IdentityNumber == identityNumber && (!currentId.HasValue || d.Id != currentId));

        public async Task<bool> ExistsByNameAsync(string name, int? currentId = null)
            => await _context.Donors.AnyAsync(d => d.Name == name && (!currentId.HasValue || d.Id != currentId));

        public async Task<bool> ExistsByEmailAsync(string email, int? currentId = null)
            => await _context.Donors.AnyAsync(d => d.Email == email && (!currentId.HasValue || d.Id != currentId));

        public async Task AddAsync(Donor donor)
        {
            await _context.Donors.AddAsync(donor);
            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateAsync(Donor donor)
        {
            _context.Donors.Update(donor);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var donor = await _context.Donors
                .Include(d => d.Gifts)
                .FirstOrDefaultAsync(d => d.Id == id);
            
            if (donor == null) 
                throw new KeyNotFoundException("Donor not found");

            if (donor.Gifts.Any())
                throw new InvalidOperationException("לא ניתן למחוק תורם שיש לו מתנות רשומות");

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Donor> GetSearchQuery()
            => _context.Donors.AsQueryable();
    }
}