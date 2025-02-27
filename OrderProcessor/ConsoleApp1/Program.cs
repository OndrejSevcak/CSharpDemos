using ConsoleApp1.Models;
using ConsoleApp1.Helpers;
using ConsoleApp1.Services;
using System.Diagnostics;

IEnumerable<Order>? orders = DataHelper.LoadOrders(@"C:\Users\ondre\OneDrive\Plocha\Temp\CSharpDevKitProjects\Console1\ConsoleApp1\Data\orders.json");

if(orders != null)
{
    Stopwatch sw = new Stopwatch();
    var orderProcessor = new OrderProcessor();
    
    sw.Start();
    orderProcessor.ProcessOrdersSequentially(orders).Wait();    //takes 100s
    sw.Stop();
    Console.WriteLine($"Processing time: {sw.ElapsedMilliseconds / 1000}s");

    orderProcessor = new OrderProcessor();
    sw.Restart();
    orderProcessor.ProcessOrdersConcurently(orders).Wait();    //takes 15s
    sw.Stop();
    Console.WriteLine($"Processing time: {sw.ElapsedMilliseconds / 1000}s");
}