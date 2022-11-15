using StockTestAPI.DTO;
using StockTestAPI.Infrastructure.Repositories.Interfaces;
using StockTestAPI.Services.Interfaces;

namespace StockTestAPI.Services
{
    public class StockReportModel : IStockReportModel
    {
        private readonly IStockApiClientService _stockApiClientService;
        private readonly IStockHistoryProxyRepository _stockHistoryProxyRepository;
        private readonly IConfiguration _configuration;
        private readonly IStockReportService _stockReportService;
        private readonly string _mainStockId;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public StockReportModel(IStockApiClientService stockApiClientService,
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            IStockHistoryProxyRepository stockHistoryProxyRepository,
            IConfiguration configuration,
            IStockReportService stockReportService
            )
        {
            _stockApiClientService = stockApiClientService;
            _stockHistoryProxyRepository = stockHistoryProxyRepository;
            _configuration = configuration;
            _stockReportService = stockReportService;
#pragma warning disable CS8601 // Possible null reference assignment.
            _mainStockId = _configuration["MainStockId"];
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public async Task<Dictionary<string, Dictionary<int, decimal>>> GetStockPerfomanceByDay(string stockId, int lastDaysCount)
        {
            var stockHistoryData = await _stockApiClientService.GetStockPriceByDate(stockId, lastDaysCount);
            var mainStockHistoryData = await _stockApiClientService.GetStockPriceByDate(_mainStockId, lastDaysCount);

            await _stockHistoryProxyRepository.AddStockHistory(stockId, stockHistoryData);
            await _stockHistoryProxyRepository.AddStockHistory(_mainStockId, mainStockHistoryData);

            var targetStockPerfomance = _stockReportService.GetCalculatedStockPerfomance(stockHistoryData);
            var mainStockPerfomance = _stockReportService.GetCalculatedStockPerfomance(mainStockHistoryData);

            var result = new Dictionary<string, Dictionary<int, decimal>>
            {
                { stockId, targetStockPerfomance },
                { _mainStockId, mainStockPerfomance }
            };

            return result;
        }
        public async Task<List<StockPerfomanceDto>> GetStockPerfomanceByHour(string stockId, int lastDaysCount)
        {
            var stockHistoryData = await _stockApiClientService.GetStockPriceByHour(stockId,  lastDaysCount);
            var mainStockHistoryData = await _stockApiClientService.GetStockPriceByHour(_mainStockId, lastDaysCount);

            var targetStock = new StockPerfomanceDto
            {
                StockId = stockId,
                Perfomance = _stockReportService.GetCalculatedStockPerfomance(stockHistoryData),
                HistoricalData = stockHistoryData.ToDictionary(x => (int)x.DateTime.Subtract(DateTime.UnixEpoch).TotalSeconds, z => z)
            };

            var mainStock = new StockPerfomanceDto
            {
                StockId = _mainStockId,
                Perfomance = _stockReportService.GetCalculatedStockPerfomance(mainStockHistoryData),
                HistoricalData = mainStockHistoryData.ToDictionary(x => (int)x.DateTime.Subtract(DateTime.UnixEpoch).TotalSeconds, z => z)
            };

            var result = new List<StockPerfomanceDto>
            {
                targetStock,
                mainStock
            };

            return result;
        }






    }
}
