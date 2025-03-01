using System;
using System.Threading.Tasks;

namespace ParallelDemo
{
    public static class ParallelClass
    {
        public static void RunFor()
        {
            Parallel.For(0, 10, i =>
            {
                Console.WriteLine($"Task {i} started");
                Task.Delay(1000).Wait();
                Console.WriteLine($"Task {i} completed");
            });
        }

        // public static async Task RunForAsync()
        // {
        //     ParallelOptions options = new() { MaxDegreeOfParallelism = 2 };

        //     await Parallel.ForAsync(0, 10, options,  async (i) =>
        //     {
        //         Console.WriteLine($"Task {i} started");
        //         await Task.Delay(1000);
        //         Console.WriteLine($"Task {i} completed");
        //     });
        // }
    }
}