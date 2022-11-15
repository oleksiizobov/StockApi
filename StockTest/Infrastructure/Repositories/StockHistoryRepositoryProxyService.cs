using Microsoft.Extensions.Caching.Memory;
using StockTestAPI.Domain;
using StockTestAPI.DTO;
using StockTestAPI.Infrastructure.Repositories.Interfaces;
using StockTestAPI.Services.Interfaces;
using static StockTestAPI.Enums;

namespace StockTestAPI.Infrastructure.Repositories
{
    public class StockHistoryRepositoryProxyService : IStockHistoryProxyRepository
    {
        private readonly ICacheHelperService _cacheHelperService;
        private readonly IMemoryCache _memoryCache;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        public StockHistoryRepositoryProxyService(ICacheHelperService cacheHelperService, IStockHistoryRepository stockHistoryRepository, IMemoryCache memoryCache)
        {
            _cacheHelperService = cacheHelperService;
            _stockHistoryRepository = stockHistoryRepository;
            _memoryCache = memoryCache;     
        }

        private string GetStockHistoryKey(string stockId, DateTime date)
        {
            return $"{stockId}_{date.ToString("yyyy-MM-dd")}";
        }

        public async ValueTask<int> AddStockHistory(string stockId, List<StockParams> stockHistoryData)
        {
            var enumType = EnumCacheType.StockByDateRangeSavedToDB;
            var rowsToAdd = new List<StockParams>();
         
            foreach (var stH in stockHistoryData)
            {
                var key = _cacheHelperService.GetCacheKey(enumType, GetStockHistoryKey(stockId, stH.DateTime));
                if (!_memoryCache.TryGetValue(key, out bool exist))
                {
                    rowsToAdd.Add(stH);
                   
                }
            }
            if (rowsToAdd.Count == 0)
                return 0;

            var existingDates = (await _stockHistoryRepository.GetExistingHistoryData(stockId, rowsToAdd.Select(x => x.DateTime).ToList())).ToHashSet();
            var newHistoryData = new List<StockHistory>();

            foreach (var r in rowsToAdd)
            {
                if (existingDates.Contains(r.DateTime))
                {
                    var key = _cacheHelperService.GetCacheKey(enumType, GetStockHistoryKey(stockId, r.DateTime));
                    _memoryCache.Set(key, true, new TimeSpan(24, 0, 0));
                }
                else
                {
                    newHistoryData.Add(new StockHistory(stockId, r.DateTime, r.OpenPrice, r.ClosePrice));
                }
            }
            
            if (newHistoryData.Count == 0)
            {
                return 0;
            }

            var countRows = await _stockHistoryRepository.AddStockHistory(newHistoryData);
            foreach (var nr in newHistoryData)
            {
                var key = _cacheHelperService.GetCacheKey(enumType, GetStockHistoryKey(stockId, nr.DateTime));
                _memoryCache.Set(key, true, new TimeSpan(24, 0, 0));
            }
           
            return newHistoryData.Count;
        }
    }
}
