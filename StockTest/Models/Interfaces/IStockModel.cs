
using StockTestAPI.DTO;

namespace StockTestAPI.Services.Interfaces
{
    public interface IStockReportModel
    {
        Task<Dictionary<string, Dictionary<int, decimal>>> GetStockPerfomanceByDay(string stockId, int lastDaysCount);
        Task<List<StockPerfomanceDto>> GetStockPerfomanceByHour(string stockId, int lastDaysCount);
    }
}
