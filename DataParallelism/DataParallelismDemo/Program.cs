using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

//Data Parallelism - processing multiple data elements concurrently
Stopwatch stopwatch = new Stopwatch();

//For vs. Parallel.For
stopwatch.Start();
for (int i = 0; i < 10; i++)
{
    Task.Delay(1000).Wait();
}
stopwatch.Stop();
Console.WriteLine($"For loop took {stopwatch.ElapsedMilliseconds} ms"); //10112 ms

stopwatch.Restart();
Parallel.For(0, 10, i =>
{
    Task.Delay(1000).Wait();
});
stopwatch.Stop();
Console.WriteLine($"Parallel.For loop took {stopwatch.ElapsedMilliseconds} ms");    //1051 ms


//Parallel.ForEach
List<Item> items = new List<Item>();
var random = Random.Shared;

long demo1 = 0;
long demo2 = 0;

for(int i = 0; i < 1_000_000; i++)  //small data sets are not suitable for parallel processing
{
    var item = new Item { Id = i, Name = $"Item {i}" };
    items.Add(item);
}

Console.WriteLine("Data is ready for processing");

//sequential processing
stopwatch.Start();

foreach (var item in items)
{
    AddHashCode(item);
}

Console.WriteLine("Sequential processing is done");

stopwatch.Stop();
demo1 = stopwatch.ElapsedMilliseconds;

//parallel processing
stopwatch.Restart();

ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
Parallel.ForEach(items, options, item => AddHashCode(item));

stopwatch.Stop();
demo2 = stopwatch.ElapsedMilliseconds;

Console.WriteLine($"Sequential processing took {demo1} ms");    //17734 ms
Console.WriteLine($"Parallel processing took {demo2} ms");      //3872 ms


void AddHashCode(Item item)
{
    int code;  
    code = random.Next(1000, 9999); //here we are using Random.Shared which is thread-safe and available since .NET 6
    
    //simulating heavier workload
    for (int i = 0; i < 10; i++)
    {
        item.HashCode = $"{HashNumber(code)}";
    }    
}

static string HashNumber(int input)
{
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}

class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? HashCode { get; set; }
}



