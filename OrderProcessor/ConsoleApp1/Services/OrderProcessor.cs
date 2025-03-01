//create a service class that will be used to process orders in parallel in the same order as they are received
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1.Services;

public class OrderProcessor
{
    //The Producer/Consumer pattern is a design pattern used in multithreaded applications to manage shared resources efficiently. It involves two types of threads:
    // - Producer: Generates data and places it into a shared buffer or queue.
    // - Consumer: Retrieves data from the buffer and processes it

    private readonly BlockingCollection<Order> _orders = new BlockingCollection<Order>();
    private readonly ILogger<OrderProcessor> _logger;

    public OrderProcessor(ILogger<OrderProcessor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// The ProcessOrdersSequentially method processes orders sequentially in the order they are received. 
    /// It uses a single consumer task to process orders one by one.
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    public Task ProcessOrdersSequentially(IEnumerable<Order> orders, CancellationToken cancellationToken)
    {
        // Create a producer task that adds orders to the blocking collection
        var producerTask = Task.Run(() =>
        {
            try
            {
                foreach (var order in orders)
                {
                    _orders.Add(order, cancellationToken);
                    Console.WriteLine($"Added order {order.OrderId} to the queue on thread {Task.CurrentId}");
                }
            }
            catch(OperationCanceledException)
            {
                _logger.LogInformation("The operation was canceled.");
            }
            finally
            {
                _orders.CompleteAdding();
            }
        }, cancellationToken);

        // Create a single consumer task to process orders sequentially
        var consumerTask = Task.Run(() =>
        {
            try
            {
                foreach (var order in _orders.GetConsumingEnumerable(cancellationToken))
                {
                    ProcessOrder(order, cancellationToken);
                }
            }
            catch(OperationCanceledException)
            {
                _logger.LogInformation("The operation was canceled.");
            }
        }, cancellationToken);

        return Task.WhenAll(producerTask, consumerTask);
    }

    /// <summary>
    /// The ProcessOrdersConcurently method processes orders concurrently using multiple consumer tasks.
    /// It creates a producer task to add orders to the blocking collection and multiple consumer tasks to process orders concurrently.
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    public Task ProcessOrdersConcurently(IEnumerable<Order> orders, CancellationToken cancellationToken)
    {
        // Create a producer task that adds orders to the blocking collection
        var producerTask = Task.Run(() =>
        {
            try
            {
                foreach (var order in orders)
                {
                    _orders.Add(order, cancellationToken);
                    Console.WriteLine($"Added order {order.OrderId} to the queue on thread {Task.CurrentId}");
                }
            }
            catch(OperationCanceledException)
            {
                _logger.LogInformation("The operation was canceled.");
            }
            finally
            {
                _orders.CompleteAdding();
            }
        }, cancellationToken);

        // Create multiple consumer tasks to process orders concurrently
        var consumerTasks = Enumerable.Range(0, Environment.ProcessorCount).Select(_ => Task.Run(() =>
        {
            try
            {
                foreach (var order in _orders.GetConsumingEnumerable())
                {
                    ProcessOrder(order, cancellationToken);
                }
            }
            catch(OperationCanceledException)
            {
                _logger.LogInformation("The operation was canceled.");
            }

        }, cancellationToken)).ToArray();

        return Task.WhenAll(producerTask, Task.WhenAll(consumerTasks));
    }

    private void ProcessOrder(Order order, CancellationToken cancellationToken)
    {
        try
        {
            // Simulate processing time
            Task.Delay(500, cancellationToken).Wait(cancellationToken);
            Console.WriteLine($"Processed order {order.OrderId} on thread {Task.CurrentId}");
        }
        catch(OperationCanceledException)
        {
            _logger.LogInformation("The operation was canceled.");
        }

    }
}