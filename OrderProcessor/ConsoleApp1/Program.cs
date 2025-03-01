using ConsoleApp1.Models;
using ConsoleApp1.Helpers;
using ConsoleApp1.Services;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IEnumerable<Order>? orders = DataHelper.LoadOrders(@"C:\Users\ondre\OneDrive\Plocha\GitHub Folders\CSharpDemos\OrderProcessor\ConsoleApp1\Data\orders.json");

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.AddConsole();
    })
    .AddTransient<OrderProcessor>()
    .BuildServiceProvider();

var logger = serviceProvider.GetService<ILogger<OrderProcessor>>();
logger.LogInformation("Applicatoin started...");

if(orders != null)
{
    Stopwatch sw = new Stopwatch();
    var orderProcessor = serviceProvider.GetService<OrderProcessor>();
    var cancellationTokenSource = new CancellationTokenSource();
    var cancellationToken = cancellationTokenSource.Token;
    
    sw.Start();
    orderProcessor.ProcessOrdersSequentially(orders, cancellationToken).Wait();    //takes 50s
    sw.Stop();
    Console.WriteLine($"Processing time: {sw.ElapsedMilliseconds / 1000}s");

    var orderProcessor2 = serviceProvider.GetService<OrderProcessor>();
    sw.Restart();
    orderProcessor2.ProcessOrdersConcurently(orders, cancellationToken).Wait();    //takes 4s
    sw.Stop();
    Console.WriteLine($"Processing time: {sw.ElapsedMilliseconds / 1000}s");
}
else
{
    Console.WriteLine("No orders to process.");
}