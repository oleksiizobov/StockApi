using StockTestAPI.DTO;
using StockTestAPI.Services.Interfaces;

namespace StockTestAPI.Services
{
    public class StockReportService : IStockReportService
    {
        public StockReportService()
        {

        }
        public Dictionary<int, decimal> GetCalculatedStockPerfomance(List<StockParams> stockHistoryData)
        {
            var result = new Dictionary<int, decimal>();
            if (stockHistoryData == null)
            {
                return result;
            }
            if (stockHistoryData.Count == 0)
            {
                return result;
            }
            var firstTimePoint = stockHistoryData.First(); ;
            var firstPriceValue = firstTimePoint.ClosePrice;
            decimal perfomanceBuff = 0;
            foreach (var stParams in stockHistoryData)
            {
                perfomanceBuff = Math.Round(firstPriceValue > 0 ? stParams.ClosePrice * 100 / firstPriceValue - 100 : 0, 4);
                result.Add((int)stParams.DateTime.Subtract(DateTime.UnixEpoch).TotalSeconds, perfomanceBuff);
            }
            return result;
        }

    }
}
