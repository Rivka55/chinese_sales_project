using AutoMapper;
using project1.BLL.Interfaces;
using project1.DAL.Interfaces;
using project1.DTOs.Category;
using project1.Models;
using Microsoft.Extensions.Logging;

namespace project1.BLL.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDAL _dal;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryDAL dal, IMapper mapper, ILogger<CategoryService> logger)
        {
            _dal = dal;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            _logger.LogDebug("Fetching categories from database.");
            var categories = await _dal.GetAllAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task AddAsync(CategoryCreateDTO dto)
        {
            _logger.LogInformation("Processing business logic to add category: {CategoryName}", dto.Name);

            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name == "string")
            {
                _logger.LogWarning("Validation failed: Category name is empty or invalid.");
                throw new ArgumentException("שם הקטגוריה הוא שדה חובה");
            }

            var existing = await _dal.GetByNameAsync(dto.Name);
            if (existing != null)
            {
                _logger.LogWarning("Duplicate category name detected: {CategoryName}", dto.Name);
                throw new InvalidOperationException("קטגוריה בשם זה כבר קיימת במערכת");
            }

            var category = _mapper.Map<Category>(dto);
            await _dal.AddAsync(category);
            _logger.LogInformation("New category '{CategoryName}' persisted with ID {Id}.", category.Name, category.Id);
        }

        public async Task UpdateAsync(int id, CategoryCreateDTO dto)
        {
            _logger.LogInformation("Processing update for category ID {Id}.", id);

            var category = await _dal.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogError("Category ID {Id} was not found in database.", id);
                throw new KeyNotFoundException("הקטגוריה לא נמצאה");
            }

            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name == "string")
                throw new ArgumentException("שם הקטגוריה הוא שדה חובה");

            var existing = await _dal.GetByNameAsync(dto.Name);
            if (existing != null && existing.Id != id)
            {
                _logger.LogWarning("Cannot update category ID {Id}: Name '{Name}' is taken by another category.", id, dto.Name);
                throw new InvalidOperationException("קיים כבר שם כזה במערכת");
            }

            category.Name = dto.Name;
            await _dal.UpdateAsync(category);
            _logger.LogInformation("Database update complete for category ID {Id}.", id);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Processing deletion for category ID {Id}.", id);

            var category = await _dal.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogError("Delete failed: Category ID {Id} does not exist.", id);
                throw new KeyNotFoundException("הקטגוריה לא נמצאה");
            }

            await _dal.DeleteAsync(id);
            _logger.LogInformation("Category ID {Id} removed from database.", id);
        }
    }
}
















//using AutoMapper;
//using project1.BLL.Interfaces;
//using project1.DAL.Interfaces;
//using project1.DTOs.Category;
//using project1.Models;

//namespace project1.BLL.Implementations
//{
//    public class CategoryService : ICategoryService
//    {
//        private readonly ICategoryDAL _dal;
//        private readonly IMapper _mapper;

//        public CategoryService(ICategoryDAL dal, IMapper mapper)
//        {
//            _dal = dal;
//            _mapper = mapper;
//        }

//        public async Task<List<CategoryDTO>> GetAllAsync()
//        {
//            var categories = await _dal.GetAllAsync();

//            return _mapper.Map<List<CategoryDTO>>(categories);
//        }

//        public async Task AddAsync(CategoryCreateDTO dto)
//        {
//            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name == "string")
//                throw new ArgumentException("שם הקטגוריה הוא שדה חובה");

//            if (await _dal.GetByNameAsync(dto.Name) != null)
//                throw new InvalidOperationException("קטגוריה בשם זה כבר קיימת במערכת");

//            var category = _mapper.Map<Category>(dto);
//            await _dal.AddAsync(category);
//        }

//        public async Task UpdateAsync(int id, CategoryCreateDTO dto)
//        {
//            var category = await _dal.GetByIdAsync(id)
//                ?? throw new KeyNotFoundException("הקטגוריה לא נמצאה");

//            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name == "string")
//                throw new ArgumentException("שם הקטגוריה הוא שדה חובה");

//            var existing = await _dal.GetByNameAsync(dto.Name);
//            if (existing != null && existing.Id != id)
//                throw new InvalidOperationException("קיים כבר שם כזה במערכת");

//            category.Name = dto.Name;
//            await _dal.UpdateAsync(category);
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var category = await _dal.GetByIdAsync(id)
//                ?? throw new KeyNotFoundException("הקטגוריה לא נמצאה");

//            await _dal.DeleteAsync(id);
//        }
//    }
//}