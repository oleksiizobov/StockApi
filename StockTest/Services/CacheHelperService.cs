using StockTestAPI.Services.Interfaces;

namespace StockTestAPI.Services
{
    public class CacheHelperService : ICacheHelperService
    {
        public string GetCacheKey(Enums.EnumCacheType cacheType, string cacheKey)
        {
            return $"{(int)cacheType}_{cacheKey}";
        }
    }
}
