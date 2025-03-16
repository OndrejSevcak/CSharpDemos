using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockPriceAPI.Models;

namespace StockPriceAPI.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly List<Stock> _stocks;

        public StockController()
        {
            _stocks = new()
            {
                new Stock { Symbol = "AAPL", Price = 150.00m },
                new Stock { Symbol = "GOOG", Price = 2800.00m },
                new Stock { Symbol = "TSLA", Price = 700.00m }
            };
        }

        [HttpGet("live")]
        public ActionResult<IEnumerable<Stock>> GetLivePrices()
        {
            return Ok(_stocks);
        }
    }
}
