namespace StockTestAPI.Domain
{
    public class StockHistory
    {
        public StockHistory(string stockId, DateTime dateTime, decimal openPrice, decimal closePrice)
        {
            StockId = stockId;
            DateTime = dateTime;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
        }

        public string StockId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
    }
}
