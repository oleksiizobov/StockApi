using StockTestAPI.DTO;

namespace StockTestAPI.Infrastructure.Repositories.Interfaces
{
    public interface IStockHistoryProxyRepository
    {
        ValueTask<int> AddStockHistory(string stockId, List<StockParams> stockHistoryData);

        ValueTask<List<StockParams>> GetStockByDates(string stockId, int lastDaysCount);
    }
}
