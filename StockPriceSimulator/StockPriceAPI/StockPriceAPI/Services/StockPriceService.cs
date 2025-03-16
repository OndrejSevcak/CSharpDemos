
using Microsoft.AspNetCore.SignalR;
using StockPriceAPI.Models;

namespace StockPriceAPI.Services
{
    public class StockPriceService : BackgroundService
    {
        private readonly IHubContext<StockHub> _hubContext;
        private readonly Random _random = new Random();
        private readonly List<Stock> _stocks = new()
        {
            new Stock { Symbol = "AAPL", Price = 150.00m },
            new Stock { Symbol = "GOOG", Price = 2800.00m },
            new Stock { Symbol = "TSLA", Price = 700.00m }
        };

        public StockPriceService(IHubContext<StockHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var stock in _stocks)
                {
                    stock.Price += (decimal)(_random.NextDouble() * 10 - 5); // Random price fluctuation
                }
                await _hubContext.Clients.All.SendAsync("UpdateStockPrice", _stocks, stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
