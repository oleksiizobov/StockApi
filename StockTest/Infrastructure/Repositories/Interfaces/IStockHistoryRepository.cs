
using StockTestAPI.Domain;

namespace StockTestAPI.Infrastructure.Repositories.Interfaces
{
    public interface IStockHistoryRepository
    {
        Task<int> AddStockHistory(List<StockHistory> stocksHistory);
        Task<List<DateTime>> GetExistingHistoryData(string stockId, List<DateTime> date);
    }
}
