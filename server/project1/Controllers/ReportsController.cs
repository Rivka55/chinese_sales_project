using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project1.BLL.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "manager")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    [HttpGet("winners")]
    public async Task<IActionResult> GetWinnersReport()
    {
        _logger.LogInformation("Request received to generate winners report.");
        try
        {
            var report = await _reportService.GetWinnersReportAsync();
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating winners report.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בהפקת דוח זוכים" });
        }
    }

    [HttpGet("revenue-summary")]
    public async Task<IActionResult> GetRevenueSummary()
    {
        _logger.LogInformation("Request received to fetch revenue summary.");
        try
        {
            var summary = await _reportService.GetRevenueSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to calculate revenue summary.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "שגיאה בחישוב נתוני הכנסות" });
        }
    }
}