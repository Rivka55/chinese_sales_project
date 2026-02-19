using project1.DTOs.Donor;

namespace project1.BLL.Interfaces
{
    public interface IDonorService
    {
        Task<List<DonorDTO>> GetAllAsync();
        Task<DonorDTO?> GetByIdAsync(int id);
        Task AddAsync(DonorCreateDTO dto);
        Task UpdateAsync(int id, DonorUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<List<DonorDTO>> SearchAsync(string? donorName, string? giftName, string? email);
    }
}