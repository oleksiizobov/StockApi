using Microsoft.AspNetCore.Mvc;
using StockTestAPI.Services.Interfaces;

namespace StockTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockReportController : Controller
    {
        private readonly ILogger<StockReportController> _logger;
        private readonly IStockReportModel _stockModel;

        public StockReportController(ILogger<StockReportController> logger,
            IStockReportModel stockModel
            )
        {
            _logger = logger;
            _stockModel = stockModel;

        }
        [HttpGet]
        [Route("stockPerfomanceByLastWeekByDay")]
        public async Task<IActionResult> GetStockPerfomanceByLastWeekByDay(string stockId)
        {          
            var result = await _stockModel.GetStockPerfomanceByDay(stockId, lastDaysCount: 7);
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("stockPerfomanceByLastWeekByHour")]
        public async Task<IActionResult> GetStockPerfomanceByLastWeekByHour(string stockId)
        {
            var result = await _stockModel.GetStockPerfomanceByHour(stockId, lastDaysCount: 7);
            return new JsonResult(result); ;
        }


    }
}
