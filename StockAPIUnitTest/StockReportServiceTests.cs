using StockTestAPI.Services;
using StockTestAPI.Services.Interfaces;

namespace StockAPIUnitTest
{
    public class StockReportServiceTests
    {
        private IStockReportService GetStockReportService()
        {
            return new StockReportService();
        }


        [Test]
        public void GetCalculatedStockPerfomance_CheckNullInputParams_ReturnEmptyResult()
        {
            var service = GetStockReportService();
            var result = service.GetCalculatedStockPerfomance(new List<StockTestAPI.DTO.StockParams>());
            Assert.That(result, Is.Empty);
        }
        [Test]
        public void GetCalculatedStockPerfomance_CheckEmptyInputParams_ReturnEmptyResult()
        {
            var service = GetStockReportService();
            var result = service.GetCalculatedStockPerfomance(new List<StockTestAPI.DTO.StockParams>());
            Assert.That(result, Is.Empty);
        }
        [Test]
        public void GetCalculatedStockPerfomance_FirstPriceIsZero_ReturnResultWithZero()
        {
            var service = GetStockReportService();
            var data = new List<StockTestAPI.DTO.StockParams>
            {
                new StockTestAPI.DTO.StockParams { DateTime = DateTime.UtcNow.AddDays(-5), ClosePrice = 0 },
                new StockTestAPI.DTO.StockParams { DateTime = DateTime.UtcNow.AddDays(-4), ClosePrice = 10 },
                new StockTestAPI.DTO.StockParams { DateTime = DateTime.UtcNow.AddDays(-3), ClosePrice = 50 }
            };
            var result = service.GetCalculatedStockPerfomance(data);
            Assert.AreEqual(0, result.Sum(x => x.Value));
        }
        [Test]
        public void GetCalculatedStockPerfomance_CheckResult()
        {
            var service = GetStockReportService();
            var data = new List<StockTestAPI.DTO.StockParams>
            {
                new StockTestAPI.DTO.StockParams { DateTime = DateTime.UtcNow.AddDays(-5), ClosePrice = 100 },
                new StockTestAPI.DTO.StockParams { DateTime = DateTime.UtcNow.AddDays(-4), ClosePrice = 120 },
                new StockTestAPI.DTO.StockParams { DateTime = DateTime.UtcNow.AddDays(-3), ClosePrice = 110 },
                new StockTestAPI.DTO.StockParams { DateTime = DateTime.UtcNow.AddDays(-2), ClosePrice = 95 }
            };
            //
            var expecedPerfomance = new decimal[] { 0, 20, 10, -5 };
            var expecedTimeStamp = data.Select(x => (int)x.DateTime.Subtract(DateTime.UnixEpoch).TotalSeconds).ToArray();
            var result = service.GetCalculatedStockPerfomance(data);
            Assert.That(result.Count, Is.EqualTo(expecedPerfomance.Length));

            for(int i =0; i < expecedTimeStamp.Length; i++)
            {
                Assert.That(result[expecedTimeStamp[i]], Is.EqualTo(expecedPerfomance[i]));
               
            }

        }
    }
}