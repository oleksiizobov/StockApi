using Microsoft.Extensions.Caching.Memory;
using StockTestAPI.DTO;
using StockTestAPI.Infrastructure.Repositories.Interfaces;
using StockTestAPI.Services.Interfaces;
using static StockTestAPI.Enums;

namespace StockTestAPI.Infrastructure.Repositories
{
    public class StockApiClientProxyRepository : IStockApiClientService
    {
        private readonly ICacheHelperService _cacheHelperService;
        private readonly IMemoryCache _memoryCache;
        public IStockApiClientService StockApiClient { get; set; }
        public StockApiClientProxyRepository(ICacheHelperService cacheHelperService, IMemoryCache memoryCache, HttpClient httpClient, IConfiguration configuration)
        {
            _cacheHelperService = cacheHelperService;
            StockApiClient = new StockApiClientRepository(httpClient, configuration);
            _memoryCache = memoryCache;
        }
        //ValueTask<List<StockParams>> GetStockPriceByDate(string stockId, DateTime df, DateTime dt);
        //ValueTask<List<StockParams>> GetStockPriceByHour(string stockId, DateTime df, DateTime dt);


        public async ValueTask<List<StockParams>> GetStockPriceByHour(string stockId, int lastDaysCount)
        {
            var key = _cacheHelperService.GetCacheKey(EnumCacheType.StockByDateRangeByHour,$"{stockId}_{DateTime.UtcNow.ToString("yyyy-MM-dd")}_{lastDaysCount}" );
            if (!_memoryCache.TryGetValue(key, out List<StockParams> stockPriceHistory))
            {
                stockPriceHistory = await StockApiClient.GetStockPriceByHour(stockId, lastDaysCount);

                _memoryCache.Set(key, stockPriceHistory, new TimeSpan(24, 0, 0));
            }
            return stockPriceHistory;
        }

        public async ValueTask<List<StockParams>> GetStockPriceByDate(string stockId, int lastDaysCount)
        {
            var key = _cacheHelperService.GetCacheKey(EnumCacheType.StockByDateRangeByDate, $"{stockId}_{DateTime.UtcNow.ToString("yyyy-MM-dd")}_{lastDaysCount}");
            if (!_memoryCache.TryGetValue(key, out List<StockParams> stockPriceHistory))
            {
                stockPriceHistory = await StockApiClient.GetStockPriceByDate(stockId, lastDaysCount);

                if (stockPriceHistory.Count > 0)
                {
                    _memoryCache.Set(key, stockPriceHistory, new TimeSpan(24, 0, 0));
                }
              
            }
            return stockPriceHistory;
        }
    }
}
