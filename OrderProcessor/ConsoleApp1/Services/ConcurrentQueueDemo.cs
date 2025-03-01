//ConcurrentQueue<T>:
//Type: thread-safe, FIFO (First-In-First-Out) collection.
//Blocking: does not provide blocking operations. If you try to dequeue from an empty queue, it will return immediately with a failure.
//Usage: Suitable for scenarios where you need a non-blocking, thread-safe queue.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.Services;

public class ConcurrentQueueDemo
{
    private readonly ConcurrentQueue<int> _queue = new();

    public void Run()
    {
        AddItems();

        if(!_queue.TryPeek(out var result)) //inspect the top item without removing it
        {
            Console.WriteLine("Queue is empty");
        }
        else
        {
            Console.WriteLine($"Peeked {result}");
        }

        int outerSum = 0;
        Action processItems = () => 
        {
            int localSum = 0;
            while(_queue.TryDequeue(out int item)){
                localSum += item;
                Console.WriteLine($"Dequeued {item}");
            }
            Interlocked.Add(ref outerSum, localSum);    //atomic operation working on a thread shared variable
        };

        Parallel.Invoke(processItems, processItems, processItems, processItems);

    }

    private void AddItems()
    {
        for (var i = 0; i < 100; i++)
        {
            _queue.Enqueue(i);
            Console.WriteLine($"Added {i}");
        }        
    }

    
}