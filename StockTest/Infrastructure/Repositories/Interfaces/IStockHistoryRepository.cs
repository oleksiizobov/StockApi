
using StockTestAPI.Domain;
using StockTestAPI.DTO;

namespace StockTestAPI.Infrastructure.Repositories.Interfaces
{
    public interface IStockHistoryRepository
    {
        Task<int> AddStockHistory(List<StockHistory> stocksHistory);
        Task<List<DateTime>> GetExistingHistoryData(string stockId, List<DateTime> date);

        Task<List<StockParams>> GetStockByDates(string stockId, List<DateTime> dates);
    }
}
