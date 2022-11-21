namespace StockTestAPI.DTO
{
    public class StockParams
    {
        public string StockId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
    }
}
