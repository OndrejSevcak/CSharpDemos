using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ConsoleApp1.Models;

namespace ConsoleApp1.Helpers
{
    public static class DataHelper
    {
        public static IEnumerable<Order>? LoadOrders(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<IEnumerable<Order>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading orders: {ex.Message}");
                return null;
            }
        }
    }
}
