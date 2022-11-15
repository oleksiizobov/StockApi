using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockTestAPI.DTO;
using StockTestAPI.Infrastructure.Repositories.Interfaces;

namespace StockTestAPI.Infrastructure.Repositories
{
    public class StockApiClientRepository : IStockApiClientService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public StockApiClientRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

        }

        private List<StockParams> DeserialaizeStocksParams(IEnumerable<JToken> timeSeries)
        {
            var result = new List<StockParams>();
            foreach (var d in (IEnumerable<JToken>)timeSeries)
            {
                var stParams = new StockParams
                {
                    DateTime = DateTime.Parse(d.Path.Replace("[", "").Replace("]", "").Replace("'", "")),
                };
                var props = d.First().ToList();
                foreach (var p in props)
                {
                    if (p.ToString().Contains("open"))
                    {
                        stParams.OpenPrice = Convert.ToDecimal(p.First());
                    }
                    else if (p.ToString().Contains("close"))
                    {
                        stParams.ClosePrice = Convert.ToDecimal(p.First());
                    }
                }
                result.Add(stParams);
            }
            return result;
        }

        public async ValueTask<List<StockParams>> GetStockPriceByHour(string stockId, int lastDaysCount)
        {
            var url = _configuration["StockApiURL"];
            var key = _configuration["StockApiKey"];
            string queryUrl = $"{url}/query?function=TIME_SERIES_INTRADAY&symbol={stockId}&interval=60min&apikey={key}";
            var response = await _httpClient.GetAsync(queryUrl);
            var str = await response.Content.ReadAsStringAsync();
            var keyVal = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str);
            if (keyVal == null)
                return new List<StockParams>();
            var timeSeries = keyVal.Where(x => x.Key.Contains("Time Series")).Select(x => x.Value).FirstOrDefault();
            if (timeSeries == null)
                return new List<StockParams>();

            var result = DeserialaizeStocksParams((IEnumerable<JToken>)timeSeries);
           
            var days = result.Select(x => x.DateTime.Date).Distinct().OrderByDescending(x => x).Take(lastDaysCount).ToHashSet();
            result = result.Where(x => days.Contains(x.DateTime.Date)).OrderBy(x=>x.DateTime).ToList();

            return result;
        }


        public async ValueTask<List<StockParams>> GetStockPriceByDate(string stockId, int lastDaysCount)
        {
            var url = _configuration["StockApiURL"];
            var key = _configuration["StockApiKey"];
            string queryUrl = $"{url}/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={stockId}&interval=60min&apikey={key}";
            var response = await _httpClient.GetAsync(queryUrl);
            var str = await response.Content.ReadAsStringAsync();
            var keyVal = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str);
            if (keyVal == null)
                return new List<StockParams>();
            var timeSeries = keyVal.Where(x => x.Key.Contains("Time Series")).Select(x => x.Value).FirstOrDefault();
            if (timeSeries == null)
                return new List<StockParams>();

            var result = DeserialaizeStocksParams((IEnumerable<JToken>)timeSeries);
            result = result.OrderByDescending(x => x.DateTime).Take(lastDaysCount).OrderBy(x => x.DateTime).ToList();


            return result;
        }
    }
}
