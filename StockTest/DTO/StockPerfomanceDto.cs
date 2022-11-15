namespace StockTestAPI.DTO
{
    public class StockPerfomanceDto
    {
        public string StockId { get; set; }
        public Dictionary<int, decimal> Perfomance{ get; set; }
        public Dictionary<int, StockParams> HistoricalData { get; set; }
    }
}
