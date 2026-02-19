using project1.DTOs.Report;

namespace project1.BLL.Interfaces
{
    public interface IReportService
    {
        Task<List<GiftWinnerReportDTO>> GetWinnersReportAsync();
        Task<RevenueSummaryDTO> GetRevenueSummaryAsync();
    }
}
