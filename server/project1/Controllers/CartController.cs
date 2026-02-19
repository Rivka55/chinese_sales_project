using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project1.BLL.Interfaces;
using project1.DTOs.Cart;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace project1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService service, ILogger<CartController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("admin/top-gift")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetTopGift([FromQuery] string criteria)
        {
            _logger.LogInformation("Request received to fetch top gift by criteria {Criteria}", criteria);
            try
            {
                var result = await _service.FindTopGiftAsync(criteria);

                if (result == null)
                {
                    _logger.LogInformation("No top gift found for criteria {Criteria} (no purchases)", criteria);
                    return Ok(null);
                }

                _logger.LogInformation("Successfully retrieved top gift {GiftId} for criteria {Criteria}", result.GiftId, criteria);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid parameters for top gift query.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top gift.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        private int GetUserId()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("Unauthenticated user tried to access personal data.");
                throw new UnauthorizedAccessException("You are not logged in. Please sign in.");
            }

            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                _logger.LogError("Auth Token is valid but NameIdentifier claim is missing.");
                throw new UnauthorizedAccessException("User identifier was not found in the token.");
            }

            return int.Parse(idClaim.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            try
            {
                int userId = GetUserId();
                _logger.LogInformation("User {UserId} requested their cart.", userId);
                var cart = await _service.GetMyCartAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve cart.");
                return StatusCode(500, new { message = "Error while loading the cart." });
            }
        }

        [HttpGet("purchases/{giftId}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetGiftPurchases(int giftId)
        {
            _logger.LogInformation("Request received to view purchases for Gift ID: {GiftId}", giftId);
            try
            {
                var summary = await _service.GetPurchasesByGiftIdAsync(giftId);
                _logger.LogInformation("Successfully retrieved purchase list for Gift ID {GiftId}. Purchasers: {Count}", giftId, summary.Purchasers.Count);
                return Ok(summary);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Gift ID {GiftId} not found.", giftId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving purchases for Gift ID: {GiftId}", giftId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error while retrieving purchase data." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToCartDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid data sent for adding to cart.");
                return BadRequest(ModelState);
            }

            try
            {
                int userId = GetUserId();
                _logger.LogInformation("User {UserId} is adding Gift {GiftId} to cart.", userId, dto.GiftId);
                await _service.AddAsync(userId, dto);
                return Ok(new { message = "Item added to cart successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to add item to cart.");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] int quantity)
        {
            _logger.LogInformation("Updating quantity for CartItem {Id} to {Qty}", id, quantity);
            try
            {
                int userId = GetUserId();
                await _service.UpdateQuantityAsync(id, userId, quantity);
                return Ok(new { message = "Cart quantity updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantity for CartItem {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                int userId = GetUserId();
                _logger.LogInformation("User {UserId} removing item {Id} from cart.", userId, id);
                await _service.RemoveAsync(id, userId);
                return Ok(new { message = "Item removed from cart successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to remove item {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase()
        {
            int userId = GetUserId();
            _logger.LogInformation("User {UserId} is processing purchase.", userId);
            try
            {
                await _service.PurchaseAsync(userId);
                _logger.LogInformation("User {UserId} successfully completed purchase.", userId);
                return Ok(new { message = "Purchase completed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Purchase failed for user {UserId}", userId);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            try
            {
                int userId = GetUserId();
                _logger.LogInformation("User {UserId} is clearing cart.", userId);
                await _service.ClearCartAsync(userId);
                return Ok(new { message = "Cart cleared successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clear cart.");
                return StatusCode(500, new { message = "Error while clearing the cart." });
            }
        }

        [HttpGet("admin/purchasers")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetAllPurchasers()
        {
            _logger.LogInformation("Request received to fetch all purchasers report.");
            try
            {
                var data = await _service.GetAllPurchasersAsync();
                _logger.LogInformation("Successfully retrieved purchasers report for {Count} users.", data.Count);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching purchasers report");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error while fetching purchasers report." });
            }
        }

        [HttpGet("admin/purchaser/{userId}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetPurchaserDetails(int userId)
        {
            _logger.LogInformation("Request received to view full details for Purchaser ID: {UserId}", userId);
            try
            {
                var details = await _service.GetPurchaserDetailsAsync(userId);
                return Ok(details);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Purchaser lookup failed: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving purchaser details for User ID: {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error while loading purchaser details." });
            }
        }
    }
}