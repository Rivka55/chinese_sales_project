using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project1.BLL.Interfaces;
using project1.DTOs.Category;
using Microsoft.Extensions.Logging;

namespace project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService service, ILogger<CategoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Received request to fetch all categories.");
            try
            {
                var categories = await _service.GetAllAsync();
                _logger.LogInformation("Successfully returned {Count} categories.", categories.Count);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בשליפת הקטגוריות", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Add(CategoryCreateDTO dto)
        {
            _logger.LogInformation("Received request to add a new category: {CategoryName}", dto.Name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddAsync(dto);
                _logger.LogInformation("Category '{CategoryName}' added successfully via API.", dto.Name);
                return Ok(new { message = "הקטגוריה נוספה בהצלחה" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Conflict while adding category: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding category '{CategoryName}'.", dto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בתהליך ההוספה", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Update(int id, CategoryCreateDTO dto)
        {
            _logger.LogInformation("Received request to update category ID: {Id} to Name: {NewName}", id, dto.Name);
            try
            {
                await _service.UpdateAsync(id, dto);
                _logger.LogInformation("Category ID {Id} updated successfully.", id);
                return Ok(new { message = "הקטגוריה עודכנה בהצלחה" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Update failed: Category ID {Id} not found.", id);
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Update failed due to name conflict: {NewName}", dto.Name);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during update of category ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בתהליך העדכון", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Received request to delete category ID: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Category ID {Id} deleted successfully.", id);
                return Ok(new { message = "הקטגוריה נמחקה בהצלחה" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Delete failed: Category ID {Id} not found.", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during deletion of category ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בתהליך המחיקה", error = ex.Message });
            }
        }
    }
}















//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using project1.BLL.Interfaces;
//using project1.DTOs.Category;

//namespace project1.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CategoryController : ControllerBase
//    {
//        private readonly ICategoryService _service;

//        public CategoryController(ICategoryService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var categories = await _service.GetAllAsync();
//                return Ok(categories);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בשליפת הקטגוריות", error = ex.Message });
//            }
//        }

//        [HttpPost]
//        [Authorize(Roles = "manager")]
//        public async Task<IActionResult> Add(CategoryCreateDTO dto)
//        {
//            try
//            {
//                await _service.AddAsync(dto);
//                return Ok(new { message = "הקטגוריה נוספה בהצלחה" });
//            }
//            catch (InvalidOperationException ex)
//            {
//                return Conflict(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בתהליך ההוספה", error = ex.Message });
//            }
//        }

//        [HttpPut("{id}")]
//        [Authorize(Roles = "manager")]
//        public async Task<IActionResult> Update(int id, CategoryCreateDTO dto)
//        {
//            try
//            {
//                await _service.UpdateAsync(id, dto);
//                return Ok(new { message = "הקטגוריה עודכנה בהצלחה" });
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (InvalidOperationException ex)
//            {
//                return Conflict(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בתהליך העדכון", error = ex.Message });
//            }
//        }

//        [HttpDelete("{id}")]
//        [Authorize(Roles = "manager")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                await _service.DeleteAsync(id);
//                return Ok(new { message = "הקטגוריה נמחקה בהצלחה" });
//            }
//            catch (KeyNotFoundException ex)
//            {
//                return NotFound(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בתהליך המחיקה", error = ex.Message });
//            }
//        }
//    }
//}