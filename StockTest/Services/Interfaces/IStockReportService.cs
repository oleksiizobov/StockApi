using StockTestAPI.DTO;

namespace StockTestAPI.Services.Interfaces
{
    public interface IStockReportService
    {
      
        Dictionary<int, decimal> GetCalculatedStockPerfomance(List<StockParams> stockHistoryData);
    }
}
