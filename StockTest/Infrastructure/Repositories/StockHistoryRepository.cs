
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StockData.Objects;
using StockTestAPI.Domain;
using StockTestAPI.Infrastructure.Repositories.Interfaces;

namespace StockTestAPI.Infrastructure.Repositories
{
    public class StockHistoryRepository : IStockHistoryRepository
    {
        private readonly StockDbContext _stockDbContext;
        public StockHistoryRepository(StockDbContext contextFactory)
        {
            _stockDbContext = contextFactory;
        }

        public async Task<int> AddStockHistory(List<StockHistory> stocksHistory)
        {
            var dbRows = new List<StockData.Objects.Entities.StockHistory>();
            var date = DateTime.UtcNow;
            foreach (var srcHistory in stocksHistory)
            {
                var dst = new StockData.Objects.Entities.StockHistory
                {
                    StockId = srcHistory.StockId,
                    DateTime = srcHistory.DateTime,
                    OpenPrice = srcHistory.OpenPrice,
                    ClosePrice = srcHistory.ClosePrice,
                    DateCreated = date
                };
                dbRows.Add(dst);
            }

            await _stockDbContext.StockHistory.AddRangeAsync(dbRows);
            await _stockDbContext.SaveChangesAsync();

            return dbRows.Count;
        }

        public async Task<List<DateTime>> GetExistingHistoryData(string stockId, List<DateTime> dates)
        {
            var existingsDates = await _stockDbContext.StockHistory
                     .Where(x => x.StockId == stockId && dates.Contains(x.DateTime))
                     .Select(x => x.DateTime).ToListAsync();
            return existingsDates;
        }
    }
}
