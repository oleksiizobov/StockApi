using static StockTestAPI.Enums;

namespace StockTestAPI.Services.Interfaces
{
    public interface ICacheHelperService
    {
         string GetCacheKey(EnumCacheType cacheType,  string cacheKey);
    }
}
