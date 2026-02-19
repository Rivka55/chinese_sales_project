using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project1.BLL.Interfaces;
using project1.DAL.Implementations;
using project1.DAL.Interfaces;
using project1.DTOs.Donor;
using project1.DTOs.Gift;
using project1.Models;

namespace project1.BLL.Implementations
{
    public class DonorService : IDonorService
    {
        private readonly IDonorDAL _dal;
        private readonly IMapper _mapper;
        private readonly ILogger<DonorService> _logger;

        public DonorService(IDonorDAL dal, IMapper mapper, ILogger<DonorService> logger)
        {
            _dal = dal;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<DonorDTO>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all donor records from database.");
            var donors = await _dal.GetAllAsync();
            return _mapper.Map<List<DonorDTO>>(donors);
        }

        public async Task<DonorDTO?> GetByIdAsync(int id)
        {
            var donor = await _dal.GetByIdAsync(id);
            if (donor == null)
            {
                _logger.LogWarning("Donor lookup failed for ID: {DonorId}", id);
                throw new KeyNotFoundException("התורם לא נמצא");
            }

            return _mapper.Map<DonorDTO>(donor);
        }

        public async Task AddAsync(DonorCreateDTO dto)
        {
            _logger.LogInformation("Validating new donor: {Name}, Identity: {Identity}", dto.Name, dto.IdentityNumber);

            if (await _dal.ExistsByIdentityNumberAsync(dto.IdentityNumber))
            {
                _logger.LogWarning("Validation failed: Identity Number {Identity} already exists.", dto.IdentityNumber);
                throw new InvalidOperationException("ת.ז כבר קיימת במערכת");
            }

            if (await _dal.ExistsByNameAsync(dto.Name))
            {
                _logger.LogWarning("Validation failed: Donor name '{Name}' already exists.", dto.Name);
                throw new InvalidOperationException("שם כבר קיים במערכת");
            }

            if (await _dal.ExistsByEmailAsync(dto.Email))
            {
                _logger.LogWarning("Validation failed: Email '{Email}' already exists.", dto.Email);
                throw new InvalidOperationException("אימייל כבר קיים במערכת");
            }

            var donor = _mapper.Map<Donor>(dto);
            await _dal.AddAsync(donor);
            _logger.LogInformation("New donor successfully persisted with ID: {DonorId}", donor.Id);
        }

        public async Task UpdateAsync(int id, DonorUpdateDTO dto)
        {
            _logger.LogInformation("Validating updates for donor ID: {DonorId}", id);

            var existingDonor = await _dal.GetByIdAsync(id);
            if (existingDonor == null)
            {
                _logger.LogWarning("Update failed: Donor ID {DonorId} not found in database.", id);
                throw new KeyNotFoundException("התורם לא נמצא");
            }

            if (!string.IsNullOrWhiteSpace(dto.Name) && await _dal.ExistsByNameAsync(dto.Name, id))
            {
                _logger.LogWarning("Update rejected: Name '{Name}' is taken by another donor.", dto.Name);
                throw new InvalidOperationException("השם החדש כבר קיים במערכת");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && await _dal.ExistsByEmailAsync(dto.Email, id))
            {
                _logger.LogWarning("Update rejected: Email '{Email}' is taken by another donor.", dto.Email);
                throw new InvalidOperationException("האימייל החדש כבר קיים במערכת");
            }

            _logger.LogDebug("Mapping update DTO to existing donor entity for ID: {DonorId}", id);
            _mapper.Map(dto, existingDonor);

            await _dal.UpdateAsync(existingDonor);
            _logger.LogInformation("Donor ID {DonorId} successfully updated.", id);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Attempting to delete donor ID: {DonorId}", id);

            var existingDonor = await _dal.GetByIdAsync(id);
            if (existingDonor == null)
            {
                _logger.LogWarning("Delete failed: Donor ID {DonorId} does not exist.", id);
                throw new KeyNotFoundException("התורם לא נמצא");
            }

            await _dal.DeleteAsync(id);
            _logger.LogInformation("Donor ID {DonorId} has been permanently deleted.", id);
        }

        public async Task<List<DonorDTO>> SearchAsync(string? donorName, string? giftName, string? email)
        {
            _logger.LogDebug("Executing manager search with filters.");
            IQueryable<Donor> query = _dal.GetSearchQuery()
                .AsNoTracking()
                .Include(d => d.Gifts)
                .ThenInclude(g => g.Category);

            if (!string.IsNullOrWhiteSpace(donorName))
                query = query.Where(d => d.Name.Contains(donorName));

            if (!string.IsNullOrWhiteSpace(giftName))
                query = query.Where(d => d.Gifts.Any(g => g.Name.Contains(giftName)));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(d => d.Email.Contains(email));

            var results = await query.ToListAsync();

            return _mapper.Map<List<DonorDTO>>(results);
        }
    }
}