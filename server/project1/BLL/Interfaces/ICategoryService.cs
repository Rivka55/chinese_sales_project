using project1.DTOs.Category;

namespace project1.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync();
        Task AddAsync(CategoryCreateDTO dto);
        Task UpdateAsync(int id, CategoryCreateDTO dto);
        Task DeleteAsync(int id);
    }
}