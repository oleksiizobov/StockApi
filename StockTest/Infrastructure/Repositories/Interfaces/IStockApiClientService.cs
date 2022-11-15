using StockTestAPI.DTO;
using StockTestAPI.Domain;

namespace StockTestAPI.Infrastructure.Repositories.Interfaces
{
    public interface IStockApiClientService
    {
        ValueTask<List<StockParams>> GetStockPriceByDate(string stockId, int lastDaysCount);
        ValueTask<List<StockParams>> GetStockPriceByHour(string stockId, int lastDaysCount);
    }
}
